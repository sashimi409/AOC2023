using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Day07 : BaseDay
    {
        private List<Hand> Game;

        #region Helper Structs

        public Dictionary<char, int> CardValues = new Dictionary<char, int>()
        {
            { 'A', 1 },
            { 'K', 2 },
            { 'Q', 3 },
            { 'J', 4 },
            { 'T', 5 },
            { '9', 6 },
            { '8', 7 },
            { '7', 8 },
            { '6', 9 },
            { '5', 10 },
            { '4', 11 },
            { '3', 12 },
            { '2', 13 }
        };

        public Dictionary<char, int> CardValuesWithJoker = new Dictionary<char, int>()
        {
            { 'A', 1 },
            { 'K', 2 },
            { 'Q', 3 },
            { 'T', 4 },
            { '9', 5 },
            { '8', 6 },
            { '7', 7 },
            { '6', 8 },
            { '5', 9 },
            { '4', 10 },
            { '3', 11 },
            { '2', 12 },
            { 'J', 13 }
        };

        public enum HandRanks { FiveOfAKind, FourOfAKind, FullHouse, ThreeOfAKind, TwoPair, OnePair, HighCard }
        public struct Hand
        {
            public string Cards;
            public long bid;
            public Dictionary<char, int> NumberOfCard;
            public HandRanks HandRank;
            public HandRanks HandRankWithJoker;

            public void CalculateHand()
            {
                List<int> CardCounts = NumberOfCard.Values.ToList();
                CardCounts.Sort();
                CardCounts.Reverse();
                switch(CardCounts[0])
                {
                    case 5:
                        HandRank = HandRanks.FiveOfAKind;
                        break;
                    case 4:
                        HandRank = HandRanks.FourOfAKind;
                        break;
                    case 3:

                        if (CardCounts[1] == 2)
                        {
                            HandRank = HandRanks.FullHouse;
                        }
                        else
                        {
                            HandRank = HandRanks.ThreeOfAKind;
                        }
                        break;
                    case 2:
                        if (CardCounts[1] == 2)
                        {
                            HandRank = HandRanks.TwoPair;
                        }
                        else
                        {
                            HandRank = HandRanks.OnePair;
                        }
                        break;
                    case 1:
                        HandRank = HandRanks.HighCard;
                        break;
                }

            }

            public void CalculateHandWithJoker()
            {

                int NumberOfJokers;

                if (NumberOfCard.ContainsKey('J'))
                {
                    NumberOfJokers = NumberOfCard['J'];
                    NumberOfCard.Remove('J');
                }
                else
                {
                    NumberOfJokers = 0;
                }

                List<int> CardCounts = NumberOfCard.Values.ToList();
                CardCounts.Sort();
                CardCounts.Reverse();

                if (CardCounts.Count == 0)
                {
                    HandRankWithJoker = HandRanks.FiveOfAKind;
                }
                else
                {
                    switch (CardCounts[0] + NumberOfJokers)
                    {
                        case 5:
                            HandRankWithJoker = HandRanks.FiveOfAKind;
                            break;
                        case 4:
                            HandRankWithJoker = HandRanks.FourOfAKind;
                            break;
                        case 3:
                            if (CardCounts.Count > 1)
                            {
                                if (CardCounts[1] == 2)
                                {
                                    HandRankWithJoker = HandRanks.FullHouse;
                                }
                                else
                                {
                                    HandRankWithJoker = HandRanks.ThreeOfAKind;
                                }
                            }
                            else
                            {
                                HandRankWithJoker = HandRanks.ThreeOfAKind;
                            }
                            break;
                        case 2:
                            if (CardCounts.Count > 1)
                            {
                                if (CardCounts[1] == 2)
                                {
                                    HandRankWithJoker = HandRanks.TwoPair;
                                }
                                else
                                {
                                    HandRankWithJoker = HandRanks.OnePair;
                                }
                            }
                            else
                            {
                                HandRankWithJoker = HandRanks.OnePair;
                            }
                            break;
                        case 1:
                            HandRankWithJoker = HandRanks.HighCard;
                            break;
                    }

                    if (HandRank < HandRankWithJoker)
                    {
                        HandRankWithJoker = HandRank;
                    }
                }
            }
        }


        #endregion


        #region BoilerPlate

        private readonly string[] _input;

        public Day07()
        {
            _input = File.ReadAllLines(InputFilePath);
            ParseInput(_input);
        }

        public override ValueTask<string> Solve_1() => new(Part1());

        public override ValueTask<string> Solve_2() => new(Part2());

        #endregion

        #region Logging

        public void LogGame(List<Hand> Game)
        {
            foreach (Hand hand in Game)
            {
                Console.WriteLine("{0} | {1, 13} | {2}", hand.Cards, hand.HandRank.ToString(), hand.HandRankWithJoker.ToString());
            } 
        }

        #endregion

        #region Common


        private void ParseInput(string[] input)
        {
            Game = new List<Hand>();
            foreach (string line in input)
            {
                Hand newHand = new Hand()
                {
                    Cards = line.Split(" ")[0],
                    bid = int.Parse(line.Split(" ")[1]),
                    NumberOfCard = new Dictionary<char, int>()
                };

                foreach(char Card in newHand.Cards)
                {
                    if(newHand.NumberOfCard.ContainsKey(Card))
                    {
                        newHand.NumberOfCard[Card]++;
                    }
                    else
                    {
                        newHand.NumberOfCard[Card] = 1;
                    }
                }
                newHand.CalculateHand();
                newHand.CalculateHandWithJoker();
                Game.Add(newHand);
            }

        }

        #endregion

        #region Part 1

        private string Part1()
        {
            //Console.WriteLine("Before Sort");
            //LogGame(Game);
            Game.Sort(Part1SortCompare);
            Console.WriteLine();
            //Console.WriteLine("After Sort");
            //Game.Reverse();
            LogGame(Game);
            long sum = 0;
            for(int i = 0; i < Game.Count; i++)
            {
                sum += Game[i].bid * (i+1);
            }
            return sum.ToString();
        }

        public int Part1SortCompare(Hand hand1, Hand hand2) 
        {
            if (hand1.HandRank > hand2.HandRank)
                return 1;
            if (hand1.HandRank < hand2.HandRank)
                return -1;
            for(int i = 0; i< 5; i++)
            {
                if (CardValues[hand1.Cards[i]] > CardValues[hand2.Cards[i]])
                {
                    return 1;
                }
                else if (CardValues[hand1.Cards[i]] < CardValues[hand2.Cards[i]])
                {
                    return -1;
                }
            }
            return 0;
        }
        #endregion

        #region Part 2
        private string Part2()
        {
            //Console.WriteLine("Before Sort");
            //LogGame(Game);
            Game.Sort(Part2SortCompare);
            Console.WriteLine();
            Console.WriteLine("After Sort");
            Game.Reverse();
            LogGame(Game);
            long sum = 0;
            for (int i = 0; i < Game.Count; i++)
            {
                sum += Game[i].bid * (i + 1);
            }
            return sum.ToString();
        }

        public int Part2SortCompare(Hand hand1, Hand hand2)
        {
            if (hand1.HandRankWithJoker > hand2.HandRankWithJoker)
                return 1;
            if (hand1.HandRankWithJoker < hand2.HandRankWithJoker)
                return -1;
            for (int i = 0; i < 5; i++)
            {
                if (CardValuesWithJoker[hand1.Cards[i]] > CardValuesWithJoker[hand2.Cards[i]])
                {
                    return 1;
                }
                else if (CardValuesWithJoker[hand1.Cards[i]] < CardValuesWithJoker[hand2.Cards[i]])
                {
                    return -1;
                }
            }
            return 0;
        }


            #endregion

            #region Utilities

            #endregion
        }
    }
