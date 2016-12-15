using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2016
{
  internal class Day15
  {
    private static void Main()
    {
      var initialPositions = new[] {5, 8, 1, 7, 1, 0, 0};
      var limitPositions = new[] {17, 19, 7, 13, 5, 3, 11};

      var state = initialPositions.Zip(limitPositions, Tuple.Create).Select((tup, idx) => Tuple.Create((tup.Item1 + idx + 1) % tup.Item2, tup.Item2)).ToList();

      for (int i = 0;; i++)
      {
        if (state.All(tup => tup.Item1 == 0))
        {
          Console.WriteLine($"Found solution: {i}");
          Console.ReadKey();
        }

        state = state.Select(tup => Tuple.Create((tup.Item1 + 1)%tup.Item2, tup.Item2)).ToList();
      }
    }
    
  }
}
