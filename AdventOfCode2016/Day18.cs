using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode2016
{
  internal class Day18
  {
    private static void Run()
    {
      var timer = new Stopwatch();
      timer.Start();

      const string input = @".^..^....^....^^.^^.^.^^.^.....^.^..^...^^^^^^.^^^^.^.^^^^^^^.^^^^^..^.^^^.^^..^.^^.^....^.^...^^.^.";
      int safeCells = CountSafe(input);

      string thisRow = input;

      for (int i = 1; i < 400000; i++)
      {
        char[] nextRow = new char[thisRow.Length];

        for (int j = 0; j < nextRow.Length; j++)
        {
          var left = j == 0 ? false : thisRow[j - 1] == '^';
          var centre = thisRow[j] == '^';
          var right = j == nextRow.Length - 1 ? false : thisRow[j + 1] == '^';

          nextRow[j] = (left && centre && !right) || (centre && right && !left) || (left && !centre && !right) || (right && !centre && !left) ? '^' : '.';
        }

        thisRow = string.Join("", nextRow);
        safeCells += CountSafe(thisRow);

        if (i == 39)
        {
          Console.WriteLine($"40 rows: {safeCells}");
        }
      }

      timer.Stop();

      Console.WriteLine($"400000 rows: {safeCells}");
      Console.WriteLine($"Total time: {timer.ElapsedMilliseconds}ms");
      Console.ReadKey();
    }

    private static int CountSafe(string row)
    {
      return row.Count(c => c == '.');
    }
  }
}
