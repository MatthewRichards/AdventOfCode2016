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
  class Day5
  {
    static void Run()
    {
      const string doorId = "ojvtpuvg";
      var password = new char[8];
      var index = 0;
      var found = 0;
      var md5 = new MD5CryptoServiceProvider();

      while (found < 8)
      {
        var stringToHash = doorId + index;
        var bytesToHash = Encoding.ASCII.GetBytes(stringToHash);
        var hashedBytes = md5.ComputeHash(bytesToHash);

        if (hashedBytes[0] == 0 && hashedBytes[1] == 0 && ((hashedBytes[2] & 0x0F) == hashedBytes[2]))
        {
          var position = hashedBytes[2];

          if (position <= 7 && password[position] == '\0')
          {
            var hexDigit = "0123456789abcdef"[hashedBytes[3]/0x10];
            password[position] = hexDigit;
            found++;
            Console.WriteLine(string.Join("", password));
          }
        }

        index++;
      }

      Console.WriteLine(string.Join("", password));
      Console.ReadKey();
    }
  }
}
