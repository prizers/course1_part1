using System.Collections.Generic;
using System.Text;

namespace TextAnalysis
{
    static class SentencesParserTask
    {
        public static readonly string[] StopWords =
        {
            "the", "and", "to", "a", "of", "in", "on", "at", "that",
            "as", "but", "with", "out", "for", "up", "one", "from", "into"
        };

        public static readonly HashSet<string> StopwordsSet = new HashSet<string>(StopWords);

        public static readonly char[] PunctuationChars = { '.', '!', '?', ';', ':', '(', ')' };


        static void CheckAndAdd(List<string> words, string word)
        {
            if (0 < word.Length && !StopwordsSet.Contains(word)) words.Add(word);
        }

        static List<string> ExtractListOfMeaningfulWords(string text)
        {
            var words = new List<string>();
            var collector = new StringBuilder();
            foreach (var ch in text)
            {
                if (char.IsLetter(ch) || ch == '\'')
                {
                    collector.Append(char.ToLower(ch));
                }
                else
                {
                    CheckAndAdd(words, collector.ToString());
                    collector.Clear();
                }
            }
            CheckAndAdd(words, collector.ToString());
            return words;
        }

        public static List<List<string>> ParseSentences(string text)
        {
            var sentences = new List<List<string>>();
            foreach (var sentence in text.Split(PunctuationChars))
            {
                var words = ExtractListOfMeaningfulWords(sentence);
                if (0 < words.Count) sentences.Add(words);
            }
            return sentences;
        }
    }
}