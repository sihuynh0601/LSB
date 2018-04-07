using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LSB
{
    class LSBHelper
    {
        public static void Encode(FileStream inStream, byte[] message, FileStream outStream)
        {
            int byteRead;       //1 byte doc vao tu inStream ( phai dung kieu int do ReadByte() tra ve kieu byte)
            byte byteWrite;     // 1 byte de viet vao ouStream
            int i = 0;          // chay trong mang byte Message
            int j = 0;          //chay tung bit trong 1 byte Message[i]
            while ((byteRead = inStream.ReadByte()) != -1)    // trong khi con chua ket thuc Stream
            {
                byteWrite = (byte)byteRead; // cast (ep kieu)

                if (i < message.Length)       // thong diep van con
                {
                    byte bit = BitOperation.Extract(message[i], j++);   // trich 1 bit tu vi tri j tu message[i] ra
                    BitOperation.Replace(ref byteWrite, 0, bit);            // thay the bit vao vi tri 0 (LSB)                                         
                    if (j == 8) { j = 0; i++; }    // da trich het 8 bit cua message[i]
                }
                //viet ra ouStream (co nhung truong hop nhung Byte cuoi khong bi thay doi
                outStream.WriteByte(byteWrite);
            }

            if (i < message.Length) 
                throw new Exception("Thong diep qua lon de giau");
        }

        public static byte[] Decode(FileStream inStream, int length) 
        {
            int byteIndex = 0;  
            int bitIndex = 0;  
            byte[] arrResult = new byte[length];
            int byteRead;  
            while ((byteRead = inStream.ReadByte()) != -1)
            {
                byte bit = BitOperation.Extract((byte)byteRead, 0); 
                
                BitOperation.Replace(ref arrResult[byteIndex], bitIndex++, bit);
                if (bitIndex == 8)    
                {
                    bitIndex = 0;
                    byteIndex++;
                }
                if (byteIndex == length) break; 
            }
            return arrResult;
        }
    }
}
