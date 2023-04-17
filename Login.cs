using System.Text.Json;

namespace Server
{
    internal class Login
    {
        private string Path = "M:\\C#\\Server\\Depc\\Users.json";
        public static List<Users> User = new();

        public void AddUser(Users u)
        {
            User.Add(u);
        }

        public void SaveUsers()
        {
            string Text = JsonSerializer.Serialize(User);

            File.WriteAllText(Path, Text);
        }

        public List<Users> LoadUsers()
        {
            return JsonSerializer.Deserialize<List<Users>>(File.ReadAllText(Path));
        }

    }
}