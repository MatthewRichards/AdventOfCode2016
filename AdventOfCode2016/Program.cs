using System;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2016
{
  class Program
  {
    static void Main()
    {
      var latestSolution = typeof(Program).Assembly.GetTypes().Where(t => t.Name.StartsWith("Day")).OrderByDescending(t => int.Parse(t.Name.Substring(3))).First();

      var instance = latestSolution.GetConstructor(new Type[0]).Invoke(new object[0]);

      var clock = new Stopwatch();
      clock.Start();

      latestSolution.GetMethods().Single(m => m.Name == "Run").Invoke(instance, new object[0]);

      clock.Stop();
      Console.WriteLine($"Time taken: {clock.ElapsedMilliseconds}ms");
      Console.ReadKey();
    }
  }
}
