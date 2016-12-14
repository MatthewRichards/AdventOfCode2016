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
  internal class Day13
  {
    private static void Main13()
    {
      int pathLength = 0;
      var cubiclesAtThisDistance = new List<Tuple<int, int>> {Tuple.Create(1, 1)};
      var cubiclesVisited = new HashSet<Tuple<int, int>> {Tuple.Create(1, 1)};
      //var targetCubicle = Tuple.Create(31, 39);
      var targetDistance = 50;

      while (pathLength < targetDistance)
      {
        pathLength++;
        var cubiclesAtNextDistance = new List<Tuple<int, int>>();

        foreach (var cubicle in cubiclesAtThisDistance)
        {
          foreach (var adjacentCubicle in GetAdjacentCubicles(cubicle, cubiclesVisited))
          {
            cubiclesAtNextDistance.Add(adjacentCubicle);
            cubiclesVisited.Add(adjacentCubicle);
          }
        }

        cubiclesAtThisDistance = cubiclesAtNextDistance;
      }

      Console.WriteLine(cubiclesVisited.Count);
      Console.ReadKey();
    }

    private static IEnumerable<Tuple<int, int>> GetAdjacentCubicles(Tuple<int, int> cubicle, HashSet<Tuple<int, int>> cubiclesVisited)
    {
      var north = Tuple.Create(cubicle.Item1, cubicle.Item2 - 1);
      var south = Tuple.Create(cubicle.Item1, cubicle.Item2 + 1);
      var east = Tuple.Create(cubicle.Item1 - 1, cubicle.Item2);
      var west = Tuple.Create(cubicle.Item1 + 1, cubicle.Item2);

      return new[] {north, south, east, west}.Where(c => IsCubicle(c.Item1, c.Item2) && !cubiclesVisited.Contains(c));
    }

    private static bool IsCubicle(int x, int y)
    {
      if (x < 0 || y < 0)
      {
        return false;
      }

      const int favouriteNumber = 1358;
      var calc = x*x + 3*x + 2*x*y + y + y*y + favouriteNumber;

      bool even = true;
      while (calc > 0)
      {
        if (calc%2 == 1)
        {
          even = !even;
        }

        calc = calc/2;
      }

      return even;
    }
  }
}
