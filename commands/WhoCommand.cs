namespace Server.commands
{
    internal class WhoCommand : ICommand
    {
        private static string _name;

        public static string Name { get => _name; set => _name = value; }

        public List<string> Args { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string Execute()
        {
            return "Currently connected as: " + Name + ";";
        } 

    }
}
