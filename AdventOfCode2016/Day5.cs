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
    static void Main()
    {
      const string doorId = "ojvtpuvg";
      var password = "";
      var index = 0;
      var md5 = new MD5CryptoServiceProvider();

      while (password.Length < 8)
      {
        var stringToHash = doorId + index;
        var bytesToHash = Encoding.ASCII.GetBytes(stringToHash);
        var hashedBytes = md5.ComputeHash(bytesToHash);

        if (hashedBytes[0] == 0 && hashedBytes[1] == 0 && ((hashedBytes[2] & 0x0F) == hashedBytes[2]))
        {
          // First five hex digits are 0, so take the sixth hex digit and add to the password
          var hexDigit = "0123456789abcdef"[hashedBytes[2]];
          password += hexDigit;
          Console.WriteLine(password);
        }

        index++;
      }

      Console.WriteLine(password);
      Console.ReadKey();
    }
  }
}
