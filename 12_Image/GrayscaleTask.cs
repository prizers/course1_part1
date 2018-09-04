namespace Recognizer
{
    public static class GrayscaleTask
    {
        /* 
		 * Переведите изображение в серую гамму.
		 * 
		 * original[x, y] - массив пикселей с координатами x, y. 
		 * Каждый канал R,G,B лежит в диапазоне от 0 до 255.
		 * 
		 * Получившийся массив должен иметь те же размеры, 
		 * grayscale[x, y] - яркость пикселя (x,y) в диапазоне от 0.0 до 1.0
		 *
		 * Используйте формулу:
		 * Яркость = (0.299*R + 0.587*G + 0.114*B) / 255
		 * 
		 * Почему формула именно такая — читайте в википедии 
		 * http://ru.wikipedia.org/wiki/Оттенки_серого
		 */

        static double GrayValue(Pixel px) =>
            (px.R * 0.299 + px.G * 0.587 + px.B * 0.114) / 255;

        public static double[,] ToGrayscale(Pixel[,] original)
        {
            var rows = original.GetLength(0);
            var cols = original.GetLength(1);
            var grayscale = new double[rows, cols];
            for (var row = 0; row < rows; ++row)
            {
                for (var col = 0; col < cols; ++col)
                {
                    grayscale[row, col] = GrayValue(original[row, col]);
                }
            }
            return grayscale;
        }
    }
}