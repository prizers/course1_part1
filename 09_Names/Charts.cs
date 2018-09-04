using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ZedGraph;

namespace Names
{
    internal static class Charts
    {
        public static void ShowHistogram(HistogramData stats)
        {
            // Графики строятся сторонней библиотекой ZedGraph. Документацию можно найти тут http://zedgraph.sourceforge.net/samples.html
            // Не бойтесь экспериментировать с кодом самостоятельно!

            var chart = new ZedGraphControl
            {
                Dock = DockStyle.Fill
            };
            chart.GraphPane.Title.Text = stats.Title;
            chart.GraphPane.YAxis.Title.Text = "Y";
            chart.GraphPane.AddBar("", Enumerable.Range(0, stats.YValues.Length).Select(i => (double) i).ToArray(),
                stats.YValues, Color.Blue);
            chart.GraphPane.YAxis.Scale.MaxAuto = true;
            chart.GraphPane.YAxis.Scale.MinAuto = true;
            chart.GraphPane.XAxis.Type = AxisType.Text;
            chart.GraphPane.XAxis.Scale.TextLabels = stats.XLabels;

            chart.AxisChange();
            // Form — это привычное нам окно программы.
            // Это одна из главных частей подсистемы под названием Windows Forms http://msdn.microsoft.com/ru-ru/library/ms229601.aspx
            var form = new Form
            {
                Text = stats.Title,
                Size = new Size(800, 600)
            };
            form.Controls.Add(chart);
            form.ShowDialog();
        }

        public static void ShowHeatmap(HeatmapData stats)
        {
            var form = new Form
            {
                Text = stats.Title,
                Size = new Size(800, 600)
            };
            form.Paint += (s, e) => DrawHeatmap(form.ClientRectangle, e.Graphics, stats);
            form.ShowDialog();
        }

        private static void DrawHeatmap(Rectangle clientRect, Graphics g, HeatmapData data)
        {
            var values = data.Heat.Cast<double>().ToList();
            var avgHeat = values.Average();
            var sigma = Math.Sqrt(values.Average(x => (x - avgHeat) * (x - avgHeat)));
            var cellWidth = clientRect.Width / (data.XLabels.Length + 1);
            var cellHeight = clientRect.Height / (data.YLabels.Length + 1);
            for (var x = 0; x < data.XLabels.Length; x++)
            for (var y = 0; y < data.YLabels.Length; y++)
            {
                var color = GetColor(data.Heat[x, y], avgHeat, sigma);
                var cellRect = new Rectangle(
                    clientRect.Left + cellWidth * (1 + x),
                    clientRect.Top + cellHeight * y,
                    cellWidth,
                    cellHeight
                );
                g.FillRectangle(new SolidBrush(color), cellRect);
            }

            DrawLabels(g, data, cellWidth, cellHeight);
        }

        private static void DrawLabels(Graphics g, HeatmapData data, int cellWidth, int cellHeight)
        {
            var font = new Font(FontFamily.GenericMonospace, 10);
            for (var x = 0; x < data.XLabels.Length; x++)
            {
                var text = data.XLabels[x];
                var labelRect = new RectangleF(cellWidth * (1 + x), data.YLabels.Length * cellHeight, cellWidth,
                    cellHeight);
                var format = new StringFormat();
                format.LineAlignment = StringAlignment.Near;
                format.Alignment = StringAlignment.Center;
                g.DrawString(text, font, new SolidBrush(Color.Black), labelRect, format);
            }

            for (var y = 0; y < data.YLabels.Length; y++)
            {
                var text = data.YLabels[y];
                var labelRect = new RectangleF(0, y * cellHeight, cellWidth, cellHeight);
                var format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                format.Alignment = StringAlignment.Far;
                g.DrawString(text, font, new SolidBrush(Color.Black), labelRect, format);
            }
        }


        private static Color GetColor(double value, double avgHeat, double sigma)
        {
            var sigmaValue = (value - avgHeat) / sigma;
            var colorValue = Math.Min(255, (int) (200 * Math.Abs(sigmaValue)));
            var color = sigmaValue >= 0
                ? Color.FromArgb(255, 255 - colorValue, 255, 255 - colorValue)
                : Color.FromArgb(255, 255, 255 - colorValue, 255 - colorValue);
            return color;
        }
    }
}