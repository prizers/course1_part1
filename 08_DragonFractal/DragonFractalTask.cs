// В этом пространстве имен содержатся средства для работы с изображениями.
// Чтобы оно стало доступно, в проект был подключен Reference на сборку System.Drawing.dll
using System;
using System.Drawing;

namespace Fractals
{
    internal static class DragonFractalTask
    {
        /*
 		Начните с точки (1, 0)
		Создайте генератор рандомных чисел с сидом seed
		На каждой итерации:
		1. Выберите случайно одно из следующих преобразований и примените его к текущей точке:
			Преобразование 1. (поворот на 45° и сжатие в sqrt(2) раз):
				x' = (x · cos(45°) - y · sin(45°)) / sqrt(2)
				y' = (x · sin(45°) + y · cos(45°)) / sqrt(2)
			Преобразование 2. (поворот на 135°, сжатие в sqrt(2) раз, сдвиг по X на единицу):
				x' = (x · cos(135°) - y · sin(135°)) / sqrt(2) + 1
				y' = (x · sin(135°) + y · cos(135°)) / sqrt(2)

			2. Нарисуйте текущую точку методом pixels.SetPixel(x, y)
		*/
        const double Angle1 = 45.0 * Math.PI / 180.0;
        const double Angle2 = 135.0 * Math.PI / 180.0;
        // тут тоже логичнее было бы пользовать константы, 
        // но их вычислить на этапе компиляции затруднительно,
        // посему вот так...
        static double sin1 = Math.Sin(Angle1);
        static double cos1 = Math.Cos(Angle1);
        static double sin2 = Math.Sin(Angle2);
        static double cos2 = Math.Cos(Angle2);
        static double sqrt2 = Math.Sqrt(2.0);

        public static void DrawDragonFractal(Pixels pixels, int iterationsCount, int seed)
        {

            var rng = new Random(seed);
            double x = 1.0, y = 0.0;
            for (int i = 0; i < iterationsCount; ++i)
            {
                double x1, y1;
                if (0 == rng.Next(2))
                {
                    x1 = (x * cos1 - y * sin1) / sqrt2;
                    y1 = (x * sin1 + y * cos1) / sqrt2;
                }
                else
                {
                    x1 = (x * cos2 - y * sin2) / sqrt2 + 1; // ???
                    y1 = (x * sin2 + y * cos2) / sqrt2;
                }
                x = x1;
                y = y1;
                pixels.SetPixel(x, y);
            }
        }
    }
}