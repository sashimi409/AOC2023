using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdventOfCode.Day05;


namespace AdventOfCode
{
    internal class Day05 : BaseDay
    {
        private List<long> Seeds;
        private List<SeedRange> SeedRanges;
        private Map SeedToSoil;
        private Map SoilToFertilizer;
        private Map FertilizerToWater;
        private Map WaterToLight;
        private Map LightToTemperature;
        private Map TemperatureToHumidity;
        private Map HumidityToLocation;
            
        #region Helper Structs

        public struct Map
        {
            public List<CoveredRange> mappedRanges;

            public long GetDestination(long Source)
            {
                for(int i = 0; i < mappedRanges.Count; i++)
                {
                    if (Source < mappedRanges[i].sourceStart)
                    {
                        continue;
                    }
                    else if(Source >= mappedRanges[i].sourceStart && Source <= mappedRanges[i].sourceEnd)
                    {
                        return mappedRanges[i].destStart + (Source - mappedRanges[i].sourceStart);
                    }
                }
                return Source;
            }

            public long GetSource(long Destination)
            {
                for (int i = 0; i < mappedRanges.Count; i++)
                {
                    if (Destination < mappedRanges[i].destStart)
                    {
                        continue;
                    }
                    else if (Destination >= mappedRanges[i].destStart && Destination <= mappedRanges[i].destEnd)
                    {
                        return mappedRanges[i].sourceStart + (Destination - mappedRanges[i].destStart);
                    }
                }
                return Destination;
            }

            public void SortDest()
            {
                mappedRanges.Sort((s1, s2) => s2.sourceStart.CompareTo(s1.sourceStart));
            }
        }

        public struct CoveredRange
        {
            public long sourceStart;
            public long sourceEnd;
            public long destStart;
            public long destEnd;
        }

        public struct InputRange
        {
            public int Skip;
            public int Take;
        }

        public struct SeedRange
        {
            public long start;
            public long length;
        }
        

        #endregion

        #region BoilerPlate

        private readonly string[] _input;

        public Day05()
        {
            _input = File.ReadAllLines(InputFilePath);
            ParseInput(_input);
        }

        public override ValueTask<string> Solve_1() => new(Part1());

        public override ValueTask<string> Solve_2() => new(Part2());

        #endregion

        #region Logging

        private void LogMap(Map map)
        {

            foreach(var range in map.mappedRanges)
            {
                Console.WriteLine("From {0, 12} to {1,12}: Start at {2}", range.sourceStart, range.sourceEnd, range.destStart);
            }
        }

        private void LogSeedRanges(List<SeedRange> seedRanges)
        {
            foreach(SeedRange range in seedRanges)
            {
                Console.WriteLine("From {0, 12} to {1,12}", range.start, range.length);
            }
        }

        #endregion

        #region Common


        private void ParseInput(string[] input)
        {
            //Seeds
            Seeds = input[0].Split(":")[1].Trim().Split(" ").Select(x => long.Parse(x)).ToList();
            SeedRanges = new List<SeedRange>();
            for (int i = 0; i < Seeds.Count; i = i+2)
            {
                SeedRange newRange = new SeedRange();
                newRange.start = Seeds[i];
                newRange.length = Seeds[i+1];
                SeedRanges.Add(newRange);
            }
            LogSeedRanges(SeedRanges);
 
            //SeedToSoil
            List<string> inputRange = GetMapRange("seed-to-soil map:", input);
            SeedToSoil = BuildMap(inputRange);
            LogMap(SeedToSoil);

            //SoilToFertilizer
            inputRange = GetMapRange("soil-to-fertilizer map:", input);
            SoilToFertilizer = BuildMap(inputRange);
            LogMap(SoilToFertilizer);

            //FertilizerToWater
            inputRange = GetMapRange("fertilizer-to-water map:", input);
            FertilizerToWater = BuildMap(inputRange);
            LogMap(FertilizerToWater);

            //WaterToLight
            inputRange = GetMapRange("water-to-light map:", input);
            WaterToLight = BuildMap(inputRange);
            LogMap(WaterToLight);

            //LightToTemperature
            inputRange = GetMapRange("light-to-temperature map:", input);
            LightToTemperature = BuildMap(inputRange);
            LogMap(LightToTemperature);

            //TemperatureToHumidity
            inputRange = GetMapRange("temperature-to-humidity map:", input);
            TemperatureToHumidity = BuildMap(inputRange);
            LogMap(TemperatureToHumidity);

            //HumidityToLocation
            inputRange = GetMapRange("humidity-to-location map:", input);
            HumidityToLocation = BuildMap(inputRange);
            LogMap(HumidityToLocation);
        }

        private List<string> GetMapRange(string header, string[] input)
        {
            List<string> output = new List<string>();
            bool foundBlock = false;
            for(int i = 0; i < input.Length; i++)
            {
                if (foundBlock)
                {
                    if (input[i] == "") 
                    {
                        break;
                    }
                    else
                    {
                        output.Add(input[i]);
                    }
                }
                else
                {
                    if (input[i] == header)
                    {
                        foundBlock = true;
                        continue;
                    }
                }
            }

            return output;
        }

        public Map BuildMap(List<string> input)
        {
            Map map = new Map();
            map.mappedRanges = new  List<CoveredRange>();

            foreach(string line in input)
            {
                CoveredRange newRange = new CoveredRange();

                newRange.sourceStart = long.Parse(line.Split(" ")[1]);
                newRange.sourceEnd = newRange.sourceStart + long.Parse(line.Split(" ")[2]) - 1;

                newRange.destStart = long.Parse(line.Split(" ")[0]);
                newRange.destEnd = newRange.destStart + long.Parse(line.Split(" ")[2]) - 1;

                map.mappedRanges.Add(newRange);
            }

            map.mappedRanges.Sort((s1, s2) => s1.sourceStart.CompareTo(s2.sourceStart));
            return map;
        }

        #endregion

        #region Part 1

        private string Part1()
        {
            long sum = GetClosestSeed();
            return sum.ToString();
        }

        public long GetClosestSeed()
        {
            long closest = GetLocationFromSeed(Seeds[0]);
            foreach(var seed in Seeds)
            {
                if(GetLocationFromSeed(seed) < closest)
                {
                    closest = GetLocationFromSeed(seed);
                }
            }
            return closest;
        }

        public long GetLocationFromSeed(long seed)
        {
            SeedToSoil.SortDest();
            long soil = SeedToSoil.GetDestination(seed);
            long Fertilizer = SoilToFertilizer.GetDestination(soil);
            long Water = FertilizerToWater.GetDestination(Fertilizer);
            long Light = WaterToLight.GetDestination(Water);
            long Temperature = LightToTemperature.GetDestination(Light);
            long Humidity = TemperatureToHumidity.GetDestination(Temperature);
            long Location = HumidityToLocation.GetDestination(Humidity);

            return Location;

        }

        #endregion
        
        #region Part 2
        private string Part2()
        {
            Console.WriteLine(GetSeedFromLocation(579439039).ToString());
            long sum = GetClosestSeedFromRange();
            return sum.ToString();
        }

        private long GetClosestSeedFromRange()
        {
            long TotalSeeds = CountTotalSeeds();
            Console.WriteLine("Total seeds: {0}", TotalSeeds);
            long Location = 0;
            bool SeedFound = false;
            while(!SeedFound)
            {
                long seed = GetSeedFromLocation(Location);
 //               Console.WriteLine("Seed Found: {0}", seed);
                foreach (var range in SeedRanges)
                {
                    if(seed >= range.start && seed <= (range.start + range.length -1) )
                    {
                        SeedFound = true;
                        return Location;
                        break;
                    }
                }
                Location++;
            }

            return Location;
        }

        public long GetSeedFromLocation(long Location)
        {
            long Humidity = HumidityToLocation.GetSource(Location);
            long Temperature = TemperatureToHumidity.GetSource(Humidity);
            long Light = LightToTemperature.GetSource(Temperature);
            long Water = WaterToLight.GetSource(Light);
            long Fertilizer = FertilizerToWater.GetSource(Water);
            long soil = SoilToFertilizer.GetSource(Fertilizer);
            long seed = SeedToSoil.GetSource(soil);

 //           Console.WriteLine("Location {0} matches Humidity {1} matches Temperature {2} matches Light {3} matches Water {4} matches Fertilizer {5} matches Soil {6} matches seed {7}", Location, Humidity, Temperature, Light, Water, Fertilizer, soil, seed);

            return seed;

        }

        public long CountTotalSeeds()
        {
            long TotalSeeds = 0;
            foreach (SeedRange range in SeedRanges)
            {
                TotalSeeds += range.length;
            }

            return TotalSeeds;
        }



        #endregion

        #region Utilities

        #endregion
    }
}