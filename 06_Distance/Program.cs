using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using TestingRoom;

namespace DistanceTask
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TestRoom(CreateTestCases()));
        }

        private static IEnumerable<TestCase> CreateTestCases()
        {
            var allCases = OutsideSegment(30).Concat(OnSegment(30)).ToList();

            var angles = new[] {0, Math.PI / 3, Math.PI / 6};


            var allCombinations =
                from i in Enumerable.Range(0, 3)
                from a in angles
                from shift in new[] {new SizeF(0, 0), new SizeF(50, 20), new SizeF(-20, 10)}
                from t in allCases
                select t.Shift(shift).Rotate(a + i * Math.PI / 2);
            return Dot(30).Concat(allCombinations);
        }

        private static IEnumerable<DistanceTestRoomTestCase> Dot(int d)
        {
            yield return new DistanceTestRoomTestCase(new Point(d, d), new Point(d, d), new Point(0, 0),
                d * Math.Sqrt(2));
            yield return new DistanceTestRoomTestCase(new Point(d, d), new Point(d, d), new Point(d, d), 0);
        }

        private static IEnumerable<DistanceTestRoomTestCase> OnSegment(int d)
        {
            yield return new DistanceTestRoomTestCase(new Point(-d, 0), new Point(d, 0), new Point(-d, 0), 0);
            yield return new DistanceTestRoomTestCase(new Point(-d, 0), new Point(d, 0), new Point(-d / 2, 0), 0);
            yield return new DistanceTestRoomTestCase(new Point(-d, 0), new Point(d, 0), new Point(d / 2, 0), 0);
            yield return new DistanceTestRoomTestCase(new Point(-d, 0), new Point(d, 0), new Point(d, 0), 0);
        }

        private static IEnumerable<DistanceTestRoomTestCase> OutsideSegment(int d)
        {
            yield return new DistanceTestRoomTestCase(new Point(-d, 0), new Point(d, 0), new Point(-d, d), d);
            yield return new DistanceTestRoomTestCase(new Point(-d, 0), new Point(d, 0), new Point(0, d), d);
            yield return new DistanceTestRoomTestCase(new Point(-d, 0), new Point(d, 0), new Point(d, d), d);
            yield return new DistanceTestRoomTestCase(new Point(-d, 0), new Point(d, 0), new Point(2 * d, d),
                d * Math.Sqrt(2));
            yield return new DistanceTestRoomTestCase(new Point(-d, 0), new Point(d, 0), new Point(2 * d, 0), d);
            yield return new DistanceTestRoomTestCase(new Point(-d, 0), new Point(d, 0), new Point(d, -d), d);
            yield return new DistanceTestRoomTestCase(new Point(-d, 0), new Point(d, 0), new Point(0, -d), d);
            yield return new DistanceTestRoomTestCase(new Point(-d, 0), new Point(d, 0), new Point(-d, -d), d);
            yield return new DistanceTestRoomTestCase(new Point(-d, 0), new Point(d, 0), new Point(-2 * d, 0), d);
            yield return new DistanceTestRoomTestCase(new Point(-d, 0), new Point(d, 0), new Point(-2 * d, d),
                d * Math.Sqrt(2));
        }
    }

    internal class DistanceTestRoomTestCase : TestCase
    {
        private readonly PointF a, b, x;
        private readonly double distance;
        private double answer;

        public DistanceTestRoomTestCase(PointF a, PointF b, PointF x, double distance) : base(
            string.Format("d(x, AB) = {0:0.00}", distance))
        {
            this.a = a;
            this.b = b;
            this.x = x;
            this.distance = distance;
        }

        protected override void InternalVisualize(TestCaseUI ui)
        {
            ui.Line(-100, 0, 100, 0, neutralThinPen);
            ui.Line(0, -100, 0, 100, neutralThinPen);

            ui.Line(a.X, a.Y, b.X, b.Y, neutralPen);
            ui.Circle(a.X, a.Y, 1, neutralPen);
            ui.Circle(b.X, b.Y, 1, neutralPen);

            ui.Circle(x.X, x.Y, 3, neutralPen);
            ui.Circle(x.X, x.Y, answer,
                new Pen(actualAnswerPen.Color, 1) {DashStyle = DashStyle.Custom, DashPattern = new float[] {4, 4}});
            ui.Log("A = " + a);
            ui.Log("B = " + b);
            ui.Log("X = " + x);
            ui.Log("Expected distance " + distance);
            ui.Log("Calculated distance: {0}", answer);
        }

        protected override bool InternalRun()
        {
            answer = DistanceTask.GetDistanceToSegment(a.X, a.Y, b.X, b.Y, x.X, x.Y);
            return Math.Abs(answer - distance) < 1e-3;
        }

        public DistanceTestRoomTestCase Rotate(double angle)
        {
            return new DistanceTestRoomTestCase(Rotate(a, angle), Rotate(b, angle), Rotate(x, angle), distance);
        }

        private static PointF Rotate(PointF p, double a)
        {
            return new PointF((float) (Math.Cos(a) * p.X - Math.Sin(a) * p.Y),
                (float) (Math.Sin(a) * p.X + Math.Cos(a) * p.Y));
        }

        public DistanceTestRoomTestCase Shift(SizeF shift)
        {
            return new DistanceTestRoomTestCase(a + shift, b + shift, x + shift, distance);
        }
    }
}