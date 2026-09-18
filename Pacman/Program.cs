namespace Pacman
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new();
            Console.ReadKey(true);
            game.Start();
        }
    }
}
