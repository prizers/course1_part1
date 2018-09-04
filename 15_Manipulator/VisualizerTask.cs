using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Manipulation
{
    public static class VisualizerTask
    {
        public const double DELTA = Math.PI / 180; // 1 градус
        public const float JOINT_RADIUS = 5; // радиус отрисовки Joint

        public static double X = 220;
        public static double Y = -100;
        public static double Alpha = 0.05;
        public static double Wrist = 2 * Math.PI / 3;
        public static double Elbow = 3 * Math.PI / 4;
        public static double Shoulder = Math.PI / 2;

        public static Brush UnreachableAreaBrush = new SolidBrush(Color.FromArgb(255, 255, 230, 230));
        public static Brush ReachableAreaBrush = new SolidBrush(Color.FromArgb(255, 230, 255, 230));
        public static Pen ManipulatorPen = new Pen(Color.Black, 3);
        public static Brush JointBrush = Brushes.Gray;

        public static void KeyDown(Form form, KeyEventArgs key)
        {
            switch (key.KeyCode)
            {
                case Keys.Q:
                    Shoulder += DELTA;
                    break;
                case Keys.A:
                    Shoulder -= DELTA;
                    break;
                case Keys.W:
                    Elbow += DELTA;
                    break;
                case Keys.S:
                    Elbow -= DELTA;
                    break;
                default:
                    return;
            }
            Wrist = -Alpha - Shoulder - Elbow;
            form.Invalidate(); // 
        }

        public static void MouseMove(Form form, MouseEventArgs e)
        {
            PointF mathPoint = ConvertWindowToMath(new PointF(e.X, e.Y), GetShoulderPos(form));
            X = mathPoint.X;
            Y = mathPoint.Y;
            UpdateManipulator();
            form.Invalidate();
        }

        public static void MouseWheel(Form form, MouseEventArgs e)
        {
            Alpha += DELTA * e.Delta;
            UpdateManipulator();
            form.Invalidate();
        }

        public static void UpdateManipulator()
        {
            double[] angles = ManipulatorTask.MoveManipulatorTo(X, Y, Alpha);
            foreach (var angle in angles)
            {
                if (Double.IsNaN(angle))
                {
                    return;
                }
            }
            Shoulder = angles[0];
            Elbow = angles[1];
            Wrist = angles[2];
        }

        public static void DrawManipulator(Graphics graphics, PointF shoulderPos)
        {
            var joints = AnglesToCoordinatesTask.GetJointPositions(Shoulder, Elbow, Wrist);

            graphics.DrawString($"X={X:0}, Y={Y:0}, Alpha={Alpha:0.00}",
                new Font(SystemFonts.DefaultFont.FontFamily, 12), Brushes.DarkRed, 10, 10);
            DrawReachableZone(graphics, ReachableAreaBrush, UnreachableAreaBrush, shoulderPos, joints);
            PointF[] points = new PointF[joints.Length + 1];
            points[0] = ConvertMathToWindow(new PointF(0, 0), shoulderPos);
            for (int i = 0; i < joints.Length; i++)
                points[i + 1] = ConvertMathToWindow(joints[i], shoulderPos);
            graphics.DrawLines(ManipulatorPen, points);
            for (int i = 0; i <= joints.Length; i++)
                graphics.FillEllipse(JointBrush,
                    points[i].X - JOINT_RADIUS, points[i].Y - JOINT_RADIUS,
                    2 * JOINT_RADIUS, 2 * JOINT_RADIUS);
        }

        private static void DrawReachableZone(Graphics graphics,
            Brush reachableBrush, Brush unreachableBrush,
            PointF shoulderPos, PointF[] joints)
        {
            var rmin = Math.Abs(Manipulator.UpperArm - Manipulator.Forearm);
            var rmax = Manipulator.UpperArm + Manipulator.Forearm;
            var mathCenter = new PointF(joints[2].X - joints[1].X, joints[2].Y - joints[1].Y);
            var windowCenter = ConvertMathToWindow(mathCenter, shoulderPos);
            graphics.FillEllipse(reachableBrush,
                windowCenter.X - rmax, windowCenter.Y - rmax,
                2 * rmax, 2 * rmax);
            graphics.FillEllipse(unreachableBrush,
                windowCenter.X - rmin, windowCenter.Y - rmin,
                2 * rmin, 2 * rmin);
        }

        public static PointF GetShoulderPos(Form form)
        {
            return new PointF(form.ClientSize.Width / 2f, form.ClientSize.Height / 2f);
        }

        public static PointF ConvertMathToWindow(PointF mathPoint, PointF shoulderPos)
        {
            return new PointF(mathPoint.X + shoulderPos.X, shoulderPos.Y - mathPoint.Y);
        }

        public static PointF ConvertWindowToMath(PointF windowPoint, PointF shoulderPos)
        {
            return new PointF(windowPoint.X - shoulderPos.X, shoulderPos.Y - windowPoint.Y);
        }

    }
}
