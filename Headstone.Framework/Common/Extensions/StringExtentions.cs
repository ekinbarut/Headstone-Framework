using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Headstone.Framework.Common.Extensions
{
    public static class StringExtentions
    {

        /// <summary>
        /// Extention of string to make a paskal or camel case string to a logical text. 
        /// for example("ThisIsASample1Text => This Is A Sample 1 Text)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToCaseWords(this string value)
        {
            Regex r = new Regex(@"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
            return r.Replace(value, " ");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static Byte[] ToByteArray(this string value, Encoding encoding)
        {
            Byte[] byteArray = encoding.GetBytes(value);
            return byteArray;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static String ToString(this Byte[] value, Encoding encoding)
        {
            String constructedString = encoding.GetString(value);
            return (constructedString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) { return value; }
            return value.Substring(0, Math.Min(value.Length, maxLength));
        }

        /// <summary>
        /// String extesion to replace all Turkish characters with English ones
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ReplaceAllTurkishChars(this string value)
        {
            String[] olds = { "Ğ", "ğ", "Ü", "ü", "Ş", "ş", "İ", "ı", "Ö", "ö", "Ç", "ç" };
            String[] news = { "G", "g", "U", "u", "S", "s", "I", "i", "O", "o", "C", "c" };

            for (int i = 0; i < olds.Length; i++)
            {
                value = value.Replace(olds[i], news[i]);
            }

            return value;
        }

        public static string RemoveHtmlTags(this string source)
        {
            return Regex.Replace(string.Format("{0}", source), "<.*?>", string.Empty);
        }

        public static string ToSummary(this string source, int limit, string brackets)
        {
            return string.Format("{0}", source).Length > limit ? string.Format("{0} {1}", source.Substring(0, limit), brackets) : string.Format("{0}", source);
        }

        public static string ToValidurl(this string phrase)
        {
            string str = phrase.ReplaceAllTurkishChars().ToLower();
            // invalid chars, make into spaces
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces/hyphens into one space       
            str = Regex.Replace(str, @"[\s-]+", " ").Trim();

            // hyphens
            str = Regex.Replace(str, @"\s", "-");

            return str;
        }

        public static List<int> ToIntegerList(this string value, char[] seperator)
        {
            // Create a default return collection
            var response = new List<int>();

            // Split the value
            var strListItems = value.Split(seperator, StringSplitOptions.RemoveEmptyEntries).ToList();

            // Try to convert each item to integer
            strListItems.ForEach(i =>
            {
                int res = 0;
                if (int.TryParse(i, out res)) response.Add(res);
            });

            return response;
        }

    }
}
