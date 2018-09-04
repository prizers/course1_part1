using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Manipulation
{
    public class Program
    {
        public static Form Form;

        [STAThread]
        private static void Main()
        {
            InitializeForm();
            Application.Run(Form);
        }

        public static void InitializeForm()
        {
            Form = new AntiFlickerForm(); //можете заменить AntiFlickerForm на Form, запустить и подвигать мышкой.
            Form.Text = "Manipulator";
            Form.ClientSize = new Size(800, 600);

            Form.Paint += (sender, ev) => Paint((Form) sender, ev);
            Form.KeyDown += (sender, ev) => VisualizerTask.KeyDown((Form) sender, ev);
            Form.MouseMove += (sender, ev) => VisualizerTask.MouseMove((Form) sender, ev);
            Form.MouseWheel += (sender, ev) => VisualizerTask.MouseWheel((Form) sender, ev);
            Form.Resize += (s, e) => Form.Invalidate();
        }

        private static void Paint(Form form, PaintEventArgs e)
        {
            var graphics = e.Graphics;
            graphics.FillRectangle(VisualizerTask.UnreachableAreaBrush, 0, 0, form.ClientSize.Width,
                form.ClientSize.Height);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            var shoulderPos = new PointF(form.ClientSize.Width / 2f, form.ClientSize.Height / 2f);
            VisualizerTask.DrawManipulator(graphics, shoulderPos);
        }
    }
}