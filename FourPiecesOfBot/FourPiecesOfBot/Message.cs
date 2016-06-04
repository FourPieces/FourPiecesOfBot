using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourPiecesOfBot
{
    class Message
    {
        public static readonly char[] separator = { ' ' };
        private string[] extractedData;

        public Message(string data)
        {
            Console.WriteLine(data);
            extractedData = data.Split(separator);
        }

        public string GetContent(int index)
        {
            if (index < extractedData.Length)
                return extractedData[index];
            else
                return null;
        }

        public int GetLength()
        {
            return extractedData.Length;
        }

        public bool CheckPing()
        {
            if (extractedData[0] == "PING")
                return true;
            return false;
        }
    }
}
