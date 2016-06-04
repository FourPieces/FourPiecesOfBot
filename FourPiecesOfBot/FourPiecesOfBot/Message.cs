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
        private string[] extractedData = new string[5];
        
        public Message(string data)
        {
            Console.WriteLine(data);
            string[] tempData = data.Split(separator);
            this.ParseMessage(tempData);
        }

        private void ParseMessage(string[] tempData)
        {
            string result = null;
            for(int i=0; i<4; i++)
            {
                if (i >= tempData.Length)
                    break;
                extractedData[i] = tempData[i];
            }
            for(int i = 4; i < tempData.Length; i++)
            {
                result = result + tempData[i] + ' ';
            }
            extractedData[4] = result;
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
