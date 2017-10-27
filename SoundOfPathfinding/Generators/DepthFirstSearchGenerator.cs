using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoundOfPathfinding.Models;

namespace SoundOfPathfinding.Generators
{
    public class DepthFirstSearchGenerator : IMazeGenerator
    {
        private Maze maze;

        public DepthFirstSearchGenerator(Maze maze)
        {
            this.maze = maze;
        }

        public bool NextStep()
        {
            return true;
        }

    }
}
