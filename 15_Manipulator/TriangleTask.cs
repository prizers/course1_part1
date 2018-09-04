using System;
using NUnit.Framework;

namespace Manipulation
{
    public class TriangleTask
    {
        /// <summary>
        /// Возвращает угол (в радианах) между сторонами a и b в треугольнике со сторонами a, b, c 
        /// c^2 = a^2 + b^2 - 2*a*b*cos(gamma) <=> gamma = acos((a^2 + b^2 - c^2) / (2*a*b))
        /// </summary>
        public static double GetABAngle(double a, double b, double c)
        {
            if (a < 0 || b < 0 || c < 0) return Double.NaN;
            if (b + c < a || a + c < b || a + b < c) return Double.NaN;
            double divisor = 2 * a * b;
            if (divisor == 0) return Double.NaN;
            return Math.Acos((a * a + b * b - c * c) / divisor);
        }
    }

    [TestFixture]
    public class TriangleTask_Tests
    {
        [TestCase(3, 4, 5, Math.PI / 2)]
        [TestCase(4, 5, 3, 0.6435)]
        [TestCase(3, 5, 4, 0.9273)]
        [TestCase(1, 1, 1, Math.PI / 3)]
        [TestCase(0, 0, 0, Double.NaN)] // точка
        [TestCase(1, 3, 5, Double.NaN)] // не треугольник, вариант 1
        [TestCase(3, 5, 1, Double.NaN)] // не треугольник, вариант 2
        [TestCase(5, 1, 3, Double.NaN)] // не треугольник, вариант 3

        [TestCase(1, 2, 3, Math.PI)] // вырожденный треугольник, развёрнутый угол
        [TestCase(2, 3, 1, 0)] // вырожденный треугольник, нулевой угол 1
        [TestCase(3, 1, 2, 0)] // вырожденный треугольник, нулевой угол 2
        // добавьте ещё тестовых случаев!
        public void TestGetABAngle(double a, double b, double c, double expectedAngle)
        {
            double angle = TriangleTask.GetABAngle(a, b, c);
            if (Double.IsNaN(expectedAngle))
                Assert.IsNaN(angle);
            Assert.AreEqual(expectedAngle, angle, 0.0001);
        }
    }
}
