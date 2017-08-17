using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace ArchPM.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class TextExtensions
    {
        public static Boolean SearchText(this String text, String searchText)
        {
            var possibleValues = searchText.ToLowerInvariant().ToSearchList();

            var contains = searchText.Contains(text.ToLowerInvariant());

            return contains;
        }

        /// <summary>
        /// Check string whether it is a valid tckn
        /// </summary>
        /// <param name="tckn"></param>
        /// <returns></returns>
        public static Boolean IsValidTCKN(this String tckn)
        {
            bool result = false;

            if (!String.IsNullOrEmpty(tckn) && tckn.Length == 11)
            {
                Int64 ATCNO, BTCNO, TcNo;
                long C1, C2, C3, C4, C5, C6, C7, C8, C9, Q1, Q2;

                result = Int64.TryParse(tckn, out TcNo);

                if (result)
                {
                    ATCNO = TcNo / 100;
                    BTCNO = TcNo / 100;

                    C1 = ATCNO % 10; ATCNO = ATCNO / 10;
                    C2 = ATCNO % 10; ATCNO = ATCNO / 10;
                    C3 = ATCNO % 10; ATCNO = ATCNO / 10;
                    C4 = ATCNO % 10; ATCNO = ATCNO / 10;
                    C5 = ATCNO % 10; ATCNO = ATCNO / 10;
                    C6 = ATCNO % 10; ATCNO = ATCNO / 10;
                    C7 = ATCNO % 10; ATCNO = ATCNO / 10;
                    C8 = ATCNO % 10; ATCNO = ATCNO / 10;
                    C9 = ATCNO % 10; ATCNO = ATCNO / 10;
                    Q1 = ((10 - ((((C1 + C3 + C5 + C7 + C9) * 3) + (C2 + C4 + C6 + C8)) % 10)) % 10);
                    Q2 = ((10 - (((((C2 + C4 + C6 + C8) + Q1) * 3) + (C1 + C3 + C5 + C7 + C9)) % 10)) % 10);

                    result = ((BTCNO * 100) + (Q1 * 10) + Q2 == TcNo);
                }
            }
            return result;
        }

        /// <summary>
        /// Tests if a String is null.
        /// Throws ValidationException with passed message.
        /// </summary>
        /// <param name="theObj">a String instance</param>
        /// <param name="msg">message to throw if the String is null</param>
        /// <param name="args">The arguments.</param>
        /// <exception cref="ArchPM.Core.Exceptions.ValidationException"></exception>
        /// <exception cref="ValidationException"></exception>
        public static void NotNullOrEmpty<T>(this String theObj, String msg, params object[] args) where T : Exception, new()
        {
            if (String.IsNullOrEmpty(theObj))
            {
                if (String.IsNullOrEmpty(msg))
                {
                    msg = "A String instance can't be null or empty";
                }


                Object temp = null;
                temp.NotNull(msg, args);
            }
        }

        /// <summary>
        /// Tests if a String is null.
        /// Throws ValidationException if object is null.
        /// </summary>
        /// <param name="theObj">a String instance</param>
        /// <exception cref="ValidationException"></exception>
        public static void NotNullOrEmpty(this String theObj, String message, params Object[] args)
        {
            NotNullOrEmpty<ArgumentNullException>(theObj, message, args);
        }

        /// <summary>
        /// Tests if a String is null or empty.
        /// if null or empty return true otherwise false
        /// </summary>
        /// <param name="theObj">a String instance</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public static Boolean IsStringNullOrEmpty(this String theObj)
        {
            return String.IsNullOrEmpty(theObj);
        }


        /// <summary>
        /// Determines whether [is not string null or empty].
        /// </summary>
        /// <param name="theObj">The object.</param>
        /// <returns></returns>
        public static Boolean IsNotNullOrEmpty(this String theObj)
        {
            return !String.IsNullOrEmpty(theObj);
        }


        /// <summary>
        /// Splits the by uppercase.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static String AddSpaceBeforeUppercase(this String text)
        {
            var converted = text.Select(x => Char.IsUpper(x) ? String.Concat(" ", x) : x.ToString());
            var singleString = converted.Aggregate((a, b) => a + b);

            return singleString;
        }

        public static List<String> SplitByUppercase(this String text)
        {
            var singleString = text.AddSpaceBeforeUppercase();
            return singleString.SplitBy(" ");
        }

        /// <summary>
        /// Splits the by uppercase.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static List<String> SplitBy(this String text, String seperator)
        {
            var list = text.Split(seperator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            list.ModifyEach(p => { return p.TrimStart().TrimEnd(); });
            return list;
        }


        /// <summary>
        /// Remove javascript dangerous characters.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static String TakeSafe(this String text, Int32 count)
        {
            if (String.IsNullOrEmpty(text))
                return "";

            if (text.Length >= count)
                text = new String(text.Take(count).ToArray());

            return text;
        }

        /// <summary>
        /// Remove javascript dangerous characters.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static String ToJavaScriptAcceptedString(this String text)
        {
            if (String.IsNullOrEmpty(text))
                return "";

            text = text.Replace("\r\n", "\\n").Replace("'", "\\'");
            return text;
        }

        /// <summary>
        /// Remove javascript dangerous characters.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static String ToSqlAcceptedString(this String text)
        {
            if (String.IsNullOrEmpty(text))
                return "";

            text = text.Replace("'", "''");
            return text;
        }


        /// <summary>
        /// Indents the string 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static String ToIndent(this String text, Int32 level)
        {
            return ToIndent(text, " ", level);
        }

        /// <summary>
        /// To the indent.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="indenter">The indenter.</param>
        /// <param name="level">The level.</param>
        /// <returns></returns>
        public static String ToIndent(this String text, String indenter, Int32 level)
        {
            if (String.IsNullOrEmpty(text))
                return "";

            for (int i = 0; i < level; i++)
            {
                text = indenter + text;
            }
            return text;
        }

        /// <summary>
        /// Uppercase first letter first character
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        /// <example>national basketball league =&gt; National basketball league</example>
        public static String ToFirstUpperRestLower(this String text)
        {
            if (String.IsNullOrEmpty(text))
                return "";

            String first = text.Substring(0, 1).ToUpper();
            String temp = String.Concat(first, text.Substring(1).ToLower());
            return temp;
        }

        public static String ToFirstLower(this String text)
        {
            if (String.IsNullOrEmpty(text))
                return "";

            String first = text.Substring(0, 1).ToLower();
            String temp = String.Concat(first, text.Substring(1));
            return temp;
        }

        /// <summary>
        /// Turn text to pascal case
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        /// <example>national basketball league =&gt; National Basketball League</example>
        public static String ToFirstUpperAllWords(this String text)
        {
            if (String.IsNullOrEmpty(text))
                return "";

            StringBuilder sb = new StringBuilder();
            String[] split = text.Split(new char[] { ' ' });

            foreach (String str in split)
            {
                sb.Append(ToFirstUpperRestLower(str));
                sb.Append(" ");
            }

            return sb.ToString().Trim();
        }

        /// <summary>
        /// Gets first characters of all letters as uppercase
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        /// <example>national football league =&gt; NFL</example>
        public static String ToShort(this String text)
        {
            if (String.IsNullOrEmpty(text))
                return "";

            StringBuilder sb = new StringBuilder();
            String first = String.Empty;
            String[] split = text.Split(new char[] { ' ' });

            foreach (String str in split)
            {
                first = str.Substring(0, 1).ToUpper();
                sb.Append(first);
            }

            return sb.ToString().Trim();
        }

        /// <summary>
        /// Creates url base string. removes unwanted literals
        /// Use - as seperator
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static String ToUrl(this String text)
        {
            var result = RemoveDengerous(text, '-');
            return result;
        }

        /// <summary>
        /// Removes the dengerous.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="replaced">The replaced.</param>
        /// <returns></returns>
        public static String RemoveDengerous(String text, Char replaced = '-')
        {
            if (String.IsNullOrEmpty(text))
                return "";

            #region Dengerous Literals

            var list = new Dictionary<Char, Char>();
            list.Add('ç', 'c');
            list.Add('ö', 'o');
            list.Add('ş', 's');
            list.Add('ı', 'i');
            list.Add('ğ', 'g');
            list.Add('ü', 'u');
            list.Add('Ç', 'C');
            list.Add('Ö', 'O');
            list.Add('Ş', 'S');
            list.Add('İ', 'I');
            list.Add('Ğ', 'G');
            list.Add('Ü', 'U');
            list.Add(' ', replaced);
            list.Add('/', replaced);
            list.Add('!', replaced);
            list.Add('(', replaced);
            list.Add(')', replaced);
            list.Add('?', replaced);
            list.Add('.', replaced);
            list.Add(',', replaced);
            list.Add(':', replaced);
            list.Add(';', replaced);
            list.Add('\'', replaced);
            list.Add('\\', replaced);
            list.Add('`', replaced);
            list.Add('"', replaced);
            list.Add('+', replaced);
            list.Add('%', replaced);
            list.Add('*', replaced);
            list.Add('\r', replaced);
            list.Add('\n', replaced);
            list.Add('“', replaced);
            list.Add('”', replaced);
            list.Add('’', replaced);


            #endregion


            Array.ForEach(text.ToCharArray(), (c)
                =>
            {
                if (list.ContainsKey(c))
                {
                    text = text.Replace(c, list[c]);
                }
            });

            String specialDoubleInARowCondition = String.Format("{0}{0}", replaced);
            while (text.Contains(specialDoubleInARowCondition))
            {
                text = text.Replace(specialDoubleInARowCondition, replaced.ToString());
            }

            text = text.TrimEnd(replaced);
            text = text.TrimStart(replaced);

            return text;
        }


        /// <summary>
        /// Converts Turkish to English String
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static String ToEnglish(this String text)
        {
            if (String.IsNullOrEmpty(text))
                return "";

            #region Turkish to English Literals

            var list = new Dictionary<Char, Char>();
            list.Add('ç', 'c');
            list.Add('ö', 'o');
            list.Add('ş', 's');
            list.Add('ı', 'i');
            list.Add('ğ', 'g');
            list.Add('ü', 'u');
            list.Add('Ç', 'C');
            list.Add('Ö', 'O');
            list.Add('Ş', 'S');
            list.Add('İ', 'I');
            list.Add('Ğ', 'G');
            list.Add('Ü', 'U');

            #endregion

            text.ToList().ForEach(c => {
                if (list.ContainsKey(c))
                {
                    text = text.Replace(c, list[c]);
                }
            });

            return text;
        }

        /// <summary>
        /// To the change character.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="index">The index.</param>
        /// <param name="newChar">The new character.</param>
        /// <returns></returns>
        public static String ToChangeChar(this String text, Int32 index, Char newChar)
        {
            char[] tmpBuffer = text.ToCharArray();
            tmpBuffer[index] = newChar;
            text = new string(tmpBuffer);
            return text;
        }

        /// <summary>
        /// To the search list.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static IEnumerable<String> ToSearchList(this String text)
        {
            return SearchHelper.Instance.Find(text);
        }

        /// <summary>
        /// To the unsecure string.
        /// </summary>
        /// <param name="secureString">The secure string.</param>
        /// <returns></returns>
        public static String ToUnsecureString(this SecureString secureString)
        {
            IntPtr stringPointer = Marshal.SecureStringToBSTR(secureString);
            string normalString = Marshal.PtrToStringBSTR(stringPointer);
            Marshal.ZeroFreeBSTR(stringPointer);

            return normalString;
        }

        /// <summary>
        /// To the secure string.
        /// </summary>
        /// <param name="unsecureString">The unsecure string.</param>
        /// <returns></returns>
        public static SecureString ToSecureString(this String unsecureString)
        {
            SecureString secureStr = new SecureString();
            for (int i = 0; i < unsecureString.Length; i++)
            {
                secureStr.AppendChar(unsecureString[i]);
            }
            secureStr.MakeReadOnly();

            return secureStr;
        }

        

    }
}
