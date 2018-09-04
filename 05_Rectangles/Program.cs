using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TestingRoom;

namespace Rectangles
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
            yield return new RectanglesTestRoomTestCase(new Rectangle(0, 0, 20, 20), new Rectangle(-20, -30, 20, 20),
                false, 0, -1);
            yield return new RectanglesTestRoomTestCase(new Rectangle(-90, -20, 180, 40),
                new Rectangle(-20, -90, 40, 180), true, 1600, -1);
            yield return new RectanglesTestRoomTestCase(new Rectangle(-90, -20, 180, 40), new Rectangle(-20, 0, 40, 90),
                true, 800, -1);
            yield return new RectanglesTestRoomTestCase(new Rectangle(-90, -20, 180, 40), new Rectangle(-20, 0, 40, 90),
                true, 800, -1);
            yield return new RectanglesTestRoomTestCase(new Rectangle(-90, -20, 180, 40), new Rectangle(-20, 0, 40, 90),
                true, 800, -1);
            yield return new RectanglesTestRoomTestCase(new Rectangle(-90, -20, 180, 40), new Rectangle(-20, 0, 40, 90),
                true, 800, -1);
            yield return new RectanglesTestRoomTestCase(new Rectangle(-90, -90, 100, 100),
                new Rectangle(-10, -10, 100, 100), true, 400, -1);
            yield return new RectanglesTestRoomTestCase(new Rectangle(-50, -50, 50, 50), new Rectangle(0, 0, 50, 50),
                true, 0, -1);
            yield return new RectanglesTestRoomTestCase(new Rectangle(-50, 0, 50, 50), new Rectangle(0, 0, 50, 50),
                true, 0, -1);
            yield return new RectanglesTestRoomTestCase(new Rectangle(-50, 0, 60, 60), new Rectangle(0, 0, 50, 50),
                true, 500, -1);
            yield return new RectanglesTestRoomTestCase(new Rectangle(20, 2, 40, 4), new Rectangle(10, 1, 0, 0), false,
                0, -1);
            yield return new RectanglesTestRoomTestCase(new Rectangle(20, 2, 40, 4), new Rectangle(10, 1, 20, 5), true,
                40, -1);
            yield return new RectanglesTestRoomTestCase(new Rectangle(20, 2, 40, 4), new Rectangle(10, 1, 50, 5), true,
                160, 0);
            yield return new RectanglesTestRoomTestCase(new Rectangle(20, 2, 40, 4), new Rectangle(10, 1, 0, 1), false,
                0, -1);
            for (var x = -1; x <= 1; x++)
            for (var y = -1; y <= 1; y++)
                if (10 * x + y != 0)
                    yield return new RectanglesTestRoomTestCase(new Rectangle(0, 0, 10, 10),
                        new Rectangle(20 * y, 20 * x, 10, 10), false, 0, -1);
            for (var x = -1; x <= 1; x++)
            for (var y = -1; y <= 1; y++)
                if (10 * x + y != 0)
                    yield return new RectanglesTestRoomTestCase(new Rectangle(0, 0, 30, 30),
                        new Rectangle(20 * y, 20 * x, 30, 30), true, x * y == 0 ? 300 : 100, -1);
            for (var x = -1; x <= 1; x++)
            for (var y = -1; y <= 1; y++)
                if (10 * x + y != 0)
                    yield return new RectanglesTestRoomTestCase(new Rectangle(-40, -40, 110, 110),
                        new Rectangle(30 * y, 30 * x, 30, 30), true, 900, 1);
            for (var x = -1; x <= 1; x++)
            for (var y = -1; y <= 1; y++)
                if (10 * x + y != 0)
                    yield return new RectanglesTestRoomTestCase(new Rectangle(30 * y, 30 * x, 30, 30),
                        new Rectangle(-40, -40, 110, 110), true, 900, 0);
            for (var x = -1; x <= 1; x++)
            for (var y = -1; y <= 1; y++)
                if (10 * x + y != 0)
                {
                    yield return new RectanglesTestRoomTestCase(new Rectangle(0, 0, 20, 20),
                        new Rectangle(20 * x, 20 * y, 20, 20), true, 0, -1);
                    yield return new RectanglesTestRoomTestCase(new Rectangle(20 * x, 20 * y, 20, 20),
                        new Rectangle(0, 0, 20, 20), true, 0, -1);
                }
        }
    }

    public class RectanglesTestRoomTestCase : TestCase
    {
        private readonly int indexOfInnerRectangle;
        private readonly bool intersected;
        private readonly int intersectionSquare;
        private readonly Rectangle r1;
        private readonly Rectangle r2;
        private int indexOfInnerRectAnswer;
        private bool intersectedAnswer;
        private int intersectionSquareAnswer;

        public RectanglesTestRoomTestCase(Rectangle r1, Rectangle r2, bool intersected, int intersectionSquare,
            int indexOfInnerRectangle) : base("Rectangles")
        {
            this.r1 = r1;
            this.r2 = r2;
            this.intersected = intersected;
            this.intersectionSquare = intersectionSquare;
            this.indexOfInnerRectangle = indexOfInnerRectangle;
        }

        protected override void InternalVisualize(TestCaseUI ui)
        {
            ui.Rect(new System.Drawing.Rectangle(r1.Left, r1.Top, r1.Width, r1.Height), neutralPen);
            ui.Rect(new System.Drawing.Rectangle(r2.Left, r2.Top, r2.Width, r2.Height), neutralPen);
            ui.Log("r1: {0}", r1);
            ui.Log("r2: {0}", r2);
            ui.Log("Solution: ");
            ui.Log("  intersected: {0} {1}", intersectedAnswer, intersected != intersectedAnswer ? "wrong!" : "");
            ui.Log("  intersection square: {0} {1}", intersectionSquareAnswer,
                intersectionSquare != intersectionSquareAnswer ? "wrong!" : "");
            ui.Log("  index of inner rectangle: {0} {1}", indexOfInnerRectAnswer,
                indexOfInnerRectangle != indexOfInnerRectAnswer ? "wrong!" : "");
        }

        protected override bool InternalRun()
        {
            intersectedAnswer = RectanglesTask.AreIntersected(r1, r2);
            intersectionSquareAnswer = RectanglesTask.IntersectionSquare(r1, r2);
            indexOfInnerRectAnswer = RectanglesTask.IndexOfInnerRectangle(r1, r2);
            return intersected == intersectedAnswer && intersectionSquare == intersectionSquareAnswer &&
                   indexOfInnerRectangle == indexOfInnerRectAnswer;
        }
    }
}