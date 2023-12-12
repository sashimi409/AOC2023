using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static AdventOfCode.Day05;

namespace AdventOfCode
{
    internal class Day08 : BaseDay
    {
        public string directions;

        public Dictionary<string, string> Left;
        public Dictionary<string, string> Right;

        List<string> StartingNodes;

        #region Helper Structs


        #endregion


        #region BoilerPlate

        private readonly string[] _input;

        public Day08()
        {
            _input = File.ReadAllLines(InputFilePath);
            ParseInput(_input);
        }

        public override ValueTask<string> Solve_1() => new(Part1());

        public override ValueTask<string> Solve_2() => new(Part2());

        #endregion

        #region Logging

        private void LogState(long stepCount, List<string> currentNodes)
        {
            Console.WriteLine("Current Step: {0}", stepCount);
            foreach (var node in currentNodes)
            {
                Console.WriteLine(node);
                
            }
            Console.WriteLine();
        }


        #endregion

        #region Common


        private void ParseInput(string[] input)
        {
            directions = input[0];
            List<string> nodes = input.Skip(2).ToList();
            Left = new Dictionary<string, string>();
            Right = new Dictionary<string, string>();
            StartingNodes = new List<string>();

            Regex rx = new Regex(@"\w{3}",
            RegexOptions.Compiled | RegexOptions.IgnoreCase); ;

            foreach (string line in nodes)
            {
                MatchCollection Matches = rx.Matches(line);
                string baseNode = Matches[0].Value;
                string left = Matches[1].Value;
                string right = Matches[2].Value;

                Left.Add(baseNode, left);
                Right.Add(baseNode, right);


                //Console.WriteLine("{0} left: {1} right {2}", baseNode, left, right);

                Regex start = new Regex(@"..A",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);


                if (start.Match(baseNode).Success)
                {
                    StartingNodes.Add(baseNode);
                }
            }

        }

        

        #endregion

        #region Part 1

        private string Part1()
        {
            long stepCount = 0;
            bool found = false;
            string currentNode = "AAA" ;
            int directionIndex = 0;
            while(!found)
            {
                stepCount++;
                char movement = directions[directionIndex%directions.Length];
                if(movement == 'R')
                {
                    currentNode = Right[currentNode];
                }
                else 
                {
                    currentNode = Left[currentNode];
                }
                if(currentNode == "ZZZ")
                {
                    found = true;
                }
                directionIndex++;
            }

            return stepCount.ToString();
        }


        #endregion

        #region Part 2
        private string Part2()
        {
            List<int> stepCounts = new List<int>();
            Regex rx = new Regex(@"..Z",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

            int directionIndex = 0;
            for(int i = 0; i < StartingNodes.Count; i++)
            {
                string startNode = StartingNodes[i];
                bool found = false;
                int stepCount = 0;

                while (!found)
                {
                    stepCount++;
                    char movement = directions[directionIndex % directions.Length];
                    if (movement == 'R')
                    {
                        startNode = Right[startNode];
                    }
                    else
                    {
                        startNode = Left[startNode];
                    }
                    directionIndex++;
                    if(rx.Match(startNode).Success)
                    {
                        stepCounts.Add(stepCount);
                        break;
                    }
                }

            }
            long LCM = 1;

            foreach(long i in stepCounts)
            {
                LCM = FindLCM(LCM, i);
            }


            return LCM.ToString();
        }

        public long FindLCM(long first, long second)
        {
            long returnValue = 1;

            long  max = (first > second) ? first : second;
            for (long  i = max; ; i += max)
            {
                if (i % first == 0 && i % second == 0)
                {
                    returnValue = i;
                    break;
                }
            }
            return returnValue;
        }
        public bool AllOnZ(List<string> nodes)
        {
            Regex rx = new Regex(@"..Z",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

            foreach ( string node in nodes )
            {
                if(!rx.Match(node).Success)
                {
                    return false;
                }
            }
            return true;

        }
        #endregion

        #region Utilities

        #endregion
    }
}
