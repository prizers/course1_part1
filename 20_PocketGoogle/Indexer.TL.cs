using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace PocketGoogle
{
    public class Indexer : IIndexer
    {
        public Dictionary<int, Dictionary<string, List<int>>> 
			Library = new Dictionary<int, Dictionary<string, List<int>>>();
 
        public void Add(int id, string documentText)
        {
            var words = documentText.Split(' ', '.', ',', '!', '?', ':', '-', '\r', '\n');
            var diction = new Dictionary<string, List<int>>();
            int index = 0;
            foreach(var word in words)
			{
            	if (!diction.ContainsKey(word))
                {
                	diction.Add(word, new List<int>());
                    diction[word].Add(index);
                }
                else
                    diction[word].Add(index);
                index += word.Length + 1;
            }

            Library.Add(id, diction);
        }
 
        public List<int> GetIds(string word)
        {
            var result = new List<int>();
            foreach (var doc in Library)
                if (doc.Value.ContainsKey(word))
                    result.Add(doc.Key);
            return result;
        }
 
        public List<int> GetPositions(int id, string word)
        {
            var indexes = new List<int>();
            foreach (var doc in Library[id].Keys)
                if (doc == word)
                    indexes = Library[id][doc];
            return indexes;
        }
 
        public void Remove(int id)
        {
            Library.Remove(id);
        }
    }
}