using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
            public int CardNumber;
            public HashSet<int> WinningNumbers;
            public HashSet<int> RevealedNumbers;
            public int NumbersMatched;

            public void UpdateMatches()
            {
                this.NumbersMatched = this.WinningNumbers.Intersect(this.RevealedNumbers).Count();
            }
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

        public void LogScractchCard(ScratchCard scratchCard)
        {
            Console.Write("Card {0}:", scratchCard.CardNumber);
            foreach (var win in scratchCard.WinningNumbers)
            {
                Console.Write(win + " ");
                
            }

            Console.Write("| ");

            foreach(var reveal in scratchCard.RevealedNumbers) 
            { 
                Console.Write(reveal + " ");
            }

            Console.Write("| Matched: " + scratchCard.NumbersMatched.ToString());


            Console.WriteLine();
        }

        public void LogScratchCards(List<ScratchCard> scratchCards)
        {
            for(int i = 0; i < scratchCards.Count; i++)
            {
                LogScractchCard(scratchCards[i]);
            }
        }

        public void LogTicketCounts(Dictionary<int, int> counts)
        {
            foreach(var count in counts)
            {
                Console.WriteLine("Card {0}: {1} copies",count.Key + 1, count.Value + 1);
            }
        }

        #endregion

        #region Common

        public List<ScratchCard> ParseInput(string[] input)
        {
            List<ScratchCard> result = new List<ScratchCard>();

            foreach (var line in input)
            {
                ScratchCard scratchCard = new ScratchCard();
                scratchCard.WinningNumbers = new HashSet<int>();
                scratchCard.RevealedNumbers = new HashSet<int>();

                string cardNumberString = line.Split(':')[0].Split(" ").Where(x => !string.IsNullOrEmpty(x)).ToArray()[1];
                scratchCard.CardNumber = int.Parse(cardNumberString);

                string numbers = line.Split(':')[1];
                string[] winningNumbers = numbers.Split("|")[0].Trim().Split(" ");
                scratchCard.WinningNumbers = winningNumbers.Where(x => !string.IsNullOrEmpty(x)).Select(x => int.Parse(x)).ToHashSet();

                string[] revealedNumbers = numbers.Split("|")[1].Trim().Split(" ");
                scratchCard.RevealedNumbers = revealedNumbers.Where(x => !string.IsNullOrEmpty(x)).Select(x => int.Parse(x)).ToHashSet();

                scratchCard.UpdateMatches();
                //LogScractchCard(scratchCard);

                result.Add(scratchCard);

            }

            return result;
        }

        #endregion

        #region Part 1

        private string Part1()
        {
            List<ScratchCard> scratchCards = ParseInput(_input);
            int sum = CalculatePoints(scratchCards);
            return sum.ToString();
        }

        private int CalculatePoints(List<ScratchCard> scratchCards)
        {
            int sum = 0;

            foreach (ScratchCard card in scratchCards)
            {
                if(card.NumbersMatched == 0)
                {
                    continue;
                }
                else
                {
                    sum += Convert.ToInt32(Math.Pow(2, (card.NumbersMatched - 1))); 
                }
            }
            return sum;
        }

        #endregion

        #region Part 2
        private string Part2()
        {
            List<ScratchCard> scratchCards = ParseInput(_input);
            //LogScratchCards(scratchCards);
            int sum = CalculateCardsTotal(scratchCards);

            return sum.ToString();
        }

        private int CalculateCardsTotal(List<ScratchCard> scratchCards)
        {
            int sum = 0;

            Dictionary<int, int> NumberOfCards = new Dictionary<int, int>();

            for(int i = 0; i < scratchCards.Count; i++)
            {
                ScratchCard card = scratchCards[i];
                if (card.NumbersMatched == 0)
                {
                    continue;
                }
                else
                {
                    int duplicates = 0;
                    if (NumberOfCards.ContainsKey(i))
                    {
                        duplicates = NumberOfCards[i] + 1;
                    }
                    else { duplicates = 1; }


                    for(int cardNumber = i + 1;  cardNumber <= i + card.NumbersMatched; cardNumber++)
                    {
                        if(NumberOfCards.ContainsKey(cardNumber))
                        {
                            NumberOfCards[cardNumber] += duplicates;
                        }
                        else
                        {
                            NumberOfCards[cardNumber] = duplicates;
                        }

                    }
                }
            }

            //LogTicketCounts(NumberOfCards);

            foreach(int TicketCount in  NumberOfCards.Values) 
            {
                sum += TicketCount;
            }
            sum += scratchCards.Count;
            return sum;
        }

        #endregion

        #region Utilities

        #endregion
    }
}