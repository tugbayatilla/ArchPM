using ArchPM.Core.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ArchPM.Core.Extensions
{
    /// <summary>
    /// Speak As money
    /// </summary>
    public static class MoneyExtensions
    {

        #region Private Properties

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>
        /// The culture.
        /// </value>
        static CultureInfo Culture { get; set; }

        /// <summary>
        /// Gets or sets the currency code.
        /// </summary>
        /// <value>
        /// The currency code.
        /// </value>
        static CurrencyCodes CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets the localized text.
        /// </summary>
        /// <value>
        /// The localized text.
        /// </value>
        static Dictionary<string, Dictionary<String, String>> LocalizedText { get; set; }

        #endregion

       
        /// <summary>
        /// Default TL
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        public static String CurrencyToWord(this decimal number)
        {
            string numberStr = number.ToStringWithPoint();

            return numberStr.CurrencyToWord();
        }

        /// <summary>
        /// Currencies to word.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="currencyCode">The currency code.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static String CurrencyToWord(this decimal number, CurrencyCodes currencyCode = CurrencyCodes.TL, String culture = "tr-TR")
        {
            string numberStr = number.ToStringWithPoint();

            return numberStr.CurrencyToWord(currencyCode, culture);
        }

        /// <summary>
        /// Currencies to word.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static String CurrencyToWord(this decimal number, String culture = "tr-TR")
        {
            string numberStr = number.ToStringWithPoint();

            return numberStr.CurrencyToWord(CurrencyCodes.TL, culture);
        }

        #region Privates

        /// <summary>
        /// Currencies to word.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="currencyCode">The currency code.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// You must enter a correct value in the variable of culture. For example; en-US, tr-TR
        /// or
        /// You must enter a correct value in the variable of culture. For example; en-US, tr-TR
        /// </exception>
        static String CurrencyToWord(this String number, CurrencyCodes currencyCode = CurrencyCodes.TL, String culture = "tr-TR")
        {
            if (string.IsNullOrEmpty(culture))
                throw new Exception("You must enter a correct value in the variable of culture. For example; en-US, tr-TR");

            Culture = new CultureInfo(culture);
            if (Culture == null)
                throw new Exception("You must enter a correct value in the variable of culture. For example; en-US, tr-TR");

            CurrencyCode = currencyCode;

            PrepareLocalizedValue();
            CheckNumberFormat(number);
            PrepareInOrderToUse(ref number);

            return ConvertToWords(number);
        }


        /// <summary>
        /// Prepares the in order to use.
        /// </summary>
        /// <param name="number">The number.</param>
        static void PrepareInOrderToUse(ref string number)
        {
            number = number.Trim();
            number = number.TrimStart('0');
        }

        /// <summary>
        /// Checks the number format.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <exception cref="System.Exception">
        /// The comma seperator cannot use.
        /// or
        /// You can use just one point seperator.
        /// or
        /// The number which next from decimal point must be less than one hundred.
        /// or
        /// The number is not valid.
        /// </exception>
        static void CheckNumberFormat(string number)
        {
            if (number.IndexOf(',') > 0)
                throw new Exception("The comma seperator cannot use.");

            string[] arr = number.Split('.');
            if (arr.Length > 2)
                throw new Exception("You can use just one point seperator.");

            if (arr.Length == 2 && arr[1].Length > 2)
                throw new Exception("The number which next from decimal point must be less than one hundred.");

            decimal parseResult = 0;
            if (!decimal.TryParse(arr[0], NumberStyles.AllowDecimalPoint, Culture, out parseResult) || !Decimal.TryParse(arr[1], NumberStyles.AllowDecimalPoint, Culture, out parseResult))
                throw new Exception("The number is not valid.");
        }

        /// <summary>
        /// Converts to words.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        static String ConvertToWords(string number)
        {
            string val = "";
            string wholeNo = "";
            string wholeText = "";
            string points = "";
            string andStr = GetLocalizedValue("and");
            string decimalStr = "";
            string startStr = GetLocalizedValue("Only");
            string endStr = "";

            try
            {
                int decimalPlace = number.IndexOf(".");
                if (decimalPlace > 0 || number.StartsWith("."))
                {
                    if (decimalPlace > 0)
                    {
                        wholeNo = number.Substring(0, decimalPlace);
                        wholeText = string.Format(" {0} {1}", TranslateWholeNumber(wholeNo), CurrencyCode);
                    }

                    points = number.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        decimalStr = decimalPlace > 0 ? andStr + " " : "";
                        decimalStr += TranslateWholeNumber(points);
                        if (points.Length < 3)
                            endStr = GetDecimalCode();
                    }
                }

                val = string.Format("{0}{1} {2} {3}", startStr, wholeText, decimalStr, endStr);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return val;
        }

        /// <summary>
        /// Gets the decimal code.
        /// </summary>
        /// <returns></returns>
        static String GetDecimalCode()
        {
            String result = "Cents";

            switch (CurrencyCode)
            {
                case CurrencyCodes.TL:
                    result = "Kuruş";
                    break;
                case CurrencyCodes.USD:
                    result = "Cents";
                    break;
                case CurrencyCodes.EUR:
                    result = "Cents";
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// Translates the whole number.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        static String TranslateWholeNumber(string number)
        {
            string word = "";

            try
            {
                bool beginsZero = false;//tests for 0XX
                bool isDone = false;//test if already translated
                double dblAmt = Convert.ToDouble(number);

                //if ((dblAmt > 0) && number.StartsWith("0"))
                if (dblAmt > 0)
                {//test for zero or digit zero in a nuemric
                    beginsZero = number.StartsWith("0");
                    int numDigits = dblAmt.ToString().Length;
                    int pos = 0;//store digit grouping
                    string place = "";//digit grouping name:hundres,thousand,etc...

                    switch (numDigits)
                    {
                        case 1://ones' range
                            word = Ones(dblAmt.ToString());
                            isDone = true;
                            break;

                        case 2://tens' range
                            word = Tens(dblAmt.ToString());
                            isDone = true;
                            break;

                        case 3://hundreds' range
                            pos = (numDigits % 3) + 1;
                            place = GetLocalizedValue("Hundred");
                            break;

                        case 4://thousands' range
                        case 5:
                        case 6:
                            pos = (numDigits % 4) + 1;
                            place = GetLocalizedValue("Thousand");
                            break;

                        case 7://millions' range
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = GetLocalizedValue("Million");
                            break;

                        case 10://Billions's range
                        case 11://Billions's range
                        case 12://Billions's range
                            pos = (numDigits % 10) + 1;
                            place = GetLocalizedValue("Billion");
                            break;

                        case 13://Trillion's range
                        case 14://Trillion's range
                        case 15://Trillion's range
                            pos = (numDigits % 13) + 1;
                            place = GetLocalizedValue("Trillion");
                            break;

                        //add extra case options for anything above Billion...
                        default:
                            isDone = true;
                            break;
                    }

                    bool isTurkishHundredOrThousand = Culture.Name == "tr-TR" && dblAmt.ToString().Substring(0, pos) == "1" && (numDigits == 3 || numDigits == 4);
                    if (!isDone)
                    {//if transalation is not done, continue...(Recursion comes in now!!)
                        if (isTurkishHundredOrThousand)
                            word = place + " " + TranslateWholeNumber(dblAmt.ToString().Substring(pos));
                        else
                            word = TranslateWholeNumber(dblAmt.ToString().Substring(0, pos)) + " " + place + " " + TranslateWholeNumber(dblAmt.ToString().Substring(pos));

                        //check for trailing zeros
                        if (Culture.Name != "tr-TR")
                            if (beginsZero) word = " " + GetLocalizedValue("and") + " " + word.Trim();
                    }

                    //ignore digit grouping names
                    if (!isTurkishHundredOrThousand & word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return word.Trim();
        }

        /// <summary>
        /// Tenses the specified digit.
        /// </summary>
        /// <param name="digit">The digit.</param>
        /// <returns></returns>
        static String Tens(string digit)
        {
            int digt = Convert.ToInt32(digit);
            string name = null;

            switch (digt)
            {
                case 10:
                    name = GetLocalizedValue("Ten");
                    break;

                case 11:
                    name = GetLocalizedValue("Eleven");
                    break;

                case 12:
                    name = GetLocalizedValue("Twelve");
                    break;

                case 13:
                    name = GetLocalizedValue("Thirteen");
                    break;

                case 14:
                    name = GetLocalizedValue("Fourteen");
                    break;

                case 15:
                    name = GetLocalizedValue("Fifteen");
                    break;

                case 16:
                    name = GetLocalizedValue("Sixteen");
                    break;

                case 17:
                    name = GetLocalizedValue("Seventeen");
                    break;

                case 18:
                    name = GetLocalizedValue("Eighteen");
                    break;

                case 19:
                    name = GetLocalizedValue("Nineteen");
                    break;

                case 20:
                    name = GetLocalizedValue("Twenty");
                    break;

                case 30:
                    name = GetLocalizedValue("Thirty");
                    break;

                case 40:
                    name = GetLocalizedValue("Fourty");
                    break;

                case 50:
                    name = GetLocalizedValue("Fifty");
                    break;

                case 60:
                    name = GetLocalizedValue("Sixty");
                    break;

                case 70:
                    name = GetLocalizedValue("Seventy");
                    break;

                case 80:
                    name = GetLocalizedValue("Eighty");
                    break;

                case 90:
                    name = GetLocalizedValue("Ninety");
                    break;

                default:
                    if (digt > 0)
                        name = Tens(digit.Substring(0, 1) + "0") + " " + Ones(digit.Substring(1));
                    break;
            }

            return name;
        }

        /// <summary>
        /// Oneses the specified digit.
        /// </summary>
        /// <param name="digit">The digit.</param>
        /// <returns></returns>
        static String Ones(string digit)
        {
            int digt = Convert.ToInt32(digit);
            string name = "";

            switch (digt)
            {
                case 1:
                    name = GetLocalizedValue("One");
                    break;

                case 2:
                    name = GetLocalizedValue("Two");
                    break;

                case 3:
                    name = GetLocalizedValue("Three");
                    break;

                case 4:
                    name = GetLocalizedValue("Four");
                    break;

                case 5:
                    name = GetLocalizedValue("Five");
                    break;

                case 6:
                    name = GetLocalizedValue("Six");
                    break;

                case 7:
                    name = GetLocalizedValue("Seven");
                    break;

                case 8:
                    name = GetLocalizedValue("Eight");
                    break;

                case 9:
                    name = GetLocalizedValue("Nine");
                    break;
            }

            return name;
        }

        /// <summary>
        /// Gets the localized value.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        static String GetLocalizedValue(string code)
        {
            if (Culture.Name == "en-US")
                return code;

            if (!LocalizedText.ContainsKey(code))
                return "";

            return LocalizedText[code][Culture.Name];
        }

        /// <summary>
        /// Prepares the localized value.
        /// </summary>
        static void PrepareLocalizedValue()
        {
            LocalizedText = new Dictionary<string, Dictionary<string, string>>();

            LocalizedText.Add("Only", new Dictionary<string, string>());
            LocalizedText["Only"].Add("tr-TR", "Yalnız");

            LocalizedText.Add("and", new Dictionary<string, string>());
            LocalizedText["and"].Add("tr-TR", "ve");

            LocalizedText.Add("One", new Dictionary<string, string>());
            LocalizedText["One"].Add("tr-TR", "Bir");

            LocalizedText.Add("Two", new Dictionary<string, string>());
            LocalizedText["Two"].Add("tr-TR", "İki");

            LocalizedText.Add("Three", new Dictionary<string, string>());
            LocalizedText["Three"].Add("tr-TR", "Üç");

            LocalizedText.Add("Four", new Dictionary<string, string>());
            LocalizedText["Four"].Add("tr-TR", "Dört");

            LocalizedText.Add("Five", new Dictionary<string, string>());
            LocalizedText["Five"].Add("tr-TR", "Beş");

            LocalizedText.Add("Six", new Dictionary<string, string>());
            LocalizedText["Six"].Add("tr-TR", "Altı");

            LocalizedText.Add("Seven", new Dictionary<string, string>());
            LocalizedText["Seven"].Add("tr-TR", "Yedi");

            LocalizedText.Add("Eight", new Dictionary<string, string>());
            LocalizedText["Eight"].Add("tr-TR", "Sekiz");

            LocalizedText.Add("Nine", new Dictionary<string, string>());
            LocalizedText["Nine"].Add("tr-TR", "Dokuz");

            LocalizedText.Add("Ten", new Dictionary<string, string>());
            LocalizedText["Ten"].Add("tr-TR", "On");

            LocalizedText.Add("Eleven", new Dictionary<string, string>());
            LocalizedText["Eleven"].Add("tr-TR", "On Bir");

            LocalizedText.Add("Twelve", new Dictionary<string, string>());
            LocalizedText["Twelve"].Add("tr-TR", "On İki");

            LocalizedText.Add("Thirteen", new Dictionary<string, string>());
            LocalizedText["Thirteen"].Add("tr-TR", "On Üç");

            LocalizedText.Add("Fourteen", new Dictionary<string, string>());
            LocalizedText["Fourteen"].Add("tr-TR", "On Dört");

            LocalizedText.Add("Fifteen", new Dictionary<string, string>());
            LocalizedText["Fifteen"].Add("tr-TR", "On Beş");

            LocalizedText.Add("Sixteen", new Dictionary<string, string>());
            LocalizedText["Sixteen"].Add("tr-TR", "On Altı");

            LocalizedText.Add("Seventeen", new Dictionary<string, string>());
            LocalizedText["Seventeen"].Add("tr-TR", "On Yedi");

            LocalizedText.Add("Eighteen", new Dictionary<string, string>());
            LocalizedText["Eighteen"].Add("tr-TR", "On Sekiz");

            LocalizedText.Add("Nineteen", new Dictionary<string, string>());
            LocalizedText["Nineteen"].Add("tr-TR", "On Dokuz");

            LocalizedText.Add("Twenty", new Dictionary<string, string>());
            LocalizedText["Twenty"].Add("tr-TR", "Yirmi");

            LocalizedText.Add("Thirty", new Dictionary<string, string>());
            LocalizedText["Thirty"].Add("tr-TR", "Otuz");

            LocalizedText.Add("Fourty", new Dictionary<string, string>());
            LocalizedText["Fourty"].Add("tr-TR", "Kırk");

            LocalizedText.Add("Fifty", new Dictionary<string, string>());
            LocalizedText["Fifty"].Add("tr-TR", "Elli");

            LocalizedText.Add("Sixty", new Dictionary<string, string>());
            LocalizedText["Sixty"].Add("tr-TR", "Altmış");

            LocalizedText.Add("Seventy", new Dictionary<string, string>());
            LocalizedText["Seventy"].Add("tr-TR", "Yetmiş");

            LocalizedText.Add("Eighty", new Dictionary<string, string>());
            LocalizedText["Eighty"].Add("tr-TR", "Seksen");

            LocalizedText.Add("Ninety", new Dictionary<string, string>());
            LocalizedText["Ninety"].Add("tr-TR", "Doksan");

            LocalizedText.Add("Hundred", new Dictionary<string, string>());
            LocalizedText["Hundred"].Add("tr-TR", "Yüz");

            LocalizedText.Add("Thousand", new Dictionary<string, string>());
            LocalizedText["Thousand"].Add("tr-TR", "Bin");

            LocalizedText.Add("Million", new Dictionary<string, string>());
            LocalizedText["Million"].Add("tr-TR", "Milyon");

            LocalizedText.Add("Billion", new Dictionary<string, string>());
            LocalizedText["Billion"].Add("tr-TR", "Milyar");

            LocalizedText.Add("Trillion", new Dictionary<string, string>());
            LocalizedText["Trillion"].Add("tr-TR", "Trilyon");

            LocalizedText.Add("Cents", new Dictionary<string, string>());
            LocalizedText["Cents"].Add("tr-TR", "Kuruş");
        }

        #endregion
    }

    public enum CurrencyCodes
    {
        /// <summary>
        /// </summary>
        [EnumDescription("TL")]
        TL,
        /// <summary>
        /// The usd
        /// </summary>
        [EnumDescription("USD")]
        USD,
        /// <summary>
        /// The eur
        /// </summary>
        [EnumDescription("EUR")]
        EUR
    }

    internal static class NumberFormatters
    {
        /// <summary>
        /// To the string with point.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        internal static String ToStringWithPoint(this Decimal number)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

            return number.ToString("0.00", nfi);
        }

        internal static String ToStringWithPoint(this Decimal number, int pointCount)
        {
            return number.ToString("n" + pointCount);
        }
    }
}
