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
    private static void Run()
    {
      var timer = new Stopwatch();
      timer.Start();

      int elfCount = 3017957;
      bool[] elves = new bool[elfCount];
      int elfIndex = 0;
      int targetElfDistance = 0;
      int targetElf = 0;
      int remainingElves = elfCount;

      while (remainingElves > 1)
      {
        int acrossTheCircle = remainingElves / 2;

        for (int i = targetElfDistance; i < acrossTheCircle; i++)
        {
          do
          {
            targetElf = (targetElf + 1) % elfCount;
          } while (elves[targetElf]);
        }

        elves[targetElf] = true;
        remainingElves--;
        targetElfDistance = acrossTheCircle - 1;

        do
        {
          targetElf = (targetElf + 1)%elfCount;
        } while (elves[targetElf]); 

        do
        {
          elfIndex = (elfIndex + 1) % elfCount;
        } while (elves[elfIndex]);
      }

      timer.Stop();
      Console.WriteLine($"Final elf: {elfIndex + 1}");
      Console.WriteLine($"Total time: {timer.ElapsedMilliseconds}ms");
      Console.ReadKey();
    }

    private static void MainWithLinkedList()
    {
      var timer = new Stopwatch();
      timer.Start();

      int elfCount = 3017957;

      LinkedList<int> elves = new LinkedList<int>(Enumerable.Range(1, elfCount));
      var currentElf = elves.First;
      var targetElf = currentElf;
      var targetElfDistance = 0;
      var remainingElves = elfCount;

      while (remainingElves > 1)
      {
        int acrossTheCircle = remainingElves/2;
        
        for (int i = targetElfDistance; i < acrossTheCircle; i++)
        {
          targetElf = targetElf.Next ?? elves.First;
        }

        var nextTargetElf = targetElf.Next ?? elves.First;
        targetElf.List.Remove(targetElf);
        remainingElves--;

        targetElf = nextTargetElf;
        currentElf = currentElf.Next ?? elves.First;
        targetElfDistance = acrossTheCircle - 1;
      }

      timer.Stop();
      Console.WriteLine($"Final elf: {currentElf.Value}");
      Console.WriteLine($"Total time: {timer.ElapsedMilliseconds}ms");
      Console.ReadKey();
    }
  }
}
