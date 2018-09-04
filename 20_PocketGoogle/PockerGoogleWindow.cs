using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PocketGoogle
{
    public partial class PocketGoogleWindow : Form
    {
        private readonly IIndexer indexer;
        private readonly TextBox request;
        private readonly Panel results;
        private readonly Button run;

        private readonly Label search;

        private readonly Dictionary<int, string> texts;

        public PocketGoogleWindow(IIndexer indexer, Dictionary<int, string> texts)
        {
            this.indexer = indexer;
            this.texts = texts;

            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            ClientSize = new Size(800, 600);
            search = new Label
            {
                Text = "Search: ",
                Location = new Point(0, 0),
                Size = new Size(100, 20)
            };
            request = new TextBox
            {
                Text = "Я",
                Location = new Point(100, 0),
                Size = new Size(600, 20)
            };
            run = new Button
            {
                Text = "I feel lucky!",
                Location = new Point(700, 0),
                Size = new Size(100, 20)
            };
            results = new Panel
            {
                Location = new Point(0, 20),
                Size = new Size(800, 580)
            };

            run.Click += PerformSearch;

            Controls.Add(search);
            Controls.Add(request);
            Controls.Add(run);
            Controls.Add(results);
        }

        private void PerformSearch(object sender, EventArgs e)
        {
            var ids = indexer.GetIds(request.Text);
            results.Controls.Clear();
            var y = 0;
            foreach (var id in ids)
            {
                var box = new RichTextBox();
                box.Text = texts[id];
                foreach (var position in indexer.GetPositions(id, request.Text))
                {
                    box.Select(position, request.Text.Length);
                    box.SelectionBackColor = Color.LightBlue;
                }

                box.ReadOnly = true;
                box.Size = new Size(results.Width, 50);
                box.Location = new Point(0, y);
                y += box.Height;
                results.Controls.Add(box);
            }
        }
    }
}