using System;

namespace Recognizer
{
    public static class ThresholdFilterTask
    {
        public static double FindThresholdValue(double[,] input, double whitePixelsFraction)
        {
            var numRows = input.GetLength(0);
            var numCols = input.GetLength(1);
            var numValues = numRows * numCols;
            var numWhites = (int)Math.Floor(numValues * whitePixelsFraction);
            if (numWhites <= 0) return double.MaxValue; // не желаем ни единого пикселя в выходе
            if (numValues <= numWhites) return double.MinValue; // хотим чтобы были все пиксели 
            var values = new double[numValues];
            for (var i = 0; i < numValues; ++i)
            {
                values[i] = input[i / numCols, i % numCols];
            }
            Array.Sort(values);
            return values[numValues - numWhites];
        }

        public static double[,] Binarize(double[,] input, double threshold)
        {
            var numRows = input.GetLength(0);
            var numCols = input.GetLength(1);
            var output = new double[numRows, numCols];
            for (var row = 0; row < numRows; ++row)
            {
                for (var col = 0; col < numCols; ++col)
                {
                    output[row, col] = threshold <= input[row, col] ? 1.0 : 0.0;
                }
            }
            return output;
        }

        public static double[,] ThresholdFilter(double[,] input, double whitePixelsFraction) =>
            Binarize(input, FindThresholdValue(input, whitePixelsFraction));
    }
}
