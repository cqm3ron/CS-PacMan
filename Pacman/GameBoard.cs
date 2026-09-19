using Pacman.Entities;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Pacman
{
    internal class GameBoard
    {
        // Enums
        private enum SpawnLocations
        {
            PacMan,
            RedGhost,
            BlueGhost,
            PinkGhost,
            YellowGhost
        }

        // Boolean Properties

        // Other Properties
        private int PelletsCollected, PowerPelletsCollected, PelletsOnBoard, PowerPelletsOnBoard;
        private PacMan PacMan;
        private RedGhost RedGhost;
        private BlueGhost BlueGhost;
        private PinkGhost PinkGhost;
        private YellowGhost YellowGhost;
        private Cell[,] Tiles;

        // Constants & readonly Properties
        private readonly (int x, int y) origin;

        // Methods

        public void UpdatePacManPosition()
        {
            (int x, int y) position = PacMan.GetPosition();
            (int x, int y) previous = PacMan.GetPreviousPosition();
            Tiles[previous.x, previous.y].TryVacate(PacMan);
            Tiles[position.x, position.y].TryOccupy(PacMan);
            if (CheckCollision(position)) PacMan.Collide();
            TryEatPellet(position);
        }
        public bool TryEatPellet((int x, int y) position)
        {
            if (Tiles[position.x, position.y].HasPellet)
            {
                if (Tiles[position.x, position.y].EatPellet())
                {
                    PacMan.EatPellet();
                    PelletsCollected++;
                    return true;
                }
                else return false;
            }
            else if (Tiles[position.x, position.y].HasPowerPellet)
            {
                if (Tiles[position.x, position.y].EatPellet())
                {
                    PacMan.EatPowerPellet();
                    PowerPelletsCollected++;
                    return true;
                }
                else return false;
            }
            else return false;
        }
        private bool CheckCollision((int x, int y) position)
        {
            if (Tiles[position.x, position.y].Occupied)
            {
                Entity? occupier = Tiles[position.x, position.y].GetOccupier();
                if (occupier != null && occupier is not Entities.PacMan)
                    return true;
            }
            return false;
        }

        // Getters
        public int GetWidth() => Tiles.GetLength(0);
        public int GetHeight() => Tiles.GetLength(1);
        public string GetPelletInfo()
        {
            return $"Collected {PelletsCollected} of {PelletsOnBoard} pellets & {PowerPelletsCollected} of {PowerPelletsOnBoard} power pellets.";
        }
        public PacMan GetPacMan() => PacMan;
        
        // Boolean Methods
        public bool PacManWalkableTile((int x, int y) position)
        {
            if (position.x < 0 || position.x >= GetWidth()) return true;
            return Tiles[position.x, position.y].IsWalkableByPacman;
        }

        // Constructors
        public GameBoard(bool random = false) // TODO: implement random board generation
        {
            Tiles = new Cell[0, 0];
            PelletsOnBoard = 0;
            PowerPelletsOnBoard = 0;
            if (!random)
            {
                TryImportBoard("Assets/Boards/board1.txt");
            }
            origin = (0, 0);
            PelletsCollected = 0;
            PowerPelletsCollected = 0;
        }

        // Board Management
        private bool TryImportBoard(string boardPath)
        {
            if (!File.Exists(boardPath)) return false;
            if (Path.GetExtension(boardPath) != ".txt") return false;

            string[] importedText = File.ReadAllLines(boardPath);

            List<string[]> importedLineList = [];
            foreach (string line in importedText)
            {
                string[] importedLine = line.Split(',');
                importedLineList.Add(importedLine);
            }

            Cell[,] board = new Cell[importedLineList[0].Length, importedLineList.Count];
            
            for (int y = 0; y < importedLineList.Count; y++)
            {
                for (int x = 0; x < importedLineList[y].Length; x++)
                {
                    Cell cellToAdd;
                    switch (importedLineList[y][x])
                    {
                        case "w":
                            cellToAdd= new Cell(Cell.CellType.Wall);
                            break;
                        case "e":
                            cellToAdd = new Cell(Cell.CellType.Empty);
                            break;
                        case "d":
                            cellToAdd = new Cell(Cell.CellType.Door);
                            break;
                        case "p":
                            cellToAdd = new Cell(Cell.CellType.Empty, hasPellet:true);
                            PelletsOnBoard++;
                            break;
                        case "P":
                            cellToAdd = new Cell(Cell.CellType.Empty, hasPowerPellet: true);
                            PowerPelletsOnBoard++;
                            break;
                        case "M":
                            cellToAdd = new Cell(Cell.CellType.Empty);
                            PacMan = new PacMan((x, y));
                            cellToAdd.TryOccupy(PacMan);
                            break;
                        case "R":
                            cellToAdd = new Cell(Cell.CellType.Empty);
                            RedGhost = new RedGhost((x, y));
                            cellToAdd.TryOccupy(RedGhost);
                            break;
                        case "B":
                            cellToAdd = new Cell(Cell.CellType.Empty);
                            BlueGhost = new BlueGhost((x, y));
                            cellToAdd.TryOccupy(BlueGhost);
                            break;
                        case "K":
                            cellToAdd = new Cell(Cell.CellType.Empty);
                            PinkGhost= new PinkGhost((x, y));
                            cellToAdd.TryOccupy(PinkGhost);
                            break;
                        case "Y":
                            cellToAdd = new Cell(Cell.CellType.Empty);
                            YellowGhost = new YellowGhost((x, y));
                            cellToAdd.TryOccupy(YellowGhost);
                            break;
                        default:
                            cellToAdd = new Cell(Cell.CellType.Empty);
                            break;
                    }

                    board[x, y] = cellToAdd;
                }
            }

            Tiles = board;

            return true;
        }

        // Display Methods
        public void Draw()
        {
            Console.SetCursorPosition(origin.x, origin.y);
            for (int y = 0; y < Tiles.GetLength(1); y++)
            {
                for (int x = 0; x < Tiles.GetLength(0); x++)
                {
                    WriteCell((x, y));
                    Console.ResetColor();
                }
            }
            Console.ResetColor();
        }
        public void DrawPacMan()
        {
            WriteCell(PacMan.GetPosition());
            WriteCell(PacMan.GetPreviousPosition());
        }
        private void WriteCell((int x, int y) tile)
        {
            (ConsoleColor foreground, ConsoleColor background, string top, string bottom) = Tiles[tile.x, tile.y].GetCellVisual(); // get the cell visual properties
            
            (int x, int y) startPos = (origin.x + (tile.x * 2), origin.y + (tile.y * 2)); // each cell is 2x2 chars so times each by 2 to get the start position, then add to origin
            
            Console.BackgroundColor = background; // do good colours :thumbsup:
            Console.ForegroundColor = foreground;

            Console.SetCursorPosition(startPos.x, startPos.y); // go to the start position
            Console.Write(top);
            Console.SetCursorPosition(startPos.x, startPos.y + 1);
            Console.Write(bottom);
            Console.SetCursorPosition(startPos.x + 1, startPos.y); // go to the top right of this cell to finish off as we know it is in range
        }
    }
}
