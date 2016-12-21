using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode2016
{
  internal class Day21
  {
    private static void Main()
    {
      var timer = new Stopwatch();
      timer.Start();

      var regex = string.Join("|", Transforms.Select((transform, index) => $"(?<transform{index}>({transform.Item1}))"));
      var matcher = new Regex(regex);

      var completeTransform = Input.SplitLines().Select(line => Tuple.Create(line, matcher.Match(line)))
        .Select(
          tuple =>
            Tuple.Create(tuple.Item1, tuple.Item2,
              Transforms[
                int.Parse(
                  matcher.GetGroupNames()
                    .Single(group => group.StartsWith("transform") && tuple.Item2.Groups[group].Success)
                    .Substring("transform".Length))]))
        .Select(tuple => (Func<string, string>) (input => tuple.Item3.Item2(tuple.Item2, input)));

      var encodedPassword = completeTransform.Aggregate(Password, (acc, func) => func(acc));

      Console.WriteLine($"Encoded password: {encodedPassword}");
      Console.WriteLine($"Total time: {timer.ElapsedMilliseconds}ms");
      Console.ReadKey();
    }

    private static readonly Tuple<string, Func<Match, string, string>>[] Transforms = { 
      CommandWithPositionArgs(
        @"swap position (?<position1>\d+) with position (?<position2>\d+)",
        (pos1, pos2, input) => Swap(input, pos1, pos2)),
      CommandWithLetterArgs(
        @"swap letter (?<letter1>\w+) with letter (?<letter2>\w+)",
        (pos1, pos2, input) => Swap(input, input.IndexOf(pos1), input.IndexOf(pos2))),
      CommandWithIntArg(
        @"rotate left (?<int>\d+) steps?",
        (steps, input) => RotateLeft(input, steps)),
      CommandWithIntArg(
        @"rotate right (?<int>\d+) steps?",
        (steps, input) => RotateRight(input, steps)),
      CommandWithLetterArg(
        @"rotate based on position of letter (?<letter>\w+)",
        (letter, input) => RotateRight(input, input.IndexOf(letter) + (input.IndexOf(letter) >= 4 ? 2 : 1))),
      CommandWithPositionArgs(
        @"reverse positions (?<position1>\d+) through (?<position2>\d+)",
        (pos1, pos2, input) => ReverseRange(input, pos1, pos2)),
      CommandWithPositionArgs(
        @"move position (?<position1>\d+) to position (?<position2>\d+)",
        (pos1, pos2, input) => Move(input, pos1, pos2))
    };

    private static Tuple<string, Func<Match, string, string>> CommandWithPositionArgs(
      string regex,
      Func<int, int, string, string> func)
    {
      return Tuple.Create(regex, (Func<Match, string, string>)((match, input) =>
         func(int.Parse(match.Groups["position1"].Value), int.Parse(match.Groups["position2"].Value), input)));
    }

    private static Tuple<string, Func<Match, string, string>> CommandWithLetterArgs(
      string regex,
      Func<char, char, string, string> func)
    {
      return Tuple.Create(regex, (Func<Match, string, string>)((match, input) =>
         func(match.Groups["letter1"].Value.Single(), match.Groups["letter2"].Value.Single(), input)));
    }

    private static Tuple<string, Func<Match, string, string>> CommandWithIntArg(
      string regex,
      Func<int, string, string> func)
    {
      return Tuple.Create(regex, (Func<Match, string, string>)((match, input) =>
         func(int.Parse(match.Groups["int"].Value), input)));
    }

    private static Tuple<string, Func<Match, string, string>> CommandWithLetterArg(
      string regex,
      Func<char, string, string> func)
    {
      return Tuple.Create(regex, (Func<Match, string, string>)((match, input) =>
         func(match.Groups["letter"].Value.Single(), input)));
    }

    private static string Swap(string input, int x, int y)
    {
      var first = Math.Min(x, y);
      var second = Math.Max(x, y);

      return (first == 0 ? "" : input.Substring(0, first)) +
             input[second] +
             input.Substring(first + 1, second - first - 1) +
             input[first] +
             (second == input.Length - 1 ? "" : input.Substring(second + 1));
    }

    private static string Move(string input, int x, int y)
    {
      var character = input[x];
      var working = input.Remove(x, 1);
      return (y == 0 ? "" : working.Substring(0, y)) + character + working.Substring(y);
    }

    private static string RotateLeft(string input, int steps)
    {
      steps = steps % input.Length;
      return input.Substring(steps) + input.Substring(0, steps);
    }

    private static string RotateRight(string input, int steps)
    {
      steps = steps%input.Length;
      return input.Substring(input.Length - steps) + input.Substring(0, input.Length - steps);
    }

    private static string ReverseRange(string input, int from, int to)
    {
      return (from == 0 ? "" : input.Substring(0, from)) +
             string.Join("", input.Substring(from, to - from + 1).Reverse()) +
             (to == input.Length - 1 ? "" : input.Substring(to + 1));
    }

    private const string Password = "abcdefgh";

    private const string Input = @"move position 0 to position 3
rotate right 0 steps
rotate right 1 step
move position 1 to position 5
swap letter h with letter b
reverse positions 1 through 3
swap letter a with letter g
swap letter b with letter h
rotate based on position of letter c
swap letter d with letter c
rotate based on position of letter c
swap position 6 with position 5
rotate right 7 steps
swap letter b with letter h
move position 4 to position 3
swap position 1 with position 0
swap position 7 with position 5
move position 7 to position 1
swap letter c with letter a
move position 7 to position 5
rotate right 4 steps
swap position 0 with position 5
move position 3 to position 1
swap letter c with letter h
rotate based on position of letter d
reverse positions 0 through 2
rotate based on position of letter g
move position 6 to position 7
move position 2 to position 5
swap position 1 with position 0
swap letter f with letter c
rotate right 1 step
reverse positions 2 through 4
rotate left 1 step
rotate based on position of letter h
rotate right 1 step
rotate right 5 steps
swap position 6 with position 3
move position 0 to position 5
swap letter g with letter f
reverse positions 2 through 7
reverse positions 4 through 6
swap position 4 with position 1
move position 2 to position 1
move position 3 to position 1
swap letter b with letter a
rotate based on position of letter b
reverse positions 3 through 5
move position 0 to position 2
rotate based on position of letter b
reverse positions 4 through 5
rotate based on position of letter g
reverse positions 0 through 5
swap letter h with letter c
reverse positions 2 through 5
swap position 7 with position 5
swap letter g with letter d
swap letter d with letter e
move position 1 to position 2
move position 3 to position 2
swap letter d with letter g
swap position 3 with position 7
swap letter b with letter f
rotate right 3 steps
move position 5 to position 3
move position 1 to position 2
rotate based on position of letter b
rotate based on position of letter c
reverse positions 2 through 3
move position 2 to position 3
rotate right 1 step
move position 7 to position 0
rotate right 3 steps
move position 6 to position 3
rotate based on position of letter e
swap letter c with letter b
swap letter f with letter d
swap position 2 with position 5
swap letter f with letter g
rotate based on position of letter a
reverse positions 3 through 4
rotate left 7 steps
rotate left 6 steps
swap letter g with letter b
reverse positions 3 through 6
rotate right 6 steps
rotate based on position of letter c
rotate based on position of letter b
rotate left 1 step
reverse positions 3 through 7
swap letter f with letter g
swap position 4 with position 1
rotate based on position of letter d
move position 0 to position 4
swap position 7 with position 6
rotate right 6 steps
rotate based on position of letter e
move position 7 to position 3
rotate right 3 steps
swap position 1 with position 2";
  }
}
