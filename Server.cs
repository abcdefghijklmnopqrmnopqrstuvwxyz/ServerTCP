using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Server
{
    internal class Server
    {
        private string Log = "D:\\Boháč_C3b\\Server\\depc\\Log.txt";
        private string Blocked = "D:\\Boháč_C3b\\Server\\depc\\Blocked.json";
        private TcpListener myServer;
        private bool isRunning;
        private string IP;
        private string user;

        public Server(int port)
        {
            myServer = new TcpListener(System.Net.IPAddress.Any, port);
            myServer.Start();
            isRunning = true;
            ServerLoop();
        }

        private void ServerLoop()
        {
            Console.WriteLine("Server started;");
            while (isRunning)
            {
                TcpClient client = myServer.AcceptTcpClient();
                ClientLoop(client);
            }
        }

        private void LogUser(string name)
        {
            try
            {
                FileStream fileStream = File.Open(Log, FileMode.Append, FileAccess.Write);
                StreamWriter fileWriter = new StreamWriter(fileStream);
                fileWriter.WriteLine(name + " - " + System.DateTime.Now.ToString() + " - " + IP);
                fileWriter.Flush();
                fileWriter.Close();
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex);
            }
        }

        private bool TryLogIn(string name, string pass)
        {
            Login l = new Login();
            List<Users> list = l.LoadUsers();

            foreach (var x in list)
            {
                if (x.Name == name && x.Password == pass)
                {
                    user = name;
                    return true;
                }
            }
            return false;
        }

        private void BlockIp()
        {
            try
            {
                List<string> ips = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(Blocked));
                ips.Add(IP);
                string Text = JsonSerializer.Serialize(ips);
                File.WriteAllText(Blocked, Text);
            }
            catch
            {
                Console.WriteLine("Blocked IPs is empty;");
            }
        }

        private List<string> GetBlockedIp()
        { 
            return JsonSerializer.Deserialize<List<string>>(File.ReadAllText(Blocked));
        }

        private void ClientLoop(TcpClient client)
        {
            StreamReader reader = new StreamReader(client.GetStream(), Encoding.UTF8);
            StreamWriter writer = new StreamWriter(client.GetStream(), Encoding.UTF8);

            string hostName = Dns.GetHostName();
            IP = Dns.GetHostByName(hostName).AddressList[0].ToString();

            bool blocked = false;

            foreach (var x in GetBlockedIp())
            {
                if (IP == x)
                {
                    writer.WriteLine("Your IP is blocked;");
                    writer.Flush();
                    blocked = true;
                }
            }

            if (!blocked)
            {
                for (int i = 0; i < 3; i++)
                {
                    writer.Write("Name: ");
                    writer.Flush();
                    string name = reader.ReadLine();
                    writer.Write("Pssword: ");
                    writer.Flush();
                    string pass = reader.ReadLine();
                    if (TryLogIn(name, pass))
                    {
                        LogUser(name);
                        writer.WriteLine("Connected;");
                        writer.Flush();
                        break;
                    }
                    writer.WriteLine("Incorrect name or password;");
                    writer.Flush();
                    if (i == 2)
                    {
                        writer.WriteLine("IP blocked;");
                        writer.Flush();
                        BlockIp();
                    }
                }

                if (!blocked)
                {
                    bool clientConnect = true;
                    string? data;
                    while (clientConnect)
                    {
                        data = reader.ReadLine();
                        data = data?.ToLower();
                        if (data == "who")
                        {
                            writer.WriteLine("Active user: " + user + ";");
                            writer.Flush();
                        }
                        else if (data == "uptime")
                        {

                        }
                        else if (data == "stats")
                        {

                        }
                        else if (data == "last")
                        {

                        }
                        else if (data == "exit")
                        {
                            clientConnect = false;
                        }
                        else
                        {
                            writer.WriteLine("Unknown command;");
                            writer.Flush();
                        }
                    }
                }
            }
            
            writer.WriteLine("Disconnected;");
            writer.Flush();
        }
    }

}