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
            if (registers.ContainsKey(targetRegister))
            {
              registers[targetRegister] = sourceValue;
            }
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
              int jump;
              if (!int.TryParse(program[instr][2], out jump))
              {
                jump = registers[program[instr][2].Single()];
              }
              nextInstr = instr + jump;
            }
            break;

          case "tgl":
            int instrToToggle;
            if (!int.TryParse(program[instr][1], out instrToToggle))
            {
              instrToToggle = registers[program[instr][1].Single()];
            }

            instrToToggle += instr;
            if (instrToToggle < 0 || instrToToggle >= program.Length)
            {
              break;
            }

            if (program[instrToToggle].Length == 2)
            {
              program[instrToToggle][0] =
                program[instrToToggle][0] == "inc" ? "dec" : "inc";
            }
            else
            {
              program[instrToToggle][0] =
                program[instrToToggle][0] == "jnz" ? "cpy" : "jnz";
            }
            break;
        }

        instr = nextInstr;
      }
    }
  }
}
