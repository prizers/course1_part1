using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketGoogle
{
    class Database : Dictionary<string, Dictionary<int, List<int>>>
    {
        public void Add(int documentId, int position, string word)
        {
            var docDict = ContainsKey(word) ? this[word]
                                            : this[word] = new Dictionary<int, List<int>>();
            var docList = docDict.ContainsKey(documentId) ? docDict[documentId]
                                                          : docDict[documentId] = new List<int>();
            docList.Add(position);
        }

        public List<int> GetIds(string word) =>
            ContainsKey(word) ? this[word].Keys.ToList() : new List<int>();

        public List<int> GetPositions(int documentId, string word) =>
            ContainsKey(word) && this[word].ContainsKey(documentId) ? this[word][documentId]
                                                                    : new List<int>();

        public void RemoveDocument(int documentId)
        {
            foreach (var docDict in this.Values) docDict.Remove(documentId);
        }
    }

    class Parser
    {
        public Parser(Database database, int documentId, string document)
        {
            id = documentId;
            db = database;
            text = document;
            position = 0;
        }

        public void Process()
        {
            for (; ; )
            {
                SkipDelimiters();
                if (position == text.Length) break;
                ProcessWord();
            }
        }

        void ProcessWord()
        {
            var length = GetWordLength();
            string word = text.Substring(position, length);
            db.Add(id, position, word);
            position += length;
        }

        void SkipDelimiters()
        {
            while (position < text.Length &&
                Delimiters.IndexOf(text[position]) != -1)
                ++position;
        }

        int GetWordLength()
        {
            var endPosition = position;
            while (endPosition < text.Length &&
                Delimiters.IndexOf(text[endPosition]) == -1)
                ++endPosition;
            return endPosition - position;
        }

        private const string Delimiters = " .,!?:-\r\n";
        private Database db;
        private int id;
        private string text;
        private int position;
    }

    public class Indexer : IIndexer
    {
        public Indexer()
        {
            db = new Database();
        }

        /*
         * Add. Этот метод должен индексировать все слова в документе. 
         * Разделители слов: { ' ', '.', ',', '!', '?', ':', '-','\r','\n' }; 
         * Сложность – O(document.Length)
         */
        public void Add(int id, string documentText)
        {
            new Parser(db, id, documentText).Process();
        }

        /*
         * GetIds. Этот метод должен искать по слову все id документов, где оно встречается. 
         * Сложность — O(result), где result — размер ответа на запрос
         */
        public List<int> GetIds(string word) => db.GetIds(word);

        /*
         * GetPositions. Этот метод по слову и id документа должен искать все позиции, 
         * в которых слово начинается. Сложность — O(result)
         */
        public List<int> GetPositions(int id, string word) => db.GetPositions(id, word);

        /*
         * Remove.Этот метод должен удалять документ из индекса, 
         * после чего слова в нем искаться больше не должны.
         * Сложность — O(document.Length)
         */
        public void Remove(int id)
        {
            db.RemoveDocument(id);
        }

        // two-level dictionary word -> id -> indexes
        private Database db;
    }
}

