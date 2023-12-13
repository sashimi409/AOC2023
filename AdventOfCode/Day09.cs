using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Day09 : BaseDay
    {

        public List<Sequence> Readings;

        #region Helper Structs

        public struct Sequence
        {
            public List<List<long>> derivativeLists;
        }

        #endregion


        #region BoilerPlate

        private readonly string[] _input;

        public Day09()
        {
            _input = File.ReadAllLines(InputFilePath);
            ParseInput(_input);
        }

        public override ValueTask<string> Solve_1() => new(Part1());

        public override ValueTask<string> Solve_2() => new(Part2());

        #endregion

        #region Logging

        public void LogSequence(Sequence sequence)
        {
            foreach(var layer in sequence.derivativeLists)
            {
                foreach(var item in layer)
                {
                    Console.Write("{0} ", item);
                }
                Console.WriteLine();
            }
        }


        #endregion

        #region Common


        private void ParseInput(string[] input)
        {
            Readings = new List<Sequence>();
            foreach (string line in input)
            {
                Sequence newSequence = new Sequence();
                newSequence.derivativeLists = new List<List<long>>();

                List<long> numbers = line.Split(' ').Select(x => long.Parse(x)).ToList();

                newSequence.derivativeLists.Add(numbers);
                bool ConstantArrayFound = false;

                while (!ConstantArrayFound)
                {
                    List<long> nextLayer = new List<long>();
                    for (int i = 0; i < newSequence.derivativeLists.Last().Count -1; i++ )
                    {
                        long change = newSequence.derivativeLists.Last()[i + 1] - newSequence.derivativeLists.Last()[i];
                        nextLayer.Add(change);
                    }
                    newSequence.derivativeLists.Add(nextLayer);
                    ConstantArrayFound = CheckForDone(newSequence);
                }
                Readings.Add(newSequence);
            }
        }

        private bool CheckForDone(Sequence newSequence)
        {
            List<long> toCheck = newSequence.derivativeLists.Last();
            long first = toCheck[0];
            foreach(long number in toCheck)
            {
                if (number != first)
                {
                    return false;
                }
            }
            return true;
        }



        #endregion

        #region Part 1

        private string Part1()
        {
            long Sum = 0;
            foreach (Sequence reading in Readings)
            {
                Sum += NextNumber(reading);
            }

            return Sum.ToString();
        }

        private long NextNumber(Sequence reading)
        {
            long nextNumber = 0;
            foreach(List<long> list in reading.derivativeLists) 
            {
                nextNumber += list.Last();
            }
            return nextNumber;
        }


        #endregion

        #region Part 2
        private string Part2()
        {
            long Sum = 0;
            foreach (Sequence reading in Readings)
            {
                Sum += PreviousNumber(reading);
            }

            return Sum.ToString();
        }

        private long PreviousNumber(Sequence reading)
        {
            long difference = 0;
            for(int i = 1; i < reading.derivativeLists.Count; i++)
            {
                if(i%2==1)
                {
                    difference += reading.derivativeLists[i][0];
                }
                else { difference -= reading.derivativeLists[i][0]; }

            }
            return reading.derivativeLists[0][0] - difference;
        }

        #endregion

        #region Utilities

        #endregion
    }
}
