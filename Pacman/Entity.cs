using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman
{
    internal class Entity
    {
        // Enums
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        // Constructors
        public Entity((int x, int y) position)
        {
            Pos = position;
            Dir = Direction.Up;
        }

        // Properties
        private (int x, int y) pos;
        private protected (int x, int y) Pos
        {
            get => pos;
            set
            {
                PrevPos = pos;
                pos = value;
            }
        }
        private protected (int x, int y) PrevPos;
        private protected (int x, int y) SpawnPos;
        private Direction dir;
        private protected Direction Dir
        {
            get => dir;
            set
            {
                PrevDir = dir;
                dir = value;
            }
        }
        private protected Direction PrevDir;

        // Getters
        public (int x, int y) GetPosition() => Pos;
        public (int x, int y) GetPreviousPosition() => PrevPos;
        public (int x, int y) GetSpawnPosition() => SpawnPos;
        public Direction GetDirection() => Dir;
        public Direction GetPreviousDirection() => PrevDir;

        // Abstracts
        public virtual bool MoveForwards(GameBoard board) => false;
        public virtual (ConsoleColor foreground, ConsoleColor background, string top, string bottom) GetVisual() => (ConsoleColor.White, ConsoleColor.Black, "??", "??");
    }
}
