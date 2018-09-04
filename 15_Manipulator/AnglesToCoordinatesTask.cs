using System;
using System.Drawing;
using NUnit.Framework;

namespace Manipulation
{
    public static class AnglesToCoordinatesTask
    {

        /// <summary>
        /// Вычисление точки конца радиус-вектора 
        /// длиной radius повернутого на угол angle
        /// </summary>
        private static SizeF CalculateRotatedPoint(double radius, double angle) =>
            new SizeF((float)(radius * Math.Cos(angle)),
                       (float)(radius * Math.Sin(angle)));

        /// <summary>
        /// По значению углов суставов возвращает массив координат суставов
        /// в порядке new []{elbow, wrist, palmEnd}
        /// </summary>
        public static PointF[] GetJointPositions(double shoulder, double elbow, double wrist)
        {
            var basePos = new PointF(0, 0);
            var currentAngle = shoulder;
            var elbowPos = basePos + CalculateRotatedPoint(Manipulator.UpperArm, currentAngle);
            currentAngle += elbow - Math.PI;
            var wristPos = elbowPos + CalculateRotatedPoint(Manipulator.Forearm, currentAngle);
            currentAngle += wrist - Math.PI;
            var palmEndPos = wristPos + CalculateRotatedPoint(Manipulator.Palm, currentAngle);
            return new PointF[]
            {
                elbowPos,
                wristPos,
                palmEndPos
            };
        }
    }

    [TestFixture]
    public class AnglesToCoordinatesTask_Tests
    {
        public static double Distance(PointF a, PointF b)
        {
            double dx = b.X - a.X;
            double dy = b.Y - a.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        // Доработайте эти тесты!
        // С помощью строчки TestCase можно добавлять новые тестовые данные.
        // Аргументы TestCase превратятся в аргументы метода.
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI,
            Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm)]
        public void TestGetJointPositions(
            double shoulder, double elbow, double wrist,
            double palmEndX, double palmEndY)
        {
            var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
            Assert.AreEqual(3, joints.Length);
            Assert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
            Assert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");
            Assert.AreEqual(Manipulator.UpperArm, Distance(joints[0], new PointF(0, 0)),
                1e-5, "upper arm length");
            Assert.AreEqual(Manipulator.Forearm, Distance(joints[1], joints[0]),
                1e-5, "forearm length");
            Assert.AreEqual(Manipulator.Palm, Distance(joints[2], joints[1]),
                1e-5, "palm length");
        }
    }
}