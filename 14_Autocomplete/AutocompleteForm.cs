using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autocomplete
{
    public partial class AutocompleteForm : Form
    {
        private long count;
        private Phrases phrases;
        private long sumMs;

        public AutocompleteForm()
        {
            InitializeComponent();
        }

        private Tuple<TimeSpan, string[], int> FindItems(string prefix)
        {
            var sw = Stopwatch.StartNew();
            var foundItems = AutocompleteTask.GetTopByPrefix(phrases, prefix, 10);
            var foundItemsCount = AutocompleteTask.GetCountByPrefix(phrases, prefix);
            if (foundItems == null)
            {
                var oneItem = AutocompleteTask.FindFirstByPrefix(phrases, prefix);
                foundItems = oneItem != null ? new[] {oneItem} : new string[0];
            }

            return Tuple.Create(sw.Elapsed, foundItems, foundItemsCount);
        }

        private async void InputBox_TextChanged(object sender, EventArgs e)
        {
            if (inputBox.ReadOnly) return;
            var prefix = inputBox.Text;
            autocompleteList.Items.Clear();
            autocompleteList.Items.Add("...searching...");
            inputBox.ReadOnly = true;
            var task = Task.Run(() => FindItems(prefix));
            await task
                .TimeoutAfter(TimeSpan.FromSeconds(2))
                .ContinueWith(UpdateUi, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void UpdateUi(Task<Tuple<TimeSpan, string[], int>> prevTask)
        {
            autocompleteList.Items.Clear();
            inputBox.ReadOnly = false;
            var tuple = prevTask.IsFaulted
                ? Tuple.Create(TimeSpan.FromSeconds(2), new[] {"... search timeout :("}, -1)
                : prevTask.Result;
            var timeTaken = (int) tuple.Item1.TotalMilliseconds;
            var foundItems = tuple.Item2;
            var foundItemsCount = tuple.Item3;
            sumMs += timeTaken;
            count++;
            statusLabel.Text = string.Format("Found: {0}; Last time: {1} ms; Average time: {2} ms", foundItemsCount,
                timeTaken, sumMs / count);
            foreach (var foundItem in foundItems)
                autocompleteList.Items.Add(foundItem);
        }

        private void AutocompleteForm_Load(object sender, EventArgs e)
        {
            phrases = CreatePhrases();
            if (phrases == null)
                Environment.Exit(-1);
            //ValidatePhrases();
        }

        private void ValidatePhrases()
        {
            MessageBox.Show(phrases.ToString());
            for (var i = 0; i < phrases.Length; i++)
                if (string.Compare(phrases[i], phrases[i + 1], StringComparison.OrdinalIgnoreCase) >= 0)
                    throw new Exception(phrases[i] + " >= " + phrases[i + 1]);
        }

        private static Phrases CreatePhrases()
        {
            try
            {
                return PhrasesLoader.CreateFromFiles("dic");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), @"Ошибка при загрузке файлов со словарями", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return null;
            }
        }
    }
}