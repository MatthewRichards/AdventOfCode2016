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
    private static void Main()
    {
      var program = Input.SplitLines().Select(line => line.Split(' ')).ToArray();

      Dictionary<char, int> registers = new Dictionary<char, int> {{'a', 0}, {'b', 0}, {'c', 0}, {'d', 0}};
      int instr = 0;

      while (instr < program.Length)
      {
        int nextInstr = instr + 1;

        switch (program[instr][0])
        {
          case "cpy":
            var source = program[instr][1];
            int sourceValue;
            if (!int.TryParse(source, out sourceValue))
            {
              sourceValue = registers[source.Single()];
            }

            var targetRegister = program[instr][2].Single();
            registers[targetRegister] = sourceValue;
            break;

          case "inc":
            var registerToInc = program[instr][1].Single();
            registers[registerToInc] = registers[registerToInc] + 1;
            break;

          case "dec":
            var registerToDec = program[instr][1].Single();
            registers[registerToDec] = registers[registerToDec] - 1;
            break;

          case "jnz":
            var variableToTest = program[instr][1];
            int valueToTest;
            if (!int.TryParse(variableToTest, out valueToTest))
            {
              valueToTest = registers[variableToTest.Single()];
            }

            if (valueToTest != 0)
            {
              nextInstr = instr + int.Parse(program[instr][2]);
            }
            break;
        }

        instr = nextInstr;
      }

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
