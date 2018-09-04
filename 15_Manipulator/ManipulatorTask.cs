using System;
using NUnit.Framework;

namespace Manipulation
{
    public static class ManipulatorTask
    {

        /// <summary>
        /// Возвращает массив углов (shoulder, elbow, wrist),
        /// необходимых для приведения эффектора манипулятора в точку x и y 
        /// с углом между последним суставом и горизонталью, равному angle (в радианах)
        /// См. чертеж manipulator.png!
        /// </summary>
        public static double[] MoveManipulatorTo(double x, double y, double angle)
        {
            double wristX = x + Manipulator.Palm * Math.Cos(Math.PI - angle);
            double wristY = y + Manipulator.Palm * Math.Sin(Math.PI - angle);
            double distanceFromShoulderToWrist = Math.Sqrt(wristX * wristX + wristY * wristY);
            double elbow = TriangleTask.GetABAngle(
                Manipulator.UpperArm,
                Manipulator.Forearm,
                distanceFromShoulderToWrist);
            if (Double.IsNaN(elbow))
            {  // что-то с треугольником... не срослось.
                return new[] { double.NaN, double.NaN, double.NaN };
            }
            double angleToWrist = Math.Atan2(wristY, wristX);
            if (Double.IsNaN(angleToWrist))
            { // случилась довольно странная ошибка. 
              // такое может быть только если wristX или wristY NaN или бесконечны
                return new[] { double.NaN, double.NaN, double.NaN };
            }
            double shoulder = TriangleTask.GetABAngle(
                Manipulator.UpperArm,
                distanceFromShoulderToWrist,
                Manipulator.Forearm) + angleToWrist;
            double wrist = -angle - shoulder - elbow;
            return new[] { shoulder, elbow, wrist };
        }
    }

    [TestFixture]
    public class ManipulatorTask_Tests
    {
        public const int NumTests = 1000;
        public const int TestSeed = 12345;
        public const double FullSize = Manipulator.UpperArm +
                                       Manipulator.Forearm +
                                       Manipulator.Palm;

        [Test]
        public void TestMoveManipulatorTo()
        {
            var rng = new Random(TestSeed);
            for (var testNo = 0; testNo < NumTests; ++testNo)
            {   // выбираем какую-то точку. не факт, что мы до неё сможем дотянуться.
                var x = rng.NextDouble() * 2 * FullSize - FullSize;
                var y = rng.NextDouble() * 2 * FullSize - FullSize;
                var a = rng.NextDouble() * 2 * Math.PI;
                var angles = ManipulatorTask.MoveManipulatorTo(x, y, a);
                Assert.AreEqual(3, angles.Length);
                if (!Double.IsNaN(angles[0]))
                { // кажется, дотянулись! проверим, в нужную ли точку
                    var joints = AnglesToCoordinatesTask.GetJointPositions(
                        angles[0], angles[1], angles[2]);
                    Assert.AreEqual(3, joints.Length);
                    Assert.AreEqual(joints[2].X, x, 0.001);
                    Assert.AreEqual(joints[2].Y, y, 0.001);
                }
            }
        }
    }
}