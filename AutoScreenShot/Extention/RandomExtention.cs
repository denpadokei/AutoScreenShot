using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoScreenShot.Extention
{
    public static class RandomExtention
    {
        public static float NextFloat(this System.Random random, float min, float max)
        {
            var diff = max - min;
            return min + (float)(random.NextDouble() * diff);
        }
    }
}
