using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundOfMazeGeneration.Sound
{
    class Tones
    {
        private static readonly double _baseFrequency = 65.41;
        private static readonly double _totalHalfSteps = 84;

        public static double CalculateFrequency(double step, double totalSteps)
        {
            var relativeStep = step * _totalHalfSteps / totalSteps;
            return _baseFrequency * Math.Pow(Math.Pow(2, 1.0 / 12.0), relativeStep);
        }
    }
}
