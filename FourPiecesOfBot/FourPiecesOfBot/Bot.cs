using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;

namespace FourPiecesOfBot
{
    struct Config
    {
        public string nick;
        public string pass;
        public string[] channels;
        public const int port = 6667;
        public const string server = "irc.chat.twitch.tv";
    }

    class Bot
    {
        Config botConfig;
        TcpClient connection = null;
        StreamReader reader = null;
        StreamWriter writer = null;
        NetworkStream stream = null;

        public Bot(Config config)
        {
            this.botConfig = config;
        }

        public void Connect()
        {
            try
            {
                connection = new TcpClient(Config.server, Config.port);

                stream = connection.GetStream();
                reader = new StreamReader(stream);
                writer = new StreamWriter(stream);
                this.SendMsg("PASS", botConfig.pass);
                this.SendMsg("NICK", botConfig.nick);
                for(int i=0; i<botConfig.channels.Length; i++)
                {
                    this.SendMsg("JOIN", botConfig.channels[i]);
                }

                Console.WriteLine("Connected");
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.SocketErrorCode);
            }
        }

        private void SendMsg(string command, string args)
        {
            if(args == null)
            {
                writer.WriteLine(command);
                writer.Flush();
            }
            else
            {
                writer.WriteLine(command + " " + args);
                writer.Flush();
            }
        }

        public void Handle()
        {
            bool running = true;
            while (running)
            {
                string data = null;
                try
                {
                    data = reader.ReadLine();
                }
                catch
                {
                    Console.WriteLine("Unable to read from server");
                    return;
                }

                Message message = new Message(data);

                if (message.CheckPing())
                    this.SendMsg("PONG", message.GetContent(1));

                if (message.GetLength() > 3)
                {
                    string command = message.GetContent(3);

                    switch (command)
                    {
                        case (":!dogs"):
                            this.SendMsg("PRIVMSG", message.GetContent(2) + " :FrankerZ OhMyDog RalpherZ");
                            break;
                        case (":!say"):
                            this.SendMsg("PRIVMSG", message.GetContent(2) + " :" + message.GetContent(4));
                            break;
                        case (":!quit"):
                            this.SendMsg("QUIT", message.GetContent(4));
                            running = false;
                            this.CloseConnection();
                            break;
                    }
                }
                System.Threading.Thread.Sleep(2000);
            }
        }

        void CloseConnection()
        {
            if (reader != null)
                reader.Close();
            if (writer != null)
                writer.Close();
            if (stream != null)
                stream.Close();
            if (connection != null)
                connection.Close();
        }
    }
}
