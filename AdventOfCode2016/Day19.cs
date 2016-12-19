using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode2016
{
  internal class Day19
  {
    private static void Main()
    {
      var timer = new Stopwatch();
      timer.Start();

      int elfCount = 3017957;
      bool[] elves = new bool[elfCount];
      int elfIndex = 0;
      int lastPresentReceiver;

      while (true)
      {
        lastPresentReceiver = elfIndex;
        elfIndex = (elfIndex + 1) % elfCount;

        while (elves[elfIndex])
        {
          elfIndex = (elfIndex + 1) % elfCount;
        }

        if (elfIndex == lastPresentReceiver)
        {
          // This is the last elf!
          Console.WriteLine($"Last elf: {elfIndex + 1}");
          break;
        }

        elves[elfIndex] = true;

        do
        {
          elfIndex = (elfIndex + 1) % elfCount;
        } while (elves[elfIndex]);
      }

      timer.Stop();
      Console.WriteLine($"Total time: {timer.ElapsedMilliseconds}ms");
      Console.ReadKey();
    }
  }
}
