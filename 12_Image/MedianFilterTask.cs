using System;
using System.Linq;

namespace Recognizer
{
    internal static class MedianFilterTask
    {
        /* 
		 * Для борьбы с пиксельным шумом, подобным тому, что на изображении,
		 * обычно применяют медианный фильтр, в котором цвет каждого пикселя, 
		 * заменяется на медиану всех цветов в некоторой окрестности пикселя.
		 * https://en.wikipedia.org/wiki/Median_filter
		 * 
		 * Используйте окно размером 3х3 для не граничных пикселей,
		 * Окно размером 2х2 для угловых и 3х2 или 2х3 для граничных.
		 */

        public static double MedianValue(double[,] matrix,
                                         int startRow, int startCol,
                                         int numRows, int numCols)
        {
            int numValues = numCols * numRows;
            var values = new double[numValues];
            for (var i = 0; i < numValues; ++i)
            {
                var row = startRow + i % numRows;
                var col = startCol + i / numRows;
                values[i] = matrix[row, col];
            }
            Array.Sort(values);
            var ix = numValues / 2;
            return 0 == numValues % 2 ? (values[ix - 1] + values[ix]) / 2 : values[ix];
        }

        public static double[,] MedianFilter(double[,] input)
        {
            var numRows = input.GetLength(0);
            var numCols = input.GetLength(1);
            var output = new double[numRows, numCols];

            for (var row = 0; row < numRows; ++row)
            {
                var firstFrameRow = Math.Max(row - 1, 0);
                var lastFrameRow = Math.Min(row + 1, numRows - 1);
                var numFrameRows = lastFrameRow - firstFrameRow + 1;
                for (var col = 0; col < numCols; ++col)
                {
                    var firstFrameCol = Math.Max(col - 1, 0);
                    var lastFrameCol = Math.Min(col + 1, numCols - 1);
                    var numFrameCols = lastFrameCol - firstFrameCol + 1;
                    output[row, col] = MedianValue(input,
                                                   firstFrameRow, firstFrameCol,
                                                   numFrameRows, numFrameCols);
                }
            }
            return output;
        }
    }
}