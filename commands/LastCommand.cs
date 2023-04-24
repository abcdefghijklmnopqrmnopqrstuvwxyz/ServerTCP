namespace Server.commands
{
    internal class LastCommand : ICommand
    {
        public List<string> Args { get => Args; set => Args = new List<string>(value); }
        private string Log = "D:\\Boháč_C3b\\Server\\depc\\Log.txt";

        public string Execute()
        {
            FileStream fileStream = File.Open(Log, FileMode.Open, FileAccess.Read);
            StreamReader fileReader = new StreamReader(fileStream);
            string line = fileReader.ReadLine();
            while (line != null)
            {
                Console.WriteLine(line);
                line = fileReader.ReadLine();
            }
            fileReader.Close();
            throw new NotImplementedException();
        }

    }
}
