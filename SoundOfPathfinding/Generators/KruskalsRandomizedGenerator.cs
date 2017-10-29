using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundOfPathfinding.Generators
{
    public class KruskalsRandomizedGenerator : IMazeGenerator
    {
        private Dictionary<Cell, HashSet<Cell>> _setByCell = new Dictionary<Cell, HashSet<Cell>>();

        public Cell NextStep()
        {
            return null;
        }
    }
}
