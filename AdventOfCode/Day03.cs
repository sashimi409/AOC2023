using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Data;
using System.Drawing;
using static AdventOfCode.Day03;

namespace AdventOfCode
{
    internal class Day03 : BaseDay
    {

        #region Helper Structs
        public struct Schematic
        {
            public int Width;
            public int Height;
            public List<List<DisplayCoordinate>> Grid;
        }

        public struct PartNumber
        {
            public int row;
            public int length;
            public int startIndex;
            public int value;
        }

        public struct Part
        {
            public PartNumber partNumber;
            public HashSet<char> symbolsNearby;
        }

        public struct Coordinate
        {
            public int row;
            public int col;
        }

        public struct Gear
        {
            public Coordinate coordinate;
            public List<int> adajacentNumbers;
        }

        public struct DisplayCoordinate
        {
            public char value;
            public ConsoleColor backgroundColor;
            public ConsoleColor textColor;
        }
        #endregion

        #region BoilerPlate

        private readonly string[] _input;

        public Day03()
        {
             _input = File.ReadAllLines(InputFilePath);
        }

        public override ValueTask<string> Solve_1() => new(Part1());

        public override ValueTask<string> Solve_2() => new(Part2());

        #endregion

        #region Logging
        private void LogSchematic(Schematic inputSchematic)
        {
            foreach(var row in inputSchematic.Grid)
            {
                foreach(var col in row)
                {
                    Console.Write(col.value.ToString());
                }
                Console.WriteLine();

            }
        }

        private void LogPartNumbers(List<PartNumber> partNumbers )
        {
            foreach(var partNumber in partNumbers)
            {
                Console.WriteLine(partNumber.value.ToString());
            }
        }

        private void LogParts(List<Part> parts)
        {
            foreach(var part in parts)
            {
                Console.WriteLine(part.partNumber.value.ToString());
                foreach(var symbol in part.symbolsNearby)
                {
                    Console.Write(symbol);
                }
                Console.WriteLine();
            }
        }

        private void LogGear(Gear gear)
        {
            Console.Write("{0, 3},{1, 3}| {2} |", gear.coordinate.row, gear.coordinate.col, gear.adajacentNumbers.Count);
            foreach(int i in gear.adajacentNumbers)
            {
                Console.Write(i.ToString() + ",");
            }
            Console.WriteLine();
        }

        private void LogFinalGearView(List<Gear> gears, Schematic schematic) 
        {
            foreach (var row in schematic.Grid)
            {
                foreach (var col in row)
                {
                    Console.BackgroundColor = col.backgroundColor; 
                    Console.ForegroundColor = col.textColor;
                    Console.Write(col.value);
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();

            }
        }
        #endregion

        #region Common

        private Schematic ParseInput(string[] input)
        {
            Schematic InputSchematic = new Schematic();

            InputSchematic.Width = _input[0].Length;
            InputSchematic.Height = _input.Length;
            InputSchematic.Grid = new List<List<DisplayCoordinate>>();
            foreach (var line in input)
            {
                List<DisplayCoordinate> row = new List<DisplayCoordinate>();
                foreach (var col in line)
                {
                    DisplayCoordinate GridValue = new DisplayCoordinate();
                    GridValue.backgroundColor = ConsoleColor.Black;
                    GridValue.textColor = ConsoleColor.White;
                    GridValue.value = col;
                    row.Add(GridValue);
                   
                }
                InputSchematic.Grid.Add(row);
            }

            //LogSchematic(InputSchematic);
            return InputSchematic;
        }

        #endregion

        #region Part 1

        private string Part1()
        {
            HashSet<char> ValidSymbols = new HashSet<char> { '*', '#', '+', '$', '=', '-', '@', '!', '&', '^', '%', '/' };

            Schematic schematic = ParseInput(_input);
            List<PartNumber> partNumbers = ParseSchematic(schematic);
            List<Part> parts = ProcessPartNumbers(partNumbers, schematic);
            int sum = SumRealParts(parts, ValidSymbols);

            return sum.ToString();
        }

        private List<PartNumber> ParseSchematic(Schematic schematic)
        {
            List<PartNumber> returnPartNumbers = new List<PartNumber>();

            for(int row = 0; row < schematic.Height; row++)
            {
                for(int col = 0; col < schematic.Width; col++)
                {
                    if (Char.IsDigit(schematic.Grid[row][col].value))
                    {
                        PartNumber foundPartNumber = new PartNumber();
                        foundPartNumber.row = row;
                        foundPartNumber.startIndex = col;
                        foundPartNumber.length = 1;
                        foundPartNumber.value = schematic.Grid[row][col].value - '0';

                       
                        for(int next = col+1; next < schematic.Width; next++)
                        {
                            char nextChar = schematic.Grid[row][next].value;
                            if (Char.IsDigit(nextChar))
                            {
                                int nextDigit = nextChar - '0';
                                foundPartNumber.value = JoinNumber(foundPartNumber.value, nextDigit);
                                foundPartNumber.length++;
                            }
                            else
                            {
                                break;
                            }
                            col = next;
                        }

                        //Console.WriteLine(foundPartNumber.value.ToString());
                        returnPartNumbers.Add(foundPartNumber);
                        //Console.WriteLine(returnPartNumbers.Count.ToString());
                    }
                    else { continue; }
                    if(col > schematic.Width)
                    {
                        break;
                    }
                }
            }
            //LogPartNumbers(returnPartNumbers);
            return returnPartNumbers;
        }

        private List<Part> ProcessPartNumbers(List<PartNumber> partNumbers, Schematic schematic)
        {
            List<Part> returnParts = new List<Part>();

            foreach (PartNumber partNumber in partNumbers)
            {
                Part part = new Part();
                part.partNumber = partNumber;
                part.symbolsNearby = new HashSet<char>();

                int StartSearchCol = Math.Clamp((partNumber.startIndex - 1), 0, schematic.Width);
                int StartSearchRow = Math.Clamp((partNumber.row - 1), 0, schematic.Height);
                int EndSearchCol = partNumber.startIndex + partNumber.length;
                int EndSearchRow = partNumber.row + 1;


                for (int row = StartSearchRow; row < schematic.Height && row <= EndSearchRow; row++)
                {
                    for (int col = StartSearchCol; col < schematic.Width && col <= EndSearchCol; col++)
                    {
                        part.symbolsNearby.Add(schematic.Grid[row][col].value);
                    }
                }
                returnParts.Add(part);
            }
            
            //LogParts(returnParts);
            return returnParts;
        }

        private int SumRealParts(List<Part> parts, HashSet<char> validSymbols)
        {
            int sum = 0;

            foreach(var part in parts)
            {
                if(true)
                {
                    if(part.symbolsNearby.Overlaps(validSymbols))
                    {
                        sum += part.partNumber.value;
                    }
                }

            }

            return sum;
        }

        #endregion

        #region Part 2
        private string Part2()
        {
            Schematic schematic = ParseInput(_input);
            List<Coordinate> gearLocations = FindGears(schematic);
            List<Gear> gears = ParseGears(gearLocations, schematic);
            int sum = SumGearWithRatios(gears, schematic);
            LogFinalGearView(gears, schematic);

            return sum.ToString();
        }


        private List<Coordinate> FindGears(Schematic schematic)
        {
            List<Coordinate> Coordinates = new List<Coordinate>();

            for (int row = 0; row < schematic.Height; row++)
            {
                for (int col = 0; col < schematic.Width; col++)
                {
                    if (schematic.Grid[row][col].value == '*')
                    {
                        var gearSymbol = schematic.Grid[row][col];
                        gearSymbol.backgroundColor = ConsoleColor.Gray;
                        gearSymbol.textColor = ConsoleColor.Black;

                        schematic.Grid[row][col] = gearSymbol;

                        Coordinate foundGear = new Coordinate()
                        {
                            row = row,
                            col = col,
                        };

                        Coordinates.Add(foundGear);
                        //Console.WriteLine("{0} , {1}", foundGear.row, foundGear.col);
                    }
                }
            }

            return Coordinates;
        }
        private List<Gear> ParseGears(List<Coordinate> gearLocations, Schematic schematic)
        {
            List<Gear> gears = new List<Gear>();

            foreach (Coordinate gearLocation in gearLocations)
            {
                Gear parsedGear = new Gear() {
                    adajacentNumbers = new List<int>(),
                    coordinate = gearLocation
                };

                //Check Left
                if (gearLocation.col > 0)
                {
                    if (Char.IsDigit(schematic.Grid[gearLocation.row][gearLocation.col - 1].value))
                    {
                        var gearSymbol = schematic.Grid[gearLocation.row][gearLocation.col - 1];
                        gearSymbol.backgroundColor = ConsoleColor.DarkBlue;
                        gearSymbol.textColor = ConsoleColor.Black;
                        schematic.Grid[gearLocation.row][gearLocation.col - 1] = gearSymbol;

                        int tempValue = schematic.Grid[gearLocation.row][gearLocation.col - 1].value - '0';

                        for (int i = (gearLocation.col - 2); i >= 0; i--)
                        {
                            if (Char.IsDigit(schematic.Grid[gearLocation.row][i].value))
                            {
                                gearSymbol = schematic.Grid[gearLocation.row][i];
                                gearSymbol.backgroundColor = ConsoleColor.DarkBlue;
                                gearSymbol.textColor = ConsoleColor.Black;
                                schematic.Grid[gearLocation.row][i] = gearSymbol;

                                int preceedingDigit = schematic.Grid[gearLocation.row][i].value - '0';
                                tempValue = JoinNumber(preceedingDigit, tempValue);
                            }
                            else { break; }
                        }
                        parsedGear.adajacentNumbers.Add(tempValue);
                    }
                }
                // Check Right
                if (gearLocation.col < schematic.Width - 1)
                {
                    if (Char.IsDigit(schematic.Grid[gearLocation.row][gearLocation.col + 1].value))
                    {
                        var gearSymbol = schematic.Grid[gearLocation.row][gearLocation.col + 1];
                        gearSymbol.backgroundColor = ConsoleColor.DarkBlue;
                        gearSymbol.textColor = ConsoleColor.Black;
                        schematic.Grid[gearLocation.row][gearLocation.col + 1] = gearSymbol;

                        int tempValue = schematic.Grid[gearLocation.row][gearLocation.col + 1].value - '0';

                        for (int i = (gearLocation.col + 2); i < schematic.Width; i++)
                        {
                            if (Char.IsDigit(schematic.Grid[gearLocation.row][i].value))
                            {
                                gearSymbol = schematic.Grid[gearLocation.row][i];
                                gearSymbol.backgroundColor = ConsoleColor.DarkBlue;
                                gearSymbol.textColor = ConsoleColor.Black;
                                schematic.Grid[gearLocation.row][i] = gearSymbol;

                                int nextDigit = schematic.Grid[gearLocation.row][i].value - '0';
                                tempValue = JoinNumber(tempValue, nextDigit);
                            }
                            else { break; }
                        }
                        parsedGear.adajacentNumbers.Add(tempValue);
                    }
                }
                //Check Top
                if (gearLocation.row > 0)
                {
                    int rowToCheck = gearLocation.row - 1;

                    CheckAdjacentRow(schematic, gearLocation, parsedGear, rowToCheck);
                }
                //Check Bottom
                if (gearLocation.row < schematic.Height - 1)
                {
                    int rowToCheck = gearLocation.row + 1;
                    CheckAdjacentRow(schematic, gearLocation, parsedGear, rowToCheck);
                }
                gears.Add(parsedGear);
                //LogGear(parsedGear);
            }
            return gears;
        }

        void CheckAdjacentRow(Schematic schematic, Coordinate gearLocation, Gear parsedGear, int rowToCheck)
        {
            if (Char.IsDigit(schematic.Grid[rowToCheck][gearLocation.col].value))
            {
                int startCol = gearLocation.col;
                for (int i = (gearLocation.col - 1); i >= 0; i--)
                {
                    if (Char.IsDigit(schematic.Grid[rowToCheck][i].value))
                    {
                        startCol = i;
                    }
                    else { break; }
                }

                var gearSymbol = schematic.Grid[rowToCheck][startCol];
                gearSymbol.backgroundColor = ConsoleColor.DarkBlue;
                gearSymbol.textColor = ConsoleColor.Black;
                schematic.Grid[rowToCheck][startCol] = gearSymbol;

                int tempValue = schematic.Grid[rowToCheck][startCol].value - '0';

                for (int i = startCol + 1; i < schematic.Width; i++)
                {
                    if (Char.IsDigit(schematic.Grid[rowToCheck][i].value))
                    {
                        gearSymbol = schematic.Grid[rowToCheck][i];
                        gearSymbol.backgroundColor = ConsoleColor.DarkBlue;
                        gearSymbol.textColor = ConsoleColor.Black;
                        schematic.Grid[rowToCheck][i] = gearSymbol;

                        int nextDigit = schematic.Grid[rowToCheck][i].value - '0';
                        tempValue = JoinNumber(tempValue, nextDigit);
                    }
                    else { break; }
                }
                parsedGear.adajacentNumbers.Add(tempValue);
            }
            else
            {
                //Left corner
                if (gearLocation.col > 0)
                {
                    if (Char.IsDigit(schematic.Grid[rowToCheck][gearLocation.col-1].value))
                    {
                        int startCol = gearLocation.col-1;
                        for (int i = (gearLocation.col - 1); i >= 0; i--)
                        {
                            if (Char.IsDigit(schematic.Grid[rowToCheck][i].value))
                            {
                                startCol = i;
                            }
                            else { break; }
                        }

                        var gearSymbol = schematic.Grid[rowToCheck][startCol];
                        gearSymbol.backgroundColor = ConsoleColor.DarkBlue;
                        gearSymbol.textColor = ConsoleColor.Black;
                        schematic.Grid[rowToCheck][startCol] = gearSymbol;

                        int tempValue = schematic.Grid[rowToCheck][startCol].value - '0';

                        for (int i = startCol + 1; i < schematic.Width; i++)
                        {
                            if (Char.IsDigit(schematic.Grid[rowToCheck][i].value))
                            {
                                gearSymbol = schematic.Grid[rowToCheck][i];
                                gearSymbol.backgroundColor = ConsoleColor.DarkBlue;
                                gearSymbol.textColor = ConsoleColor.Black;
                                schematic.Grid[rowToCheck][i] = gearSymbol;

                                int nextDigit = schematic.Grid[rowToCheck][i].value - '0';
                                tempValue = JoinNumber(tempValue, nextDigit);
                            }
                            else { break; }
                        }
                        parsedGear.adajacentNumbers.Add(tempValue);
                    }
                }
                //Right corner
                if (gearLocation.col < schematic.Width - 1)
                {
                    if (Char.IsDigit(schematic.Grid[rowToCheck][gearLocation.col + 1].value))
                    {
                        int startCol = gearLocation.col + 1;

                        var gearSymbol = schematic.Grid[rowToCheck][startCol];
                        gearSymbol.backgroundColor = ConsoleColor.DarkBlue;
                        gearSymbol.textColor = ConsoleColor.Black;
                        schematic.Grid[rowToCheck][startCol] = gearSymbol;

                        int tempValue = schematic.Grid[rowToCheck][startCol].value - '0';

                        for (int i = startCol + 1; i < schematic.Width; i++)
                        {
                            if (Char.IsDigit(schematic.Grid[rowToCheck][i].value))
                            {
                                gearSymbol = schematic.Grid[rowToCheck][i];
                                gearSymbol.backgroundColor = ConsoleColor.DarkBlue;
                                gearSymbol.textColor = ConsoleColor.Black;
                                schematic.Grid[rowToCheck][i] = gearSymbol;

                                int nextDigit = schematic.Grid[rowToCheck][i].value - '0';
                                tempValue = JoinNumber(tempValue, nextDigit);
                            }
                            else { break; }
                        }
                        parsedGear.adajacentNumbers.Add(tempValue);
                    }
                }
            }
        }
        private int SumGearWithRatios(List<Gear> gears, Schematic schematic)
        {
            int sum = 0;
            foreach(var gear in gears)
            {
                if(gear.adajacentNumbers.Count == 2)
                {
                    var gearSymbol = schematic.Grid[gear.coordinate.row][gear.coordinate.col];
                    gearSymbol.backgroundColor = ConsoleColor.Blue;
                    gearSymbol.textColor = ConsoleColor.Black;

                    schematic.Grid[gear.coordinate.row][gear.coordinate.col] = gearSymbol;

                    int Ratio = (gear.adajacentNumbers[0] * gear.adajacentNumbers[1]);
                    Console.WriteLine(Ratio.ToString());
                    sum += Ratio;
                }
            }
            return sum;
        }

        #endregion

        #region Utilities
        public int JoinNumber(int x, int y)
        {
            int z = 0;
            string temp = Convert.ToString(x) + Convert.ToString(y);
            z = Convert.ToInt32(temp);
            return z;
        }

        #endregion
    }
}
