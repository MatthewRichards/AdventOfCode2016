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
  internal class Day17
  {
    private static MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider();

    private static void Main()
    {
      const string passcode = @"ioramepc";
      byte[] initialRoute = Encoding.ASCII.GetBytes(passcode);
      var states = new Queue<State>(new[] { new State { Route = initialRoute, Distance = 0, X = 0, Y = 0 } });

      while (true)
      {
        var currentState = states.Dequeue();
        foreach (var nextState in GetNextStates(currentState))
        {
          if (nextState.X == 3 && nextState.Y == 3)
          {
            Console.WriteLine(Encoding.ASCII.GetString(nextState.Route));
            Console.ReadKey();
          }

          states.Enqueue(nextState);
        }
      }
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
