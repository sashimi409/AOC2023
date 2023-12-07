using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day01 : BaseDay
{
    private readonly string[] _input;

    public Day01()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new(Part1());

    public override ValueTask<string> Solve_2() => new(Part2());

    public string Part1()
    {
        int sum = 0;

        Regex rx = new Regex(@"\d",
          RegexOptions.Compiled | RegexOptions.IgnoreCase); ;

        foreach(string line in _input)
        {
            MatchCollection Matches = rx.Matches(line);
            string firstNumber = Matches[0].Value;
            string secondNumber = Matches[Matches.Count - 1].Value;
            int CombinedNumber = int.Parse(firstNumber + secondNumber);

            sum += CombinedNumber;
        }

        return sum.ToString();
    }


    public string Part2()
    {
        int sum = 0;

        Regex rx = new Regex(@"(?=(\d|one|two|three|four|five|six|seven|eight|nine))",
          RegexOptions.Compiled | RegexOptions.IgnoreCase); ;

        foreach (string line in _input)
        {
            MatchCollection Matches = rx.Matches(line);
            string firstNumber = GetNumber(Matches[0].Groups[1].Value);
            string secondNumber = GetNumber(Matches[Matches.Count - 1].Groups[1].Value);
            int CombinedNumber = int.Parse(firstNumber + secondNumber);

            sum += CombinedNumber;
        }

        return sum.ToString();
    }

   private string GetNumber(string input) {
        string result = "";
        if(input.Length > 1)
        {
            Dictionary<string, string> Numbers = new Dictionary<string, string>()
            {
                    {"one", "1"},
                    {"two", "2"},
                    {"three", "3"},
                    {"four", "4"},
                    {"five", "5"},
                    {"six", "6"},
                    {"seven", "7"},
                    {"eight", "8"},
                    {"nine", "9"}
            };
            result = Numbers[input];
        }
        else
        {
            result = input;
             
        }

        return result;
    }

}
