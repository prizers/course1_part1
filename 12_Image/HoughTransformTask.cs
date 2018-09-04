namespace Recognizer
{
    internal static class HoughTransformTask
    {
        public static Line[] HoughAlgorithm(double[,] original)
        {
            var width = original.GetLength(0);

            var height = original.GetLength(1);
            return new[]
            {
                new Line(0, 0, width, height),
                new Line(0, height, width, 0)
            };
        }
    }
}