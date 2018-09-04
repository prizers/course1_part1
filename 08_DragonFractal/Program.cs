using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
// Это пространство имен содержит средства создания оконных приложений. В частности в нем находится класс Form.
// Для того, чтобы оно стало доступно, в проект был подключен на System.Windows.Forms.dll

namespace Fractals
{
    internal static class Program
    {
        private static void Main()
        {
            var pixels = new Pixels();
            var image = new Bitmap(800, 800, PixelFormat.Format24bppRgb);
            using (var g = Graphics.FromImage(image))
            {
                g.Clear(Color.Black);
            }

            DragonFractalTask.DrawDragonFractal(pixels, 100000, 123456);
            pixels.DrawToBitmap(image);

            // При желании можно сохранить созданное изображение в файл вот так:
            // image.Save("dragon.png", ImageFormat.Png);

            ShowImageInWindow(image);
        }

        private static void ShowImageInWindow(Bitmap image)
        {
            // Создание нового окна заданного размера:
            var form = new Form
            {
                Text = "Harter–Heighway dragon",
                ClientSize = image.Size
            };

            //Добавляем специальный элемент управления PictureBox, который умеет отображать созданное нами изображение.
            form.Controls.Add(new PictureBox
            {
                Image = image,
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.CenterImage
            });
            form.ShowDialog();
        }
    }
}