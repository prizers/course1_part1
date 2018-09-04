using System;

namespace Mazes
{
    public static class DiagonalMazeTask
    {
        static void DoSomeSteps(Robot robot, Direction direction, int numSteps)
        {
            for (int i = 0; i < numSteps; ++i) robot.MoveTo(direction);
        }

        static void DiagonalRun(Robot robot, Direction direction, int distance)
        {
            while (true)
            {
                DoSomeSteps(robot, direction, distance);
                if (robot.Finished) break;
                if (direction == Direction.Right) robot.MoveTo(Direction.Down);
                else robot.MoveTo(Direction.Right);
            }
        }

        public static void MoveOut(Robot robot, int width, int height)
        {
            DiagonalRun(robot,
                width < height ? Direction.Down : Direction.Right,
                (Math.Max(height, width) - 2) / (Math.Min(height, width) - 2));
        }
    }
}