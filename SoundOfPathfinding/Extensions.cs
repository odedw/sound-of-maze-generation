using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundOfPathfinding
{
    public static class GeneralExtensions
    {
        public static T RandomElement<T>(this IEnumerable<T> enumerable, Random rand)
        {
            int index = rand.Next(0, enumerable.Count());
            return enumerable.ElementAt(index);
        }
    }
}
