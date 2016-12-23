using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2016
{
  class AssembunnyInterpreter
  {
    public void Execute(string input, Dictionary<char, int> registers)
    {
      var program = input.SplitLines().Select(line => line.Split(' ')).ToArray();
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
    }
  }
}
