using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    namespace AdventOfCode
    {
        internal class Template : BaseDay
        {

            #region Helper Structs

            #endregion

            #region BoilerPlate

            private readonly string[] _input;

            public Template()
            {
                _input = File.ReadAllLines(InputFilePath);
            }

            public override ValueTask<string> Solve_1() => new(Part1());

            public override ValueTask<string> Solve_2() => new(Part2());

            #endregion

            #region Logging

            #endregion

            #region Common


            #endregion

            #region Part 1

            private string Part1()
            {
                int sum = 0;

                return sum.ToString();
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

}
