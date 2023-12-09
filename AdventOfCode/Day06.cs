using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Day06 : BaseDay
    {
        private List<RaceDetails> raceDetails;
        private RaceDetails MegaRace;

        #region Helper Structs


        #endregion

        public struct RaceDetails
        {
            public long TimeToBeat;
            public long RaceLength;
        }

        #region BoilerPlate

        private readonly string[] _input;

        public Day06()
        {
            _input = File.ReadAllLines(InputFilePath);
            ParseInput(_input);
        }

        public override ValueTask<string> Solve_1() => new(Part1());

        public override ValueTask<string> Solve_2() => new(Part2());

        #endregion

        #region Logging

        #endregion

        #region Common


        private void ParseInput(string[] input)
        {
            raceDetails = new List<RaceDetails>();
            List<int> racelengths = input[0].Split(":")[1].Trim().Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(x => int.Parse(x)).ToList();
            List<long> raceRecords = input[1].Split(":")[1].Trim().Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(x => long.Parse(x)).ToList(); ;
            for(int i = 0; i < racelengths.Count; i++)
            {
                RaceDetails race = new RaceDetails()
                {
                    TimeToBeat = raceRecords[i],
                    RaceLength = racelengths[i]
                };

                raceDetails.Add(race);
            }
            MegaRace = new RaceDetails()
            {
                TimeToBeat = long.Parse(raceRecords.Select(x => x.ToString()).Aggregate("", (current, s) => current + s)),
                RaceLength = long.Parse(racelengths.Select(x => x.ToString()).Aggregate("", (current, s) => current + s))
            };

        }


        public int GetPossibleWins(RaceDetails race)
        {
            double a = -1;
            double b = race.RaceLength;
            double c = -1 * race.TimeToBeat;
            double FirstRoot = (-b + Math.Sqrt(Math.Pow(b, 2) - (4 * a * c))) / (2 * a);
            double SecondRoot = (-b - Math.Sqrt(Math.Pow(b, 2) - (4 * a * c))) / (2 * a); ;

            Console.WriteLine("Root 1: {0, 5} Root 2: {1,5}", FirstRoot, SecondRoot);
            long RangeStart = (long)FirstRoot + 1;
            long RangeEnd = (long)SecondRoot;
            if (RangeEnd == SecondRoot)
            {
                RangeEnd--;
            }
            Console.WriteLine("Root 1: {0, 5} Root 2: {1,5}", RangeStart, RangeEnd);
            long possibleWins = RangeEnd - RangeStart + 1;
            Console.WriteLine(possibleWins.ToString());
            Console.WriteLine();

            return (int)possibleWins;
        }

        #endregion

        #region Part 1

        private string Part1()
        {
            long sum = 1;
            foreach(var race in raceDetails)
            {
                sum *= GetPossibleWins(race);
            }
            return sum.ToString();
        }


        #endregion

        #region Part 2
        private string Part2()
        {
            long sum = GetPossibleWins(MegaRace);
            return sum.ToString();
        }

        #endregion

        #region Utilities

        #endregion
    }
}
