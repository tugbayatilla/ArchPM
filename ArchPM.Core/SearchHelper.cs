using System;
using System.Collections.Generic;
using ArchPM.Core.Extensions;


namespace ArchPM.Core
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SearchHelper
    {
        /// <summary>
        /// The instance
        /// </summary>
        public static readonly SearchHelper Instance = new SearchHelper();

        /// <summary>
        /// Gets the global letter list.
        /// </summary>
        /// <value>
        /// The global letter list.
        /// </value>
        public Dictionary<Char, Char> GlobalLetterList { get; private set; }

        /// <summary>
        /// Prevents a default instance of the <see cref="SearchHelper"/> class from being created.
        /// </summary>
        private SearchHelper()
        {
            GlobalLetterList = new Dictionary<Char, Char>();
            GlobalLetterList.Add('ç', 'c');
            GlobalLetterList.Add('ö', 'o');
            GlobalLetterList.Add('ş', 's');
            GlobalLetterList.Add('ı', 'i');
            GlobalLetterList.Add('ğ', 'g');
            GlobalLetterList.Add('ü', 'u');
            GlobalLetterList.Add('Ç', 'C');
            GlobalLetterList.Add('Ö', 'O');
            GlobalLetterList.Add('Ş', 'S');
            GlobalLetterList.Add('İ', 'I');
            GlobalLetterList.Add('Ğ', 'G');
            GlobalLetterList.Add('Ü', 'U');
            GlobalLetterList.Add('c', 'ç');
            GlobalLetterList.Add('o', 'ö');
            GlobalLetterList.Add('s', 'ş');
            GlobalLetterList.Add('i', 'ı');
            GlobalLetterList.Add('g', 'ğ');
            GlobalLetterList.Add('u', 'ü');
            GlobalLetterList.Add('C', 'Ç');
            GlobalLetterList.Add('O', 'Ö');
            GlobalLetterList.Add('S', 'Ş');
            GlobalLetterList.Add('I', 'İ');
            GlobalLetterList.Add('G', 'Ğ');
            GlobalLetterList.Add('U', 'Ü');
        }

        /// <summary>
        /// Creates list of strings, permutation of given text,
        /// tuğbay -&gt; tuğbay, tüğbay, tugbay, tügbay
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public IEnumerable<String> Find(String text)
        {
            //clear result
            var result = new List<String>();

            var split = text.Split(" ".ToCharArray());
            var splitUnique = new List<String>();
            foreach (var item in split)
            {
                if (!splitUnique.Contains(item))
                    splitUnique.Add(item);
            }

            foreach (var uniqueWord in splitUnique)
            {
                if (!String.IsNullOrEmpty(uniqueWord))
                {
                    result.AddAsUnique(uniqueWord);
                    FindRec(ref result, uniqueWord, 0);
                }
            }

            return result;
        }

        /// <summary>
        /// Finds the record.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="text">The text.</param>
        /// <param name="index">The index.</param>
        void FindRec(ref List<String> result, String text, Int32 index)
        {
            for (int i = index; i < text.Length; i++)
            {
                var letter = text[i];
                String letterChangedText = String.Empty;

                //found letter
                if (GlobalLetterList.ContainsValue(letter))
                {
                    //change letter
                    index = text.IndexOf(letter);
                    letterChangedText = text.ToChangeChar(index, GlobalLetterList.FindKeyByValue(letter));

                    //changed word found, add to list
                    result.AddAsUnique(letterChangedText);

                    //recursive call
                    FindRec(ref result, letterChangedText, index + 1);
                }
            }
        }


    }
}
