namespace BingoBotEmbed;
public class Program
{
    static void Main(string[] args)
    {
        Bot bot = new();
        bot.RunAsync().GetAwaiter().GetResult();
    }
}