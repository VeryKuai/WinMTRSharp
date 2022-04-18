using System;
using System.Collections.Generic;

namespace WinMTRSharp.Utility
{
    public static class StDev
    {
        public static double StandardDeviation(this IEnumerable<double> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            double num = 0.0;
            double num2 = 0.0;
            ulong length = 0UL;
            using (IEnumerator<double> enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    length += 1UL;
                    num2 = enumerator.Current;
                }
                while (enumerator.MoveNext())
                {
                    length += 1UL;
                    double sample = enumerator.Current;
                    num2 += sample;
                    double num3 = length * sample - num2;
                    num += num3 * num3 / (length * (length - 1UL));
                }
            }
            if (length <= 1UL)
            {
                return double.NaN;
            }
            return Math.Sqrt(num / (length - 1UL));
        }
    }
}
