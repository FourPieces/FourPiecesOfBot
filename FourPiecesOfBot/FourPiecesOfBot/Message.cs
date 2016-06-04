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
        
        /* an IRC message has 5 parts in total
            1: PING
            2: User?
            3: Target
            4: Command
            5: Message body 
        */
        private string[] extractedData = new string[5];
        
        public Message(string data)
        {
            Console.WriteLine(data);

            //split the data into individual words
            string[] tempData = data.Split(separator);

            //put the user's message back together
            this.ParseMessage(tempData);
        }

        private void ParseMessage(string[] tempData)
        {
            string result = null;

            //put the first 4 parts in the extractedData as they are
            for(int i=0; i<4; i++)
            {
                if (i >= tempData.Length)
                    break;
                extractedData[i] = tempData[i];
            }

            //the fifth one is the actual message, so piece it all back together before storing it
            for(int i = 4; i < tempData.Length; i++)
            {
                result = result + tempData[i] + ' ';
            }
            extractedData[4] = result;
        }

        //return the index+1 part of the extracted message
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

        //did the server ping us?
        public bool CheckPing()
        {
            if (extractedData[0] == "PING")
                return true;
            return false;
        }
    }
}
