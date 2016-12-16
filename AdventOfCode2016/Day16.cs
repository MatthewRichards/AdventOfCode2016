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
  internal class Day16
  {
    private static void Main()
    {
      const string input = "11101000110010100";
      const int diskLength = 272;

      bool[] initialBits = input.Select(d => d == '1').ToArray();
      bool[] disk = new bool[diskLength];
      Array.Copy(initialBits, disk, initialBits.Length);

      // Repeatedly copy and invert 'a' to fill the disk
      int doneBits = initialBits.Length;

      while (doneBits < diskLength)
      {
        for (int i = 0; i < doneBits && doneBits + i + 1 < diskLength; i++)
        {
          disk[doneBits + i + 1] = !disk[doneBits - i - 1];
        }

        doneBits = doneBits*2 + 1;
      }

      // Calculate the checksum
      var checkSum = GetChecksum(disk);

      Console.WriteLine(string.Join("", checkSum.Select(b => b ? "1" : "0")));
      Console.ReadKey();
    }

    private static bool[] GetChecksum(bool[] source)
    {
      var initialChecksum = new bool[(int)Math.Ceiling(source.Length/2.0)];

      for (int i = 0; i < initialChecksum.Length; i++)
      {
        initialChecksum[i] = source[i*2] == source[i*2 + 1];
      }

      return initialChecksum.Length%2 == 0 ? GetChecksum(initialChecksum) : initialChecksum;
    }
  }
}
