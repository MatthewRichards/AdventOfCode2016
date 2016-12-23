using System;
using System.Collections.Generic;

namespace AdventOfCode2016
{
  internal class Day23
  {
    public static void Run()
    {
      var testRegisters = new Dictionary<char, long> { { 'a', 0 }, { 'b', 0 }, { 'c', 0 }, { 'd', 0 } };
      new AssembunnyInterpreter().Execute(TestInput, testRegisters);
      Console.WriteLine($"Test output: {testRegisters['a']}");

      var registers = new Dictionary<char, long> { { 'a', 12 }, { 'b', 0 }, { 'c', 0 }, { 'd', 0 } };
      new AssembunnyInterpreter().Execute(Input, registers);
      Console.WriteLine($"Real output: {registers['a']}");
    }

    private const string TestInput = @"cpy 2 a
tgl a
tgl a
tgl a
cpy 1 a
dec a
dec a";

    /* 
Replaced:
cpy a d
cpy 0 a
cpy b c
inc a
dec c
jnz c -2
dec d
jnz d -5

With:
mul a b
nop
nop
nop
nop
nop
nop
nop
     */
    private const string Input = @"cpy a b
dec b
mul a b
nop
nop
nop
nop
nop
nop
nop
dec b
cpy b c
cpy c d
dec d
inc c
jnz d -2
tgl c
cpy -16 c
jnz 1 c
cpy 94 c
jnz 80 d
inc a
inc d
jnz d -2
inc c
jnz c -5";

 }
}
