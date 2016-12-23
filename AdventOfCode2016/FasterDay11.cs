using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2016
{
  internal class FasterDay11
  {
    private static void Run()
    {
      var timer = new Stopwatch();
      timer.Start();

      // Representation of the state of the system in a long:
      //    Low 4 bits: Which floor the elevator is on (lowest bit floor 1, next bit floor 2, etc.)
      //    Next 7 sets of 4 bits: Which floor each of the microchips is on (Sr lowest, then Pu, Tm, Ru, Cm, El (made that one up), Li2 highest)
      //    Next 7 sets of 4 bits: Which floor each of the generators is on (similarly)
      //    High 4 bits: Reserved for future expansion :-)

      //                             Li2 El Cm Ru Tm Pu Sr
      var initialMicrochips = new[] { 1, 1, 3, 2, 2, 1, 1 }.Select(floor => 0x1L << (floor - 1)).Aggregate(0L, (acc, floor) => (acc << 4) | floor);
      var initialGenerators = new[] { 1, 1, 2, 2, 2, 1, 1 }.Select(floor => 0x1L << (floor - 1)).Aggregate(0L, (acc, floor) => (acc << 4) | floor);
      var initialState = (((initialGenerators << (7 * 4)) | initialMicrochips) << 4) | 0x1L;

      var targetMicrochips = new[] { 4, 4, 4, 4, 4, 4, 4 }.Select(floor => 0x1L << (floor - 1)).Aggregate(0L, (acc, floor) => (acc << 4) | floor);
      var targetGenerators = new[] { 4, 4, 4, 4, 4, 4, 4 }.Select(floor => 0x1L << (floor - 1)).Aggregate(0L, (acc, floor) => (acc << 4) | floor);
      var targetState = (((targetGenerators << (7 * 4)) | targetMicrochips) << 4) | 0x8L;

      var positionsAlreadySeen = new HashSet<long>();
      var positionsReachedAtCurrentStep = new List<long> { initialState };
      var currentStep = 0;

      while (!positionsReachedAtCurrentStep.Contains(targetState))
      {
        var positionsReachedAtNextStep = new List<long>();

        foreach (var state in positionsReachedAtCurrentStep)
        {
          foreach (var nextState in GetSafeNextStates(state))
          {
            if (!positionsAlreadySeen.Contains(nextState))
            {
              positionsReachedAtNextStep.Add(nextState);
              positionsAlreadySeen.Add(nextState);
            }
          }
        }

        currentStep++;
        positionsReachedAtCurrentStep = positionsReachedAtNextStep;
      }

      timer.Stop();
      Console.WriteLine($"Answer of {currentStep} found in {timer.ElapsedMilliseconds}ms");
      Console.ReadKey();
    }


    private static IEnumerable<long> GetSafeNextStates(long currentState)
    {
      var floorUp = (currentState << 1) & 0xF;
      var floorDown = (currentState & 0xF) >> 1;

      foreach (long nextFloor in new[] { floorUp, floorDown }.Where(f => f != 0))
      {
        foreach (var nextState in GetNextStates(currentState, nextFloor))
        {
          if (IsSafe(nextState))
          {
            yield return nextState;
          }
        }
      }
    }

    private static IEnumerable<long> GetNextStates(long currentState, long nextFloor)
    {
      var currentFloor = currentState & 0xF;
      var currentStateWithNewFloor = currentState ^ (currentFloor | nextFloor);

      // What can go in the lift? Either one or two microchips, or one or two generators, or one of each but only if they're for the same element

      // For each element
      var microchips = currentState;
      var generators = microchips >> (7 * 4);
      var microchipMovingXor = currentFloor | nextFloor;
      var generatorMovingXor = microchipMovingXor << (7 * 4);

      for (int element = 0; element < 7; element++)
      {
        microchips >>= 4;
        generators >>= 4;
        microchipMovingXor <<= 4;
        generatorMovingXor <<= 4;

        var thisMicrochip = microchips & currentFloor;
        var thisGenerator = generators & currentFloor;

        if (thisMicrochip != 0 && thisGenerator != 0)
        {
          // Move this microchip and this generator together
          yield return (currentStateWithNewFloor ^ microchipMovingXor) ^ generatorMovingXor;
        }

        if (thisMicrochip != 0)
        {
          // Just move this microchip
          yield return currentStateWithNewFloor ^ microchipMovingXor;

          // Move this microchip and some other microchip
          var secondMicrochips = microchips;
          var secondMicrochipMovingXor = microchipMovingXor;
          
          for (int secondElement = element + 1; secondElement < 7; secondElement++)
          {
            secondMicrochips >>= 4;
            secondMicrochipMovingXor <<= 4;

            if ((secondMicrochips & currentFloor) != 0)
            {
              // Try moving this pair of microchips
              yield return (currentStateWithNewFloor ^ microchipMovingXor) ^ secondMicrochipMovingXor;
            }
          }
        }

        if (thisGenerator != 0)
        {
          // Just move this generator
          yield return currentStateWithNewFloor ^ generatorMovingXor;

          // Move this generator and some other generator
          var secondGenerators = generators;
          var secondGeneratorMovingXor = generatorMovingXor;

          for (int secondElement = element + 1; secondElement < 7; secondElement++)
          {
            secondGenerators >>= 4;
            secondGeneratorMovingXor <<= 4;

            if ((secondGenerators & currentFloor) != 0)
            {
              // Try moving this pair of microchips
              yield return (currentStateWithNewFloor ^ generatorMovingXor) ^ secondGeneratorMovingXor;
            }
          }
        }
      }
    }

    private static bool IsSafe(long state)
    {
      var microchips = state;
      var generators = microchips >> (7 * 4);

      var floorsWithGenerators = 0L;
      var floorsWithUnprotectedMicrochips = 0L;

      for (int element = 0; element < 7; element++)
      {
        microchips >>= 4;
        generators >>= 4;

        if ((microchips & generators & 0xF) == 0)
        {
          floorsWithUnprotectedMicrochips |= microchips;
        }

        floorsWithGenerators |= generators;
      }

      return (floorsWithGenerators & floorsWithUnprotectedMicrochips & 0xF) == 0;
    }
    
  }
}
