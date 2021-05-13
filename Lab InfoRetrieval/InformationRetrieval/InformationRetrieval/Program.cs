using System;
using System.Xml;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace InformationRetrieval
{
    class Program
    {
        static void Main(string[] args)
        {
            Words myWords = new Words();
            myWords.ReadWordsFromFiles(@"C:\Users\popa_\Desktop\Facultate\An3Sem2\Lab InfoRetrieval\Reuters_34");
            myWords.loadStopWords(@"C:\Users\popa_\Desktop\Facultate\An3Sem2\Lab InfoRetrieval\InformationRetrieval\stop_words_english.txt");
            myWords.removeUnnecessaryDots();
            myWords.removeStopWords();
            myWords.saveWordstoFile(myWords.allWords,@"C:\Users\popa_\Desktop\Facultate\An3Sem2\Lab InfoRetrieval\InformationRetrieval\old.txt");
            myWords.removeDuplicates();
            myWords.saveWordstoFile(myWords.noDuplicates,@"C:\Users\popa_\Desktop\Facultate\An3Sem2\Lab InfoRetrieval\InformationRetrieval\new.txt");
            myWords.generateFreqMatrix(@"C:\Users\popa_\Desktop\Facultate\An3Sem2\Lab InfoRetrieval\Reuters_34");
        }
    }
}
