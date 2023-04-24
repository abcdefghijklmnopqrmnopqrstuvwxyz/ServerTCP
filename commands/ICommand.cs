namespace Server.commands
{
    internal interface ICommand
    {
        private List<string> Args
        {
            get
            {
                return new List<string>();
            }
            set
            {
                
            }
        }

        string Execute();
    }
}