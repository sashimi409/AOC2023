using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{

    internal class Day10 : BaseDay
    {
        public List<List<MapPoint>> Maze;
        public int startX;
        public int startY;
        public int CurrentX;
        public int CurrentY;
        

        #region Helper Structs
        public struct MapPoint
        {
            public Pipe pipe;

            public ConsoleColor backgroundColor;
            public ConsoleColor textColor;
            public bool MainLoop;
        }
        public struct Movement
        {
            public string directionEnter;
            public string directionExit;
        }
        public struct Pipe
        {
            public bool isPipe;
            public char symbol;
            public Dictionary<string, Movement> Movements;
        }

        #endregion


        #region BoilerPlate

        private readonly string[] _input;

        public Day10()
        {
            _input = File.ReadAllLines(InputFilePath);
            ParseInput(_input);
        }

        public override ValueTask<string> Solve_1() => new(Part1());

        public override ValueTask<string> Solve_2() => new(Part2());

        #endregion

        #region Logging

        public void LogStatus(List<List<MapPoint>> Maze)
        {
            foreach(var row in Maze)
            {
                foreach(var col in row)
                {
                    Console.BackgroundColor = col.backgroundColor;
                    Console.ForegroundColor = col.textColor;
                    Console.Write(col.pipe.symbol);
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
            }
        }

        #endregion

        #region Common

        public Dictionary<char, Dictionary<string, Movement>> PipeMovements = new Dictionary<char, Dictionary<string, Movement>>
        {
            { '-', new Dictionary<string, Movement>(){ {"east", new Movement() { directionEnter = "east", directionExit = "west"} },{ "west", new Movement() { directionEnter = "west", directionExit = "east" } } } },
            { '|', new Dictionary<string, Movement>(){ {"north", new Movement() { directionEnter = "north", directionExit = "south"} },{ "south", new Movement() { directionEnter = "south", directionExit = "north" } } } },
            { 'L', new Dictionary<string, Movement>(){ {"east", new Movement() { directionEnter = "east", directionExit = "north"} },{ "north", new Movement() { directionEnter = "north", directionExit = "east" } } } },
            { 'J', new Dictionary<string, Movement>(){ {"north", new Movement() { directionEnter = "north", directionExit = "west"} },{ "west", new Movement() { directionEnter = "west", directionExit = "north" } } } },
            { '7', new Dictionary<string, Movement>(){ {"south", new Movement() { directionEnter = "south", directionExit = "west"} },{ "west", new Movement() { directionEnter = "west", directionExit = "south" } } } },
            { 'F', new Dictionary<string, Movement>(){ {"east", new Movement() { directionEnter = "east", directionExit = "south"} },{ "south", new Movement() { directionEnter = "south", directionExit = "east" } } } },
        };

        private void ParseInput(string[] input)
        {
            Maze = new List<List<MapPoint>>();
            for(int Y = 0; Y < input.Length; Y++)
            {
                List<MapPoint> row = new List<MapPoint>();

                for(int X = 0; X < input[0].Length; X++)
                {
                    MapPoint newMapPoint = new MapPoint();
                    newMapPoint.backgroundColor = ConsoleColor.Black;
                    newMapPoint.textColor = ConsoleColor.White;
                    newMapPoint.MainLoop = false;

                    char character = input[Y][X];
                    if (character == '.')
                    {
                        newMapPoint.pipe = new Pipe()
                        {
                            isPipe = false,
                            symbol = input[Y][X],
                            Movements = new Dictionary<string, Movement>()
                        };

                    }
                    else
                    {
                        if (character == 'S')
                        {
                            startX = X;
                            startY = Y;

                            character = FindStartPipe(X, Y, input);

                        }
                        
                        newMapPoint.pipe = new Pipe()
                        {
                            isPipe = true,
                            symbol = character,
                            Movements = PipeMovements[character]
                        };

                    }
                    row.Add(newMapPoint);
                }
                Maze.Add(row);
            }

            CurrentX = startX;
            CurrentY = startY;
        }

        public char FindStartPipe(int X, int Y, string[] input)
        {
            string directionBits = "";
            if (Y>0)
            {
                char symbol = input[Y - 1][X];
                if (symbol == '|' || symbol == '7' || symbol == 'F')
                {
                    directionBits += "1";
                }
                else { directionBits += "0"; }

            }
            else { directionBits += "0"; }
            if (Y < input.Count())
            {
                char symbol = input[Y + 1][X];
                if (symbol == '|' || symbol == 'L' || symbol == 'J')
                {
                    directionBits += "1";
                }
                else { directionBits += "0"; }
            }
            else { directionBits += "0"; }
            if (X > 0 )
            {
                char symbol = input[Y][X - 1];
                if (symbol == '-' || symbol == 'L' || symbol == 'F')
                {
                    directionBits += "1";
                }
                else { directionBits += "0"; }
            }
            else { directionBits += "0"; }
            if (X < input[0].Count())
            {
                char symbol = input[Y][X + 1];
                if (symbol == '-' || symbol == 'J' || symbol == '7')
                {
                    directionBits += "1";
                }
                else { directionBits += "0"; }
            }
            else { directionBits += "0"; }

            switch(directionBits)
            {
                case "1100":
                    return '|';
                case "1010":
                    return 'J';
                case "1001":
                    return 'L';
                case "0110":
                    return '7';
                case "0101":
                    return 'F';
                case "0011":
                    return '-';
            }
            Console.WriteLine(directionBits);
            return 'S';

        }
        #endregion

        #region Part 1

        private string Part1()
        {
            var pipeSection = Maze[startY][startX];
            pipeSection.backgroundColor = ConsoleColor.Blue;
            pipeSection.textColor = ConsoleColor.White;
            pipeSection.MainLoop = true;
            Maze[CurrentY][CurrentX] = pipeSection;

            string CurrentDirection = Move(Maze[CurrentY][CurrentX].pipe.Movements.Values.First());

            pipeSection = Maze[CurrentY][CurrentX];
            pipeSection.backgroundColor = ConsoleColor.Blue;
            pipeSection.textColor = ConsoleColor.White;
            pipeSection.MainLoop = true;
            Maze[CurrentY][CurrentX] = pipeSection;

            long Length = 1;
            while (!(startX == CurrentX && startY == CurrentY))
            {

                pipeSection = Maze[CurrentY][CurrentX];
                pipeSection.backgroundColor = ConsoleColor.Blue;
                pipeSection.textColor = ConsoleColor.White;
                pipeSection.MainLoop = true;
                Maze[CurrentY][CurrentX] = pipeSection;
                CurrentDirection = Move(Maze[CurrentY][CurrentX].pipe.Movements[CurrentDirection]);
                Length++;
            }
            long FarthestDistance = Length / 2;
            return FarthestDistance.ToString();
        }

        private string Move(Movement movement)
        {
            string newDirection = "";
            switch (movement.directionExit)
            {
                case "north":
                    newDirection = "south";
                    CurrentY -= 1;
                    break;
                case "east":
                    newDirection = "west";
                    CurrentX += 1;
                    break;
                case "west":
                    newDirection = "east";
                    CurrentX -= 1;
                    break;
                case "south":
                    newDirection = "north";
                    CurrentY += 1;
                    break;
            }
            return newDirection;
        }


        #endregion

        #region Part 2
        private string Part2()
        {
            long Sum = 0;

            for(int row = 0; row < Maze.Count(); row ++)
            {
                for(int col = 0; col < Maze[0].Count(); col ++)
                {
                    var pipeSection = Maze[row][col];
                    if(!pipeSection.MainLoop)
                    {
                        pipeSection.pipe.symbol = '.';
                    }
                    Maze[row][col] = pipeSection;
                }
            }

            foreach(var line in Maze)
            {
                Sum += GetInteriorSpaces(line);
            }

            //LogStatus(Maze);

            return Sum.ToString();
        }

        private long GetInteriorSpaces(List<MapPoint> line)
        {
            long interiorSpaces = 0;
            bool isInside = false;
            char LastCorner = ' ';

            for(int i = 0; i < line.Count(); i++ )
            {
                MapPoint point = line[i];

                char symbol = point.pipe.symbol;
                if(symbol == '.')
                {
                    if(isInside)
                    {
                        interiorSpaces++;
                        point.backgroundColor = ConsoleColor.Green;
                        line[i] = point;
                    }
                }
                else
                {
                    switch(symbol)
                    {
                        case '|':
                            isInside = !isInside;
                            break;
                        case '-':
                            break;
                        case 'L':
                            LastCorner = 'L';
                            break;
                        case 'J':
                            if (LastCorner != 'L')
                            {
                                isInside = !isInside;
                            }
                            LastCorner = 'J';
                            break;
                        case 'F':
                            LastCorner = 'F';
                            break;
                        case '7':
                            if (LastCorner != 'F')
                            {
                                isInside = !isInside;
                            }
                            LastCorner = '7';
                            break;
                    }
                }
            }
            return interiorSpaces;
        }

        #endregion

        #region Utilities

        #endregion
    }
}
