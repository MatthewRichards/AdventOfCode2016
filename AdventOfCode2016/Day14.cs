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
  internal class Day14
  {
    private static void Main()
    {
      int index = 0;
      var buffer = new Queue<string>();
      var foundIndex = 0;

      for (int i = 0; i < 1000; i++)
      {
        buffer.Enqueue(GetCandidateKey(i));
      }

      while (foundIndex < 64)
      {
        var candidate = buffer.Dequeue();
        buffer.Enqueue(GetCandidateKey(index + 1000));

        if (IsValidKey(candidate, buffer))
        {
          foundIndex++;
          Console.WriteLine($"Found key {foundIndex} with index {index}");
        }

        index++;
      }

      Console.ReadKey();
    }

    private static string GetCandidateKey(int index)
    {
      const string salt = "jlmsuwbz";

      return string.Join("", new MD5CryptoServiceProvider()
        .ComputeHash(Encoding.ASCII.GetBytes(salt + index))
        .SelectMany(b => new[] { b >> 4, b & 0xF })
        .Select(n => "0123456789abcdef"[n]));
    }

    private static bool IsValidKey(string key, Queue<string> nextThousandKeys)
    {
      if (nextThousandKeys.Count != 1000)
      {
        throw new InvalidOperationException("Wrong number of keys to check!");
      }

      char match = '\0';

      for (int i = 2; i < key.Length; i++)
      {
        if (key[i] == key[i - 1] && key[i] == key[i - 2])
        {
          match = key[i];
          break;
        }
      }

      if (match == '\0') return false;

      string search = string.Join("", Enumerable.Repeat(match, 5));
      return nextThousandKeys.Any(k => k.Contains(search));
    }
    
  }
}
