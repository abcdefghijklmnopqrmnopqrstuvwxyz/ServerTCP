namespace Server.commands
{
    internal class WhoCommand : ICommand
    {
        private static string _name;

        public static string Name { get => _name; set => _name = value; }

        public string Execute()
        {
            return "Currently connected as: " + Name + ";";
        } 

    }
}
