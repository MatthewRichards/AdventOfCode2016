using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode2016
{
  internal class Day17
  {
    private static MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider();

    [Flags]
    private enum Directions
    {
      Up = 0x1,
      Down = 0x2,
      Left = 0x4,
      Right = 0x8,
      Calculated = 0x10
    }

    private static void Main()
    {
      var timer = new Stopwatch();
      timer.Start();

      const string passcode = @"ioramepc";
      byte[] initialRoute = Encoding.ASCII.GetBytes(passcode);

      byte[] path = new byte[initialRoute.Length + 1000];
      Array.Copy(initialRoute, path, initialRoute.Length);
      int pathIndex = initialRoute.Length;

      Directions[] validDirections = new Directions[path.Length];
      int[] xPositions = new int[path.Length];
      int[] yPositions = new int[path.Length];

      xPositions[pathIndex] = 0;
      yPositions[pathIndex] = 0;

      int longestRoute = int.MinValue;
      int shortestRoute = int.MaxValue;
      string shortestRouteDetail = "";

      while (pathIndex >= initialRoute.Length)
      {
        if (xPositions[pathIndex] == 3 && yPositions[pathIndex] == 3)
        {
          // Solution!
          if (pathIndex > longestRoute)
          {
            longestRoute = pathIndex;
          }

          if (pathIndex < shortestRoute)
          {
            shortestRoute = pathIndex;
            shortestRouteDetail = Encoding.ASCII.GetString(path, initialRoute.Length, pathIndex - initialRoute.Length);
          }

          // Can't go any further; go back and try another direction
          validDirections[pathIndex] = 0;
          pathIndex--;
          continue;
        }

        if (validDirections[pathIndex] == 0)
        {
          // Calculate where we can go from this position
          var hash = hasher.ComputeHash(path, 0, pathIndex);

          if (yPositions[pathIndex] > 0 && hash[0] >= 0xB0)
          {
            validDirections[pathIndex] |= Directions.Up;
          }

          if (yPositions[pathIndex] < 3 && (hash[0] & 0xF) >= 0xB)
          {
            validDirections[pathIndex] |= Directions.Down;
          }

          if (xPositions[pathIndex] > 0 && hash[1] >= 0xB0)
          {
            validDirections[pathIndex] |= Directions.Left;
          }

          if (xPositions[pathIndex] < 3 && (hash[1] & 0xF) >= 0xB)
          {
            validDirections[pathIndex] |= Directions.Right;
          }

          validDirections[pathIndex] |= Directions.Calculated;
        }

        // Try going in the first available direction
        if ((validDirections[pathIndex] & Directions.Up) != 0)
        {
          validDirections[pathIndex] = validDirections[pathIndex] ^ Directions.Up; // "Use up" this option so we don't try it again in future
          path[pathIndex] = (byte)'U';
          pathIndex++;
          xPositions[pathIndex] = xPositions[pathIndex - 1];
          yPositions[pathIndex] = yPositions[pathIndex - 1] - 1;
          continue;
        }

        if ((validDirections[pathIndex] & Directions.Down) != 0)
        {
          validDirections[pathIndex] = validDirections[pathIndex] ^ Directions.Down;
          path[pathIndex] = (byte)'D';
          pathIndex++;
          xPositions[pathIndex] = xPositions[pathIndex - 1];
          yPositions[pathIndex] = yPositions[pathIndex - 1] + 1;
          continue;
        }

        if ((validDirections[pathIndex] & Directions.Left) != 0)
        {
          validDirections[pathIndex] = validDirections[pathIndex] ^ Directions.Left;
          path[pathIndex] = (byte)'L';
          pathIndex++;
          xPositions[pathIndex] = xPositions[pathIndex - 1] - 1;
          yPositions[pathIndex] = yPositions[pathIndex - 1];
          continue;
        }

        if ((validDirections[pathIndex] & Directions.Right) != 0)
        {
          validDirections[pathIndex] = validDirections[pathIndex] ^ Directions.Right;
          path[pathIndex] = (byte)'R';
          pathIndex++;
          xPositions[pathIndex] = xPositions[pathIndex - 1] + 1;
          yPositions[pathIndex] = yPositions[pathIndex - 1];
          continue;
        }

        // We have nowhere to go - we're stuck! Go back and try another path
        validDirections[pathIndex] = 0;
        pathIndex--;
      }

      timer.Stop();

      Console.WriteLine($"Shortest route: {shortestRoute - initialRoute.Length} ({shortestRouteDetail})");
      Console.WriteLine($"Longest route: {longestRoute - initialRoute.Length}");
      Console.WriteLine($"Total time: {timer.ElapsedMilliseconds}ms");
      Console.ReadKey();
    }

    private static IEnumerable<State> GetNextStates(State currentState)
    {
      var hash = hasher.ComputeHash(currentState.Route);

      if (currentState.Y > 0 && hash[0] >= 0xB0)
      {
        yield return currentState.Move('U', 0, -1);
      }

      if (currentState.Y < 4 && (hash[0] & 0xF) >= 0xB)
      {
        yield return currentState.Move('D', 0, 1);
      }

      if (currentState.X > 0 && hash[1] >= 0xB0)
      {
        yield return currentState.Move('L', -1, 0);
      }

      if (currentState.X < 4 && (hash[1] & 0xF) >= 0xB)
      {
        yield return currentState.Move('R', 1, 0);
      }
    }

    private class State
    {
      public byte[] Route;
      public int Distance;
      public int X;
      public int Y;

      public State Move(char step, int dX, int dY)
      {
        var newRoute = new byte[Route.Length + 1];
        Array.Copy(Route, newRoute, Route.Length);
        newRoute[Route.Length] = (byte)step;

        return new State { Route = newRoute, Distance = Distance + 1, X = X + dX, Y = Y + dY };
      }
    }
    
  }
}
