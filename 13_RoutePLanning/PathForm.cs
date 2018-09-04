using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RoutePlanning
{
    internal class PathForm : Form
    {
        private readonly Point[] checkpoints;
        private readonly int[] order;
        private readonly bool passed;

        public PathForm(Point[] checkpoints, int[] order, bool passed, string test)
        {
            this.checkpoints = checkpoints;
            this.order = order;
            this.passed = passed;
            DoubleBuffered = true;
            Text = test;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.FillRectangle(passed ? Brushes.LightGreen : Brushes.LightCoral, ClientRectangle);
            var minX = checkpoints.Min(p => p.X);
            var maxX = checkpoints.Max(p => p.X);
            var minY = checkpoints.Min(p => p.Y);
            var maxY = checkpoints.Max(p => p.Y);
            const int margin = 10;
            var scaleX = (ClientSize.Width - 2f * margin) / (maxX - minX + 1f);
            var scaleY = (ClientSize.Height - 2f * margin) / (maxY - minY + 1f);
            var scale = Math.Min(scaleX, scaleY);

            var pts = order.Select(i => checkpoints[i])
                .Select(p => new PointF(margin + (p.X - minX) * scale, margin + (p.Y - minY) * scale)).ToArray();

            g.DrawLines(new Pen(Brushes.DarkRed, 2), pts);

            foreach (var p in pts)
                g.FillEllipse(Brushes.DarkRed, p.X - 2, p.Y - 2, 4, 4);
            g.FillEllipse(Brushes.Red, pts[0].X - 4, pts[0].Y - 4, 8, 8);


            //Тут можно что-нибудь делать с checkopoints и order
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            Close();
        }
    }
}