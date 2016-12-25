using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2016
{
  internal class Day25
  {
    public static void Run()
    {
      for(int a = 1;a<1000;a++)
      {
        var interpreter = new AssembunnyInterpreter();
        interpreter.RequiredOutput = ClockSequence();

        var registers = new Dictionary<char, long> { { 'a', a }, { 'b', 0 }, { 'c', 0 }, { 'd', 0 } };
        interpreter.Execute(Input, registers);

        Console.WriteLine($"The answer is not {a}");
      }
    }
    
    private static IEnumerable<int> ClockSequence()
    {
      while (true)
      {
        yield return 0;
        yield return 1;
      }
    }

    private const string Input = @"cpy a d
cpy 7 c
cpy 365 b
inc d
dec b
jnz b -2
dec c
jnz c -5
cpy d a
jnz 0 0
cpy a b
cpy 0 a
cpy 2 c
jnz b 2
jnz 1 6
dec b
dec c
jnz c -4
inc a
jnz 1 -7
cpy 2 b
jnz c 2
jnz 1 4
dec b
dec c
jnz 1 -4
jnz 0 0
out b
jnz a -19
jnz 1 -21";

 }
}
