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
                //set up all the fun socket things
                connection = new TcpClient(Config.server, Config.port);

                stream = connection.GetStream();
                reader = new StreamReader(stream);
                writer = new StreamWriter(stream);

                //send the nick and password, then join all the channels it's supposed to be in
                this.SendMsg("PASS", botConfig.pass);
                this.SendMsg("NICK", botConfig.nick);
                for(int i=0; i<botConfig.channels.Length; i++)
                {
                    this.SendMsg("JOIN", botConfig.channels[i]);
                }

                Console.WriteLine("Connected");
            }
            catch
            {
                Console.WriteLine("Error: Unable to connect");
            }
        }

        private void SendMsg(string command, string args)
        {
            try
            {
                if (args == null)
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
            catch
            {
                Console.WriteLine("Error: Unable to send data to writer");
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

                    Message message = new Message(data);

                    //respond to pings appropriately
                    if (message.CheckPing())
                        this.SendMsg("PONG", message.GetContent(1));
                        
                    
                    string command = message.GetContent(3);

                    switch (command)
                    {
                        //good dogs bark bark bark
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
                        case (null):
                            break;
                    }
                }

                catch
                {
                    Console.WriteLine("Unable to read from server");
                    return;
                }
                //sleep for 2 seconds to prevent bot abuse
                System.Threading.Thread.Sleep(2000);
            }
        }

        //close all the threads when you exit
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
