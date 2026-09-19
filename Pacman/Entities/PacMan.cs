using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pacman.Entities
{
    internal class PacMan((int x, int y) position) : Entity(position)
    {
        private int score = 0;
        private Thread PowerUpThread;
        private bool IsPoweredUp = false;
        private Stopwatch PowerUpTimer = new();
        private int PowerUpDurationSeconds = 6;
        public bool TryChangeDirection(Direction direction)
        {
            Dir = direction;
            if (Dir != PrevDir)
            {
                return true;
            }
            return false;
        }
        public override bool MoveForwards(GameBoard board)
        {
            bool hasMoved = false;
            (int x, int y) newPos = Pos;
            switch (Dir)
            {
                case Direction.Up:
                    if (board.PacManWalkableTile((Pos.x, Pos.y - 1)))
                    { 
                        newPos = (Pos.x, Pos.y - 1);
                        hasMoved = true;
                    }
                    break;
                case Direction.Down:
                    if (board.PacManWalkableTile((Pos.x, Pos.y + 1)))
                    {
                        newPos = (Pos.x, Pos.y + 1);
                        hasMoved = true;
                    }
                    break;
                case Direction.Left:
                    if (board.PacManWalkableTile((Pos.x - 1, Pos.y)))
                    {
                        if (Pos.x - 1 < 0)
                        {
                            newPos = (board.GetWidth() - 1, Pos.y);
                        }
                        else
                        {
                            newPos = (Pos.x - 1, Pos.y);
                        }
                        hasMoved = true;
                    }
                    break;
                case Direction.Right:
                    if (board.PacManWalkableTile((Pos.x + 1, Pos.y)))
                    {
                        if (Pos.x + 1 >= board.GetWidth())
                        {
                            newPos = (0, Pos.y);
                        }
                        else
                        {
                            newPos = (Pos.x + 1, Pos.y);
                        }
                        hasMoved = true;
                    }
                    break;
            }
            Pos = newPos;
            return hasMoved;
        }
        public override (ConsoleColor foreground, ConsoleColor background, string top, string bottom) GetVisual()
        {
            if (IsPoweredUp) return (ConsoleColor.Blue, ConsoleColor.Black, "CC", "CC");
            else return (ConsoleColor.Yellow, ConsoleColor.Black, "CC", "CC");
        }
        public void EatPellet() => score += 10;
        public void EatPowerPellet()
        {
            PowerUpThread = new Thread(PowerUpLoop);
            score += 50;
            IsPoweredUp = true;
            PowerUpThread.Start();
        }
        private void PowerUpLoop()
        {
            PowerUpTimer.Restart();
            Debug.WriteLine("Powerup");
            while (IsPoweredUp)
            {
                if (PowerUpTimer.ElapsedMilliseconds >= PowerUpDurationSeconds * 1000)
                {
                    IsPoweredUp = false;
                    PowerUpTimer.Stop();
                    Debug.WriteLine("No more powerup");
                    PowerUpThread.Join();
                }
            }
        }
        public void Collide()
        {
            
        }
    }
}
