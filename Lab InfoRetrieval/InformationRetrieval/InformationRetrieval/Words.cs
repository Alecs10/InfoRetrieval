using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace InformationRetrieval
{
    class Words
    {
        public List<String> allWords;
        public List<String> noDuplicates;
        Dictionary<string, Dictionary<string, int>> frequencyMatrix;
        List<String> stopWords;


        public Words()
        {
            allWords = new List<string>();
            noDuplicates = new List<string>();
            frequencyMatrix = new Dictionary<string, Dictionary<string, int>>();
        }
        public void ReadWordsFromFiles(string path)
        {
            string[] docs = Directory.GetFiles(path);

            foreach (string str in docs)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(str);
                char[] separators = new char[] { ' ', ',', '?', '!', '"', ':', ';', '{', '}', '(', ')',
                                                '/', '#', '@', '$', '%', '*', '-', '&', '^', '|', '\t',
                                                '\b','\r','\v','\f','\'','+'};

                // Title
                var titleList = doc.GetElementsByTagName("title");
                string Title = titleList.Item(0).InnerXml;

                Title = removeNumbers(Title);
                Title = removeParagraphTags(Title);
                foreach (string word in Title.Split(separators, System.StringSplitOptions.RemoveEmptyEntries))
                {

                    allWords.Add(word.ToLower());

                }


                // Text

                var contentList = doc.GetElementsByTagName("text");
                string Content = contentList.Item(0).InnerXml;
                Content = removeNumbers(Content);
                Content = removeParagraphTags(Content);

                foreach (string word in Content.Split(separators, System.StringSplitOptions.RemoveEmptyEntries))
                {
                    allWords.Add(word.ToLower());

                }




            }

        }

        string removeNumbers(string input)
        {
            return Regex.Replace(input, @"[\d-]", string.Empty);
        }

        string removeParagraphTags(string input)
        {
            input = input.Replace("<p>", " ");
            input = input.Replace("</p>", " ");
            input = input.Replace(", ", " ");

            return input;
        }

        public void removeUnnecessaryDots()
        {

            for (int i = 0; i < allWords.Count; i++)
            {

                if (allWords[i].Equals("..."))
                {
                    allWords.RemoveAt(i);
                }

                if (allWords[i][allWords[i].Length - 1].Equals('.')) // if the last char is a "."
                {
                    allWords[i] = allWords[i].Remove(allWords[i].Length - 1); // remove it
                }

                if (allWords[i].Contains('.'))
                {
                    var tempWords = allWords[i].Split('.');


                    bool flag = true;

                    foreach (var elem in tempWords)
                    {
                        if (elem.Length > 1)
                        {
                            flag = false;
                        }
                    }

                    if (!flag)
                    {
                        allWords.RemoveAt(i);

                        for (int j = tempWords.Length - 1; j >= 0; j--)
                        {
                            if (tempWords[j].Length > 1)
                            {
                                allWords.Insert(i, tempWords[j]);
                            }
                        }


                    }
                }

            }

        }

        public void loadStopWords(string path)
        {
            var temp = File.ReadAllLines(path);
            stopWords = new List<string>(temp);
        }

        public void removeStopWords()
        {
            for (int i = allWords.Count - 1; i >= 0; i--)
            {


                if (allWords[i].Length <= 2)
                {
                    allWords.RemoveAt(i);
                }

                if (stopWords.IndexOf(allWords[i].ToLower()) >= 0)
                {
                    allWords.RemoveAt(i);
                }
                if (String.IsNullOrEmpty(allWords[i]))
                {
                    allWords.RemoveAt(i);
                }
            }
        }

        public void removeDuplicates()
        {

            foreach (var cuv in allWords)
            {
                if (!noDuplicates.Contains(cuv))
                {
                    noDuplicates.Add(cuv);
                }
            }

        }

        public void saveWordstoFile(List<String> words,string filepath)
        {
            using (StreamWriter outputFile = new StreamWriter(filepath))
                foreach (string cuvant in words)
                {
                    outputFile.WriteLine(cuvant);

                }
        }

        public void generateFreqMatrix(string path)
        {
            string[] docs = Directory.GetFiles(path);

            foreach (string str in docs)
            {
                Dictionary<string, int> wordsFrequencyInAFile = new Dictionary<string, int>();
                foreach (string word in noDuplicates) {
                    wordsFrequencyInAFile.Add(word, 0);
                }


                XmlDocument doc = new XmlDocument();
                doc.Load(str);
                char[] separators = new char[] { ' ', ',', '?', '!', '"', ':', ';', '{', '}', '(', ')',
                                                '/', '#', '@', '$', '%', '*', '-', '&', '^', '|', '\t',
                                                '\b','\r','\v','\f','\'','+'};

                // Title
                var titleList = doc.GetElementsByTagName("title");
                string Title = titleList.Item(0).InnerXml;

                Title = removeNumbers(Title);
                Title = removeParagraphTags(Title);
                foreach (string word in Title.Split(separators, System.StringSplitOptions.RemoveEmptyEntries))
                {

                    if (wordsFrequencyInAFile.ContainsKey(word)){
                        wordsFrequencyInAFile[word]++;
                    }

                }


                // Text

                var contentList = doc.GetElementsByTagName("text");
                string Content = contentList.Item(0).InnerXml;
                Content = removeNumbers(Content);
                Content = removeParagraphTags(Content);

                foreach (string word in Content.Split(separators, System.StringSplitOptions.RemoveEmptyEntries))
                {
                    if (wordsFrequencyInAFile.ContainsKey(word))
                    {
                        wordsFrequencyInAFile[word]++;
                    }

                }


                frequencyMatrix.Add(str, wordsFrequencyInAFile);

            }
        }

        public void printWords()
        {
            foreach (string cuvant in allWords)
            {
                Console.WriteLine(cuvant);

            }
        }
    }
}
