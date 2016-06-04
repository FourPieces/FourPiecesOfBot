using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FourPiecesOfBot
{
    class Program
    {
        static void Main(string[] args)
        {
            string password = null;
            try
            {
                password = File.ReadAllText(@"c:\botdata\oauth.txt", Encoding.UTF8);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            Config config;
            config.nick = "fourpiecesofbot";
            config.pass = password;
            config.channels = new string[] { "#fourpieces" };

            Bot bot = new Bot(config);
            bot.Connect();
            bot.Handle();
        }
    }
}
