using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace TextAnalysis
{
    [TestFixture]
    public class SentencesParser_Tests
    {
        [Test]
        [Order(00)]
        public void ReturnCorrectResult_OnTextWithOneSentenceWithOneWord()
        {
            var text = "abc";
            var expected = new List<List<string>> { new List<string> { "abc" } };
            var actual = SentencesParserTask.ParseSentences(text);
            AssertAllSentencesEqual(expected, actual, text);
        }

        [Test]
        [Order(10)]
        public void ReturnCorrectResult_OnTextWithOneSentenceWithTwoWords()
        {
            var text = "b, c";
            var expected = new List<List<string>> { new List<string> { "b", "c" } };
            var actual = SentencesParserTask.ParseSentences(text);
            AssertAllSentencesEqual(expected, actual, text);
        }

        [Test]
        [Order(20)]
        public void ReturnCorrectResult_OnTextWithOneSentence_WithWordContainingApostrophe()
        {
            var text = "it's";
            var expected = new List<List<string>> { new List<string> { "it's" } };
            var actual = SentencesParserTask.ParseSentences(text);
            AssertAllSentencesEqual(expected, actual, text);
        }

        [Test]
        [Order(30)]
        public void CorrectlyParse_SentenceDelimiters()
        {
            var text = "a.b!c?d:e;f(g)h;i";
            var expected = new List<List<string>>
            {
                new List<string> {"a"},
                new List<string> {"b"},
                new List<string> {"c"},
                new List<string> {"d"},
                new List<string> {"e"},
                new List<string> {"f"},
                new List<string> {"g"},
                new List<string> {"h"},
                new List<string> {"i"}
            };

            var actual = SentencesParserTask.ParseSentences(text);
            AssertAllSentencesEqual(expected, actual, text);
        }

        [Test]
        [Order(40)]
        public void CorrectlyParse_SpecialCharacters()
        {
            var originalText = "b;\tc;\rd;\ne;\r\nf;\r\n\r\ng";
            var escapedText = Regex.Escape(originalText);
            var expected = new List<List<string>>
            {
                new List<string> {"b"},
                new List<string> {"c"},
                new List<string> {"d"},
                new List<string> {"e"},
                new List<string> {"f"},
                new List<string> {"g"}
            };
            var actual = SentencesParserTask.ParseSentences(originalText);
            AssertAllSentencesEqual(expected, actual, escapedText);
        }

        [Test]
        [Order(50)]
        public void CorrectlyParse_OneSentenceWithWordDelimiter(
            [Values('^', '#', '$', '-', '+', '1', '=', ' ', '\t', '\n', '\r')]
            char delimiter)
        {
            var text = "x" + delimiter + "y";
            var expected = new List<List<string>>
            {
                new List<string> {"x", "y"}
            };
            var actual = SentencesParserTask.ParseSentences(text);
            AssertAllSentencesEqual(expected, actual, text);
        }

        [Test]
        [Order(60)]
        public void ReturnResult_InLowerCase()
        {
            var text = "B.C.D";
            var expected = new List<List<string>>
            {
                new List<string> {"b"},
                new List<string> {"c"},
                new List<string> {"d"}
            };
            var actual = SentencesParserTask.ParseSentences(text);
            AssertAllSentencesEqual(expected, actual, text);
        }

        [Test]
        [Order(80)]
        public void NotReturnEmptySentence([Values("..", "...!!?","")]
            string text)
        {
            var expected = new List<List<string>>();

            var actual = SentencesParserTask.ParseSentences(text);

            AssertAllSentencesEqual(expected, actual, text);
        }

        protected static void AssertAllSentencesEqual(
            List<List<string>> expectedSentences,
            List<List<string>> actualSentences,
            string text)
        {
            var actualLines = actualSentences.Select(sentence => string.Join(" ", sentence)).ToArray();
            var expectedLines = expectedSentences.Select(sentence => string.Join(" ", sentence)).ToArray();
            for (var i = 0; i < Math.Min(expectedSentences.Count, actualSentences.Count); i++)
                if (actualLines[i] != expectedLines[i])
                    AssertSentenceEuqal(expectedSentences[i], actualSentences[i], text, i);
            CollectionAssert.AreEqual(expectedSentences, actualSentences,
                $"Input text: [{text}].\nWrong number of sentences.");
        }

        protected static void AssertSentenceEuqal(
            List<string> expected,
            List<string> actual,
            string text,
            int sentenceNumber)
        {
            CollectionAssert.AreEqual(expected, actual, $"Input text: [{text}]\nSentence #{sentenceNumber} is wrong");
        }
    }
}