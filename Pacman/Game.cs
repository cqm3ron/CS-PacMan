using System.Diagnostics;

namespace Pacman
{
    internal class Game
    {
        private GameBoard Board;
        private Thread DisplayThread;
        private bool PacManNeedsRedraw = false;
        public void Start()
        {
            Board = new();
            Board.Draw();

            DisplayThread = new Thread(DisplayLoop);
            DisplayThread.Start();

            Stopwatch movementClock = new();
            movementClock.Start();

            while (true)
            {
                if (movementClock.ElapsedMilliseconds >= 250)
                {
                    if (Board.GetPacMan().MoveForwards(Board))
                    {
                        Board.UpdatePacManPosition();
                        PacManNeedsRedraw = true;
                    }
                    movementClock.Restart();
                }
            }
        }
        private void InputLoop()
        {
            ConsoleKey? key = null;
            while (Console.KeyAvailable)
                key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.W || key == ConsoleKey.UpArrow)
            {
                _ = Board.GetPacMan().TryChangeDirection(Entity.Direction.Up);
            }
            else if (key == ConsoleKey.S || key == ConsoleKey.DownArrow)
            {
                _ = Board.GetPacMan().TryChangeDirection(Entity.Direction.Down);
            }
            else if (key == ConsoleKey.A || key == ConsoleKey.LeftArrow)
            {
                _ = Board.GetPacMan().TryChangeDirection(Entity.Direction.Left);
            }
            else if (key == ConsoleKey.D || key == ConsoleKey.RightArrow)
            {
                _ = Board.GetPacMan().TryChangeDirection(Entity.Direction.Right);
            }
        }
        private void DisplayLoop()
        {
            Stopwatch displayClock = new();
            displayClock.Start();
            while (true)
            {
                if (displayClock.ElapsedMilliseconds >= 1000 / 60)
                {
                    InputLoop();
                    if (PacManNeedsRedraw)
                    {
                        Board.DrawPacMan();
                        PacManNeedsRedraw = false;
                    }

                    displayClock.Restart();
                }
            }
        }
    }
}
