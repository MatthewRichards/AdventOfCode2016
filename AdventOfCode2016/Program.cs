using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2016
{
  class Day1
  {
    static void Main(string[] args)
    {
      const string input = "R4, R5, L5, L5, L3, R2, R1, R1, L5, R5, R2, L1, L3, L4, R3, L1, L1, R2, R3, R3, R1, L3, L5, R3, R1, L1, R1, R2, L1, L4, L5, R4, R2, L192, R5, L2, R53, R1, L5, R73, R5, L5, R186, L3, L2, R1, R3, L3, L3, R1, L4, L2, R3, L5, R4, R3, R1, L1, R5, R2, R1, R1, R1, R3, R2, L1, R5, R1, L5, R2, L2, L4, R3, L1, R4, L5, R4, R3, L5, L3, R4, R2, L5, L5, R2, R3, R5, R4, R2, R1, L1, L5, L2, L3, L4, L5, L4, L5, L1, R3, R4, R5, R3, L5, L4, L3, L1, L4, R2, R5, R5, R4, L2, L4, R3, R1, L2, R5, L5, R1, R1, L1, L5, L5, L2, L1, R5, R2, L4, L1, R4, R3, L3, R1, R5, L1, L4, R2, L3, R5, R3, R1, L3";
      
      int howFarEast = 0;
      int howFarNorth = 0;
      int directionEast = 0;
      int directionNorth = 1;

      foreach (string instruction in input.Split(',').Select(x => x.Trim()))
      {
        var turn = instruction[0];
        var distance = int.Parse(instruction.Substring(1));

        // Turn
        if (turn == 'R')
        {
          if (directionEast == 0)
          {
            directionEast = directionNorth;
            directionNorth = 0;
          }
          else
          {
            directionNorth = -directionEast;
            directionEast = 0;
          }
        }
        else
        {
          if (directionEast == 0)
          {
            directionEast = -directionNorth;
            directionNorth = 0;
          }
          else
          {
            directionNorth = directionEast;
            directionEast = 0;
          }
        }

        // Move
        howFarEast += distance * directionEast;
        howFarNorth += distance * directionNorth;
      }

      Console.WriteLine(Math.Abs(howFarNorth) + Math.Abs(howFarEast));
    }
  }
}
