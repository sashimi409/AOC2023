using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AdventOfCode
{
    internal class Day04 : BaseDay
    {

        #region Helper Structs

        internal struct ScratchCard
        {
            HashSet<int> WinningNumbers;
            HashSet<int> RevealedNumbers;
            int NumbersMatched;
        }

        #endregion

        #region BoilerPlate

        private readonly string[] _input;

        public Day04()
        {
            _input = File.ReadAllLines(InputFilePath);
        }

        public override ValueTask<string> Solve_1() => new(Part1());

        public override ValueTask<string> Solve_2() => new(Part2());

        #endregion

        #region Logging

        #endregion

        #region Common

        public List<ScratchCard> ParseInput(string[] input)
        {
            List<ScratchCard> result = new List<ScratchCard>();

            foreach (var line in input)
            {
                ScratchCard scratchCard = new ScratchCard();

                string numbers = line.Split(':')[1];
                string[] winningNumbers = numbers.Split("|")[0].Trim().Split(" ");
                string[] revealedNumbers = numbers.Split("|")[1].Trim().Split(" ");

                Console.WriteLine("{0} | {1}",winningNumbers, revealedNumbers);
            }

            return result;
        }

        #endregion

        #region Part 1

        private string Part1()
        {
            int sum = 0;

            List<ScratchCard> scratchCards = ParseInput(_input);
            sum = CalculatePoints(scratchCards);
            return sum.ToString();
        }

        private int CalculatePoints(List<ScratchCard> scratchCards)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Part 2
        private string Part2()
        {
            int sum = 0;

            return sum.ToString();
        }

        #endregion

        #region Utilities

        #endregion
    }
}