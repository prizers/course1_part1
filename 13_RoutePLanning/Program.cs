using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;

namespace RoutePlanning
{
    internal static class Program
    {
        private static void Main()
        {
            RunTests(1, PathFinderTask.FindBestCheckpointsOrder);
            RunTests(2, PathFinderTask.FindBestCheckpointsOrder);
            Console.WriteLine();
            Console.WriteLine("Большие тесты. Реализуйте отсечение перебора, чтобы они проходили быстро");
            RunTests(3, PathFinderTask.FindBestCheckpointsOrder);
        }

        private static void RunTests(int difficulty, Func<Point[], int[]> solve)
        {
            foreach (var testFile in Directory.GetFiles("tests", difficulty + "-*.txt"))
            {
                var lines = File.ReadAllLines(testFile);
                var answer = double.Parse(lines.First(), CultureInfo.InvariantCulture);
                var ps = lines.Skip(1).Select(line => line.Split().Select(int.Parse).ToArray())
                    .Select(cs => new Point(cs[0], cs[1])).ToArray();
                var sw = Stopwatch.StartNew();
                var order = solve(ps);
                sw.Stop();
                var len = ps.GetPathLength(order);
                var testName = Path.GetFileNameWithoutExtension(testFile);
                var isPassed = IsPassed(len, order, answer, ps.Length);
                LogTest(testName, ps.Length, len, sw.Elapsed, answer, isPassed);
                new PathForm(ps, order, isPassed, testName).ShowDialog();
            }
        }

        public static bool IsPassed(double len, int[] order, double expectedLen, int size)
        {
            return len < expectedLen + 1e-6 &&
                   order.Distinct().OrderBy(x => x).SequenceEqual(Enumerable.Range(0, size));
        }


        private static void LogTest(string testName, int size, double pathLen, TimeSpan timeSpent, double expectedLen,
            bool isPassed)
        {
            var verdict = isPassed ? "success" : "FAILED";
            Console.WriteLine("{2} test {0} of size {1}", testName, size, verdict);
            Console.WriteLine("	found path len: {0}. Expected len: {1}", pathLen, expectedLen);
            if (timeSpent.TotalSeconds > 1)
                Console.WriteLine("	time: {0}", timeSpent.TotalSeconds);
        }
    }
}