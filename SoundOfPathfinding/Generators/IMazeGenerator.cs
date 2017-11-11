using SoundOfMazeGeneration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundOfMazeGeneration.Generators
{
    public interface IMazeGenerator
    {
        Cell NextStep();
        int RecommendedTimeStep { get; }
    }
}
