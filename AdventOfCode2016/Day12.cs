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
  internal class Day12
  {
    private static void Main12()
    {
      Dictionary<char, int> registers = new Dictionary<char, int> { { 'a', 0 }, { 'b', 0 }, { 'c', 1 }, { 'd', 0 } };

      new AssembunnyInterpreter().Execute(Input, registers);

      Console.WriteLine(registers['a']);
      Console.ReadKey();
    }

    private const string Input = @"cpy 1 a
cpy 1 b
cpy 26 d
jnz c 2
jnz 1 5
cpy 7 c
inc d
dec c
jnz c -2
cpy a c
inc a
dec b
jnz b -2
cpy c b
dec d
jnz d -6
cpy 18 c
cpy 11 d
inc a
dec d
jnz d -2
dec c
jnz c -5";

  }
}
