namespace Mazes
{
    public static class SnakeMazeTask
    {
        static void DoSomeSteps(Robot robot, Direction direction, int numSteps)
        {
            for (int i = 0; i < numSteps; ++i) robot.MoveTo(direction);
        }

        static readonly Direction[] ZigZag = { Direction.Right, Direction.Left };

        public static void MoveOut(Robot robot, int width, int height)
        {
            var distance = width - 3;
            int director = 0;
            while (true)
            {
                DoSomeSteps(robot, ZigZag[director], distance);
                if (robot.Finished) break;
                DoSomeSteps(robot, Direction.Down, 2);
                director = (director + 1) % 2;
            }
        }
    }
}