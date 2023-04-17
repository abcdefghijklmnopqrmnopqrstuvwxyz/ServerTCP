using System.Text.Json;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> ips = new();
            ips.Add("10.0.0.138");
            string Text = JsonSerializer.Serialize(ips);
            File.WriteAllText("D:\\Boháč_C3b\\Server\\depc\\Blocked.json", Text);
            Server server = new(65525);
        }

    }
}