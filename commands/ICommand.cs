namespace Server.commands
{
    internal interface ICommand
    {
        List<string> Args { get; set; }

        string Execute();
    }
}