using Pacman.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman
{
    internal class Game
    {
        private GameBoard Board;
        public void Start()
        {
            ConsoleKey key;
            Board = new();
            Board.Draw();
            while (true)
            {
                key = Console.ReadKey(true).Key;
                
                if (key == ConsoleKey.W || key == ConsoleKey.UpArrow)
                {
                    Board.GetPacMan().TryChangeDirection(Entity.Direction.Up);
                }
                else if (key == ConsoleKey.S || key == ConsoleKey.DownArrow)
                {
                    Board.GetPacMan().TryChangeDirection(Entity.Direction.Down);
                }
                else if (key == ConsoleKey.A || key == ConsoleKey.LeftArrow)
                {
                    Board.GetPacMan().TryChangeDirection(Entity.Direction.Left);
                }
                else if (key == ConsoleKey.D || key == ConsoleKey.RightArrow)
                {
                    Board.GetPacMan().TryChangeDirection(Entity.Direction.Right);
                }

                    if (Board.GetPacMan().MoveForwards(Board))
                {
                    Board.UpdatePacManPosition();
                    Board.DrawPacMan();
                }
            }
        }
    }
}
