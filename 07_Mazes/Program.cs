using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TestingRoom;

namespace Mazes
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
            Application.Run(new TestRoom(CreateMazes()));
        }

        private static IEnumerable<MazeTestCase> CreateMazes()
        {
            yield return new MazeTestCase("empty1", EmptyMazeTask.MoveOut);
            yield return new MazeTestCase("empty2", EmptyMazeTask.MoveOut);
            yield return new MazeTestCase("empty3", EmptyMazeTask.MoveOut);
            yield return new MazeTestCase("empty4", EmptyMazeTask.MoveOut);
            yield return new MazeTestCase("empty5", EmptyMazeTask.MoveOut);
            yield return new MazeTestCase("snake1", SnakeMazeTask.MoveOut);
            yield return new MazeTestCase("snake2", SnakeMazeTask.MoveOut);
            yield return new MazeTestCase("snake3", SnakeMazeTask.MoveOut);
            /*
            yield return new MazeTestCase("pyramid1", PyramidMazeTask.MoveOut);
            yield return new MazeTestCase("pyramid2", PyramidMazeTask.MoveOut);
            yield return new MazeTestCase("pyramid3", PyramidMazeTask.MoveOut);
            yield return new MazeTestCase("pyramid4", PyramidMazeTask.MoveOut);
            */
            yield return new MazeTestCase("diagonal1", DiagonalMazeTask.MoveOut);
            yield return new MazeTestCase("diagonal2", DiagonalMazeTask.MoveOut);
            yield return new MazeTestCase("diagonal3", DiagonalMazeTask.MoveOut);
        }
    }

    internal class MazeTestCase : TestCase
    {
        private readonly int cellSize;
        private readonly Maze maze;
        private readonly Action<Robot, int, int> solve;
        private Robot robot;

        public MazeTestCase(string name, Action<Robot, int, int> solve)
            : base(name)
        {
            maze = new Maze(File.ReadAllLines(Path.Combine("mazes", name + ".txt")));
            cellSize = 200 / Math.Max(maze.Size.Width, maze.Size.Height);
            this.solve = solve;
        }

        protected override void InternalVisualize(TestCaseUI ui)
        {
            for (var x = 0; x < maze.Size.Width; x++)
            for (var y = 0; y < maze.Size.Height; y++)
                if (maze.IsWall(new Point(x, y)))
                    DrawWall(ui, x, y);
            var last = maze.Robot;
            foreach (var cur in robot.Path)
            {
                ui.Line(Conv(last.X), Conv(last.Y), Conv(cur.X), Conv(cur.Y), actualAnswerPen);
                last = cur;
            }

            ui.Circle(Conv(robot.X), Conv(robot.Y), cellSize / 3.0, actualAnswerPen);
            ui.Circle(Conv(maze.Exit.X), Conv(maze.Exit.Y), cellSize / 2.5, expectedAnswerPen);
        }

        private double Conv(int coord)
        {
            return coord * cellSize - 100 + cellSize / 2.0;
        }

        private void DrawWall(TestCaseUI ui, int x, int y)
        {
            var x1 = x * cellSize - 100;
            var y1 = y * cellSize - 100;
            var x2 = (x + 1) * cellSize - 101;
            var y2 = (y + 1) * cellSize - 101;
            ui.Rect(new Rectangle(x1, y1, cellSize, cellSize), neutralPen);
            ui.Line(x1, y1, x2, y2, neutralPen);
            ui.Line(x1, y2, x2, y1, neutralPen);
        }

        protected override bool InternalRun()
        {
            robot = new Robot(maze);
            solve(robot, maze.Size.Width, maze.Size.Height);
            return robot.Finished;
        }
    }
}