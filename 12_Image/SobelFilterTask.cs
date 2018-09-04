using System;

namespace Recognizer
{
    internal static class SobelFilterTask
    {
        public static double SobelValue(double[,] input, double[,] sx, int firstRow, int firstCol)
        {
            var kernelSize = sx.GetLength(0);
            var gx = 0.0;
            var gy = 0.0;
            for (int i = 0; i < kernelSize; ++i)
            {
                for (int j = 0; j < kernelSize; ++j)
                {
                    gx += input[firstRow + i, firstCol + j] * sx[i, j];
                    gy += input[firstRow + i, firstCol + j] * sx[j, i];
                }
            }
            return Math.Sqrt(gx * gx + gy * gy);
        }

        public static double[,] SobelFilter(double[,] input, double[,] sx)
        {
            var numRows = input.GetLength(0);
            var numCols = input.GetLength(1);
            var kernelBorder = sx.GetLength(0) / 2;
            var output = new double[numRows, numCols];
            for (var row = kernelBorder; row < numRows - kernelBorder; ++row)
            {
                for (var col = kernelBorder; col < numCols - kernelBorder; ++col)
                {
                    output[row, col] = SobelValue(input, sx, row - kernelBorder, col - kernelBorder);
                }
            }
            return output;
        }
    }
}