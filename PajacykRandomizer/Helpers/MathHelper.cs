using MathNet.Numerics.Distributions;
using System;

namespace PajacykRandomizer.Helpers
{
    internal class MathHelper
    {
        public static Random GenerateRandomWithSeed() =>
            new Random((int)(DateTime.Now.ToFileTime() % int.MaxValue));

        public static int GetIntNormalizedSample(Normal normal) =>
            (int)Math.Ceiling(GetDoubleNormalizedSample(normal));

        public static double GetDoubleNormalizedSample(Normal normal) =>
            Math.Abs(normal.Sample());
    }
}
