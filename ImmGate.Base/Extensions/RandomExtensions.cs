using System;

namespace ImmGate.Base.Extensions
{
    public static class RandomExtensions
    {

        private static readonly Random Rnd = new Random(Environment.TickCount);
        public static bool IsRandomChanceOccured(int chanceInPercent)
        {
            if (chanceInPercent == 0)
                return false;
            var randomValue = Rnd.Next(1, 100);
            return randomValue <= chanceInPercent;
        }

        public static int Next(int minValue, int maxValue)
        {
            return Rnd.Next(minValue, maxValue);
        }
    }
}