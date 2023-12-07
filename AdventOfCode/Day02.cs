using Spectre.Console.Rendering;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using static AdventOfCode.Day02;

namespace AdventOfCode;

public class Day02 : BaseDay
{
    private readonly string[] _input;

    public struct cubes
    {
        public int Red;
        public int Green;
        public int Blue;
    }
    public Day02()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new(Part1());

    public override ValueTask<string> Solve_2() => new(Part2());

    public string Part1()
    {
        int sum = 0;

        cubes Scenario = new cubes()
        {
            Red = 12,
            Green = 13,
            Blue = 14
        };

        foreach( string line in _input)
        {
            cubes SeenCubes = GetSeenCubes(line);

            string[] StringParts = line.Split(':');
            Regex rx = new Regex(@"Game (?<GameNumber>\d*)",
              RegexOptions.Compiled | RegexOptions.IgnoreCase); ;

            int GameNumber = int.Parse(rx.Match(StringParts[0]).Groups["GameNumber"].Value);


            if (IsPossible(Scenario, SeenCubes))
            {
                sum += GameNumber;
            }
        }


        return sum.ToString();
    }


    public string Part2()
    {
        int sum = 0;

        foreach (string line in _input)
        {
            cubes SeenCubes = GetSeenCubes(line);

            sum += (SeenCubes.Red*SeenCubes.Green*SeenCubes.Blue);
        }

        return sum.ToString();
    }

    bool IsPossible(cubes Scenario, cubes SeenCubes)
    {
        
        return (Scenario.Red >= SeenCubes.Red && Scenario.Blue >= SeenCubes.Blue && Scenario.Green >= SeenCubes.Green);
    }

    cubes GetSeenCubes(string line)
    {
        cubes SeenCubes = new cubes()
        {
            Red = 0,
            Green = 0,
            Blue = 0
        };

        string[] StringParts = line.Split(':');

        string[] Games = StringParts[1].Split(";");
        foreach (string Game in Games)
        {
            string[] cubes = Game.Split(",");
            foreach (string c in cubes)
            {
                string[] subStrings = c.Split(" ");
                string Type = subStrings[2];
                int Number = int.Parse(subStrings[1]);

                switch (Type)
                {
                    case "red":
                        if (Number > SeenCubes.Red)
                        {
                            SeenCubes.Red = Number;
                        };
                        break;
                    case "blue":
                        if (Number > SeenCubes.Blue)
                        {
                            SeenCubes.Blue = Number;
                        };
                        break;
                    case "green":
                        if (Number > SeenCubes.Green)
                        {
                            SeenCubes.Green = Number;
                        };
                        break;
                }
            }
        }

        return SeenCubes;
    }

} 