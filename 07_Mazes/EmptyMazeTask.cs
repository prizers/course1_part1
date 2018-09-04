namespace Mazes
{
    public static class EmptyMazeTask
    {
        static void DoSomeSteps(Robot robot, Direction direction, int numSteps)
        {
            for (int i = 0; i < numSteps; ++i) robot.MoveTo(direction);
        }

        public static void MoveOut(Robot robot, int width, int height)
        {
            DoSomeSteps(robot, Direction.Right, width - 3);
            DoSomeSteps(robot, Direction.Down, height - 3);
        }
    }
}