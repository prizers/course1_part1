using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TableParser
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            var filename = args.Length > 0 ? args[0] : "data.txt";
            var lines = File.ReadAllLines(filename);
            var rows = lines.Select(FieldsParserTask.ParseLine).ToList();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(CreateForm(lines, rows));
        }


        private static Form CreateForm(string[] inputLines, List<List<string>> rows)
        {
            var form = new Form();
            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                RowCount = rows.Count,
                ColumnCount = 1 + rows.Max(r => r.Count)
            };

            grid.Columns[0].HeaderText = "Input";
            for (var c = 1; c < grid.ColumnCount; c++)
                grid.Columns[c].HeaderText = "field #" + c;

            for (var r = 0; r < rows.Count; r++)
            {
                grid[0, r].Value = inputLines[r];
                for (var c = 0; c < rows[r].Count; c++)
                    grid[c + 1, r].Value = rows[r][c];
            }

            form.Controls.Add(grid);
            return form;
        }
    }
}