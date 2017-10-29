using SoundOfPathfinding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundOfPathfinding.Generators
{
    public interface IMazeGenerator
    {
        Cell NextStep();
    }
}
