using System.Collections.Generic;

namespace TextAnalysis
{
    static class FrequencyAnalysisTask
    {
        static void UpdateFrequencyMap(Dictionary<string, Dictionary<string, int>> freqs,
                                       List<string> words)
        {
            string prevWord = null;
            foreach (var word in words)
            {
                if (prevWord != null)
                {
                    if (!freqs.ContainsKey(prevWord)) freqs[prevWord] = new Dictionary<string, int>();
                    if (!freqs[prevWord].ContainsKey(word)) freqs[prevWord][word] = 0;
                    ++freqs[prevWord][word];
                }
                prevWord = word;
            }
        }

        static Dictionary<string, string> MakePairs(Dictionary<string, Dictionary<string, int>> freqs)
        {
            var pairs = new Dictionary<string, string>();
            foreach (var firstPair in freqs)
            {
                var savedSecond = "";
                var savedFreq = int.MinValue;
                foreach (var secondPair in firstPair.Value)
                {
                    if (savedFreq < secondPair.Value || (savedFreq == secondPair.Value &&
                        string.CompareOrdinal(secondPair.Key, savedSecond) < 0))
                    {
                        savedSecond = secondPair.Key;
                        savedFreq = secondPair.Value;
                    }
                }
                pairs[firstPair.Key] = savedSecond;
            }
            return pairs;
        }

        public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
        {
            var freqs = new Dictionary<string, Dictionary<string, int>>();
            foreach (var sentence in text) UpdateFrequencyMap(freqs, sentence);
            return MakePairs(freqs);
        }
    }
}