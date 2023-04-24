using Server.commands;
using System.Text.Json;

namespace Server
{
    internal class Login
    {
        private string Path = "D:\\Boháč_C3b\\Server\\depc\\Users.json";
        private string Log = "D:\\Boháč_C3b\\Server\\depc\\Log.txt";
        private static List<Users> User = new();
        private static string _IP;

        public static string IP { get => _IP; set => _IP = value; }

        public void AddUser(Users u)
        {
            User.Add(u);
        }

        public void SaveUsers()
        {
            string Text = JsonSerializer.Serialize(User);

            File.WriteAllText(Path, Text);
        }

        public void LogUser(string name)
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

        public bool TryLogIn(string name, string pass)
        {
            List<Users> list = new Login().LoadUsers();

            foreach (var x in list)
            {
                if (x.Name == name && x.Password == pass)
                {
                    WhoCommand.Name = x.Name;
                    return true;
                }
            }
            return false;
        }

        public List<Users> LoadUsers()
        {
            return JsonSerializer.Deserialize<List<Users>>(File.ReadAllText(Path));
        }

    }
}