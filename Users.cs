namespace Server
{
    public class Users
    {
        private string _name;
        private string _password;

        public string Name { get => _name; set => _name = value; }
        public string Password { get => _password; set => _password = value; }

        public Users(string name, string password)
        {
            Name = name;
            Password = password;
        }

        public override string ToString()
        {
            return "Name: " + Name + "\nPassword: " + Password;
        }

    }
}