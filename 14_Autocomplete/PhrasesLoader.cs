using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Autocomplete
{
    public class PhrasesLoader
    {
        public static Phrases CreateFromFiles(string directory)
        {
            var verbs = LoadDictionary(directory, "verbs.txt");
            var adjectives = LoadDictionary(directory, "adjectives.txt");
            var nouns = LoadDictionary(directory, "nouns.txt");
            return new Phrases(verbs, adjectives, nouns);
        }

        private static string[] LoadDictionary(string directory, string filename)
        {
            return File.ReadAllLines(Path.Combine(directory, filename))
                .Select(a => a.ToLower())
                .Distinct()
                .OrderBy(a => a, StringComparer.OrdinalIgnoreCase)
                .ToArray();
        }

        public static Phrases CreateFromResouces()
        {
            return new Phrases(
                GetResourceContent("verbs.txt"),
                GetResourceContent("adjectives.txt"),
                GetResourceContent("nouns.txt"));
        }

        private static string[] GetResourceContent(string resouceName)
        {
            using (var stream = new StreamReader(Assembly.GetExecutingAssembly()
                .GetManifestResourceStream(string.Join(".", "autocomplete", "dic", resouceName))))
            {
                var lines = new List<string>();
                string line;
                while ((line = stream.ReadLine()) != null)
                    lines.Add(line);
                return lines.ToArray();
            }
        }
    }
}