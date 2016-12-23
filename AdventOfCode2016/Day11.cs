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
  internal class Day11
  {
    private static void Run()
    {
      var searchList = new Queue<Tuple<ContainmentArea, int>>();
      var seenPositions = new HashSet<string>();
      searchList.Enqueue(Tuple.Create(ContainmentArea.InitialSetup, 0));
      var stepsSoFar = 0;

      while (true)
      {
        var current = searchList.Dequeue();
        var stepsTaken = current.Item2 + 1;

        if (stepsSoFar < stepsTaken)
        {
          stepsSoFar = stepsTaken;
          Console.WriteLine($"On to {stepsSoFar} steps...");
        }

        foreach (var next in current.Item1.GetNextPositions())
        {
          if (next.IsSolution())
          {
            Console.WriteLine($"Found solution after {stepsTaken} steps");
            Console.ReadKey();
            return;
          }

          var uniqueCode = next.GetUniqueCode();

          if (seenPositions.Contains(uniqueCode))
          {
            continue;
          }

          seenPositions.Add(uniqueCode);
          searchList.Enqueue(Tuple.Create(next, stepsTaken));
        }
      }
    }

    public enum Element
    {
      Hydrogen,
      Lithium,
      Strontium,
      Plutonium,
      Thulium,
      Ruthenium,
      Curium,
      Elerium,
      Dilithium
    }

    public class Thing
    {
      public Element Element;
      public int Floor;

      public T Clone<T>() where T : Thing, new()
      {
        return new T
        {
          Element = Element,
          Floor = Floor
        };
      }
    }

    public class Generator : Thing { }
    public class Microchip : Thing { }

    public class ContainmentArea
    {
      public IEnumerable<Generator> Generators;
      public IEnumerable<Microchip> Microchips;
      public int LiftLocation;
      
      public static ContainmentArea TestSetup
      {
        get
        {
          return new ContainmentArea
          {
            Generators = new[]
            {
              new Generator { Element = Element.Hydrogen, Floor = 2 },
              new Generator { Element = Element.Lithium, Floor = 3 }
            },
            Microchips = new[]
            {
              new Microchip { Element = Element.Hydrogen, Floor = 1 },
              new Microchip { Element = Element.Lithium, Floor = 1 }
            },
            LiftLocation = 1
          };
        }
      }

      public static ContainmentArea InitialSetup
      {
        get
        {
          return new ContainmentArea
          {
            Generators = new[]
            {
              new Generator { Element = Element.Strontium, Floor = 1 },
              new Generator { Element = Element.Plutonium, Floor = 1 },
              new Generator { Element = Element.Thulium, Floor = 2 },
              new Generator { Element = Element.Ruthenium, Floor = 2 },
              new Generator { Element = Element.Curium, Floor = 2 },
              new Generator { Element = Element.Elerium, Floor = 1 },
              new Generator { Element = Element.Dilithium, Floor = 1 }
            },
            Microchips = new[]
            {
              new Microchip { Element = Element.Strontium, Floor = 1 },
              new Microchip { Element = Element.Plutonium, Floor = 1 },
              new Microchip { Element = Element.Ruthenium, Floor = 2 },
              new Microchip { Element = Element.Curium, Floor = 2 },
              new Microchip { Element = Element.Thulium, Floor = 3 },
              new Microchip { Element = Element.Elerium, Floor = 1 },
              new Microchip { Element = Element.Dilithium, Floor = 1 }
            },
            LiftLocation = 1
          };
        }
      }

      public ContainmentArea CloneAtNewFloor(int newFloor)
      {
        return new ContainmentArea
        {
          Generators = Generators.Select(g => g.Clone<Generator>()).ToList(),
          Microchips = Microchips.Select(m => m.Clone<Microchip>()).ToList(),
          LiftLocation = newFloor
        };
      }

      public ContainmentArea WithGeneratorAt(Element element, int floor)
      {
        Generators.Single(g => g.Element == element).Floor = floor;
        return this;
      }

      public ContainmentArea WithMicrochipAt(Element element, int floor)
      {
        Microchips.Single(m => m.Element == element).Floor = floor;
        return this;
      }

      public IEnumerable<ContainmentArea> GetNextPositions()
      {
        foreach (int nextFloor in new[] { LiftLocation - 1, LiftLocation + 1 }.Where(l => l >= 1 && l <= 4))
        {
          // What can go in the lift? Either one or two microchips, or one or two generators, or one of each but only if they're for the same element
          foreach (var generator in Generators.Where(g => g.Floor == LiftLocation))
          {
            var moveGenerator = CloneAtNewFloor(nextFloor).WithGeneratorAt(generator.Element, nextFloor);
            if (moveGenerator.IsSafe()) yield return moveGenerator;

            if (Microchips.Single(m => m.Element == generator.Element).Floor == LiftLocation)
            {
              var moveGeneratorWithChip = CloneAtNewFloor(nextFloor).WithGeneratorAt(generator.Element, nextFloor).WithMicrochipAt(generator.Element, nextFloor);
              if (moveGeneratorWithChip.IsSafe()) yield return moveGeneratorWithChip;
            }

            foreach (var secondGenerator in Generators.Where(g => g.Floor == LiftLocation && g != generator))
            {
              var moveTwoGenerators = CloneAtNewFloor(nextFloor).WithGeneratorAt(generator.Element, nextFloor).WithGeneratorAt(secondGenerator.Element, nextFloor);
              if (moveTwoGenerators.IsSafe()) yield return moveTwoGenerators;
            }
          }

          foreach (var microchip in Microchips.Where(m => m.Floor == LiftLocation))
          {
            var moveMicrochip = CloneAtNewFloor(nextFloor).WithMicrochipAt(microchip.Element, nextFloor);
            if (moveMicrochip.IsSafe()) yield return moveMicrochip;

            foreach (var secondMicrochip in Microchips.Where(m => m.Floor == LiftLocation && m != microchip))
            {
              var moveTwoMicrochips = CloneAtNewFloor(nextFloor).WithMicrochipAt(microchip.Element, nextFloor).WithMicrochipAt(secondMicrochip.Element, nextFloor);
              if (moveTwoMicrochips.IsSafe()) yield return moveTwoMicrochips;
            }
          }
        }
      }

      private bool IsSafe()
      {
        return Microchips.All(
          m => Generators.Single(g => g.Element == m.Element).Floor == m.Floor || Generators.All(g => g.Floor != m.Floor));
      }

      public bool IsSolution()
      {
        return Microchips.All(m => m.Floor == 4) && Generators.All(g => g.Floor == 4);
      }

      public void Print()
      {
        for (int floor = 4; floor >= 1; floor--)
        {
          var generatorString = string.Join(" ", Generators.Select(g => (g.Floor == floor) ? g.Element.ToString()[0] + "G" : ". "));
          var microchipString = string.Join(" ", Microchips.Select(m => (m.Floor == floor) ? m.Element.ToString()[0] + "M" : ". "));
          var elevatorString = LiftLocation == floor ? "E" : " ";

          Console.WriteLine($"F{floor} {elevatorString} {generatorString} {microchipString}");
        }
      }

      public string GetUniqueCode()
      {
        return $"{LiftLocation}{string.Join("", Generators.Select(g => g.Floor))}{string.Join("", Microchips.Select(m => m.Floor))}";
      }
    }

  }
}
