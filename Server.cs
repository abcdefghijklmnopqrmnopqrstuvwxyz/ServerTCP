using Server.commands;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Server
{
    internal class Server
    {
        private string Blocked = "D:\\Boháč_C3b\\Server\\depc\\Blocked.json";
        private Dictionary<string, ICommand> myCommands = new Dictionary<string, ICommand>();
        private TcpListener myServer;
        private bool isRunning;

        public Server(int port)
        {
            myServer = new TcpListener(System.Net.IPAddress.Any, port);
            myServer.Start();
            isRunning = true;
            Login.IP = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
            myCommands.Add("who", new WhoCommand());
            myCommands.Add("last", new LastCommand());
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

        private void BlockIp()
        {
            List<string> ips = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(Blocked));
            ips.Add(Login.IP);
            string Text = JsonSerializer.Serialize(ips);
            File.WriteAllText(Blocked, Text);
        }

        private List<string> GetBlockedIp()
        { 
            return JsonSerializer.Deserialize<List<string>>(File.ReadAllText(Blocked));
        }

        private bool IsBlocked()
        {
            foreach (var x in GetBlockedIp())
            {
                if (Login.IP == x)
                {
                    return true;
                }
            }
            return false;
        }

        private void ClientLoop(TcpClient client)
        {
            StreamReader reader = new StreamReader(client.GetStream(), Encoding.UTF8);
            StreamWriter writer = new StreamWriter(client.GetStream(), Encoding.UTF8);

            Login login = new Login();

            if (!IsBlocked())
            {
                for (int i = 0; i < 3; i++)
                {
                    writer.Write("Name: ");
                    writer.Flush();
                    string name = reader.ReadLine();
                    writer.Write("Pssword: ");
                    writer.Flush();
                    string pass = reader.ReadLine();
                    if (login.TryLogIn(name, pass))
                    {
                        login.LogUser(name);
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

                if (!IsBlocked())
                {
                    bool clientConnect = true;
                    string? data;
                    while (clientConnect)
                    {
                        data = reader.ReadLine();
                        data = data?.ToLower();
                        if (myCommands.ContainsKey(data))
                        {
                            writer.WriteLine(myCommands[data].Execute());
                            writer.Flush();
                        }
                        else if (data.Equals("exit"))
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
                else
                {
                    writer.WriteLine("Your IP is blocked;");
                    writer.Flush();
                }
            }
            else
            {
                writer.WriteLine("Your IP is blocked;");
                writer.Flush();
            }

            writer.WriteLine("Disconnected;");
            writer.Flush();
        }

    }
}