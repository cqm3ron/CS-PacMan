using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman
{
    internal class Cell
    {
        // Enums
        public enum CellType
        {
            Empty = 0,              // 0
            Wall,                   // 1
            Door,                   // 2
        }

        // Boolean Properties
        public bool HasPellet { get; private set; }
        public bool HasPowerPellet { get; private set; }
        public bool HasAnyPellet => HasPellet || HasPowerPellet;
        public bool IsWalkableByPacman => PacmanWalkable();
        public bool IsWalkableByGhost => GhostWalkable();
        public bool Occupied => HasEntity();

        // Other Properties
        private CellType Type;
        private Entity? Occupier;

        // Private Methods
        private bool PacmanWalkable()
        {
            return Type switch
            {
                CellType.Empty => true,
                CellType.Door => false,
                CellType.Wall => false,
                _ => false
            };
        }
        private bool GhostWalkable()
        {
            return Type switch
            {
                CellType.Empty => true,
                CellType.Door => true,
                CellType.Wall => false,
                _ => false
            };
        }
        private bool HasEntity()
        {
            if (Occupier != null)
            {
                return true;
            }
            return false;
        }
        
        // Getters
        public Entity? GetOccupier()
        {
            if (HasEntity())
                return Occupier;
            else
                return null;
        }
        
        // Entity Methods
        public bool TryOccupy(Entity entity)
        {
            if (!Occupied)
            {
                Occupier = entity;
                return true;
            }
            return false;
        }
        public bool TryVacate(Entity entity)
        {
            if (Occupied && Occupier == entity)
            {
                Occupier = null;
                return true;
            }
            return false;
        }

        // Constructors
        public Cell(CellType type, bool hasPellet = false, bool hasPowerPellet = false)
        {
            Type = type;
            HasPellet = hasPellet;
            HasPowerPellet = hasPowerPellet;
        }
        
        // Display Methods
        public (ConsoleColor foreground, ConsoleColor background, string top, string bottom) GetCellVisual()
        {
            if (Occupied)
            {
                Entity? occupier = GetOccupier();
                
                if (occupier != null)
                    return occupier.GetVisual();
            }

            if (HasAnyPellet)
            {
                if (HasPellet)
                    return (ConsoleColor.Gray, ConsoleColor.Black, "  ", ". ");
                else if (HasPowerPellet)
                    return (ConsoleColor.White, ConsoleColor.Black, "  ", "o ");
            }

            switch (Type)
            {
                case CellType.Empty:
                    return (ConsoleColor.White, ConsoleColor.Black, "  ", "  ");
                case CellType.Wall:
                    return (ConsoleColor.White, ConsoleColor.DarkBlue, "  ", "  ");
                case CellType.Door:
                    return (ConsoleColor.DarkYellow, ConsoleColor.Black, "==", "==");
                default:
                    return (ConsoleColor.White, ConsoleColor.Black, "  ", "  ");
            }
        }
    }
}
