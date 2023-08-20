using NesivosHakodesh.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NesivosHakodesh.Providers.Utils
{
    public static class Util
    {

        public static List<string> GetWords(string input)
        {
            MatchCollection matches = Regex.Matches(input, @"\b[\w']*\b");

            var words = from m in matches.Cast<Match>()
                        where !string.IsNullOrEmpty(m.Value)
                        select TrimSuffix(m.Value);

            return words.ToList();
        }
        private static string TrimSuffix(string word)
        {
            int apostropheLocation = word.IndexOf('\'');
            if (apostropheLocation != -1)
            {
                word = word.Substring(0, apostropheLocation);
            }

            return word;
        }
        public static string GetEnumValue(Enum en)
        {
            System.Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(EnumValue), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((EnumValue)attrs[0]).Name;
                }
            }

            return en.ToString();
        }

        public static V ValueForkey<K, V>(this Dictionary<K, V> dictionary, K key)
        {
            if(dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }

            return default(V);
        }

        public static BaseEntity GetLastUpdatedObject(BaseEntity b, List<BaseEntity> b2 = null, List<BaseEntity> b3 = null)
        {
            List<BaseEntity> list = new List<BaseEntity> { b };

            if (b2 != null)
            {
                list = list.Concat(b2).ToList();
            }

            if (b3 != null)
            {
                list = list.Concat(b3).ToList();
            }

            //last created
            BaseEntity lastCreated = list.OrderByDescending(x => x.CreatedTime).FirstOrDefault();

            //last modifed
            BaseEntity lastUpdated = list.Where(x => x.UpdatedTime.HasValue).OrderByDescending(x => x.UpdatedTime).FirstOrDefault();

            //get last from both
            if (lastUpdated != null && lastUpdated.UpdatedTime > lastCreated.CreatedTime)
            {
                return lastUpdated;
            }
            else
            {
                return new BaseEntity
                {
                    UpdatedTime = lastCreated.CreatedTime,
                    UpdatedUser = lastCreated.CreatedUser
                };
            }
        }

        public static string FormatHebrew(int num)
        {
            if (num <= 0)
                throw new ArgumentOutOfRangeException();
            StringBuilder ret = new StringBuilder(new string('ת', num / 400));
            num %= 400;
            if (num >= 100)
            {
                ret.Append("קרש"[num / 100 - 1]);
                num %= 100;
            }
            switch (num)
            {
                // Avoid letter combinations from the Tetragrammaton
                case 16:
                    ret.Append("טז");
                    break;
                case 15:
                    ret.Append("טו");
                    break;
                default:
                    if (num >= 10)
                    {
                        ret.Append("יכלמנסעפצ "[num / 10 - 1]);
                        num %= 10;
                    }
                    if (num > 0)
                        ret.Append("אבגדהוזחט"[num - 1]);
                    break;
            }
            return ret.ToString();
        }

        public static string RemovingVowels(string rawString)
        {
            rawString = StripHTML(rawString);
            var newString = "";
            rawString = rawString.Replace(@"\", "");
            rawString = rawString.Replace("־", " ");
            rawString = rawString.Replace("&lt;", " ");
            rawString = rawString.Replace("&gt;", " ");
            //rawString = Regex.Replace(rawString, @"\", String.Empty);
            // rawString = Regex.Replace(rawString, "&lt", String.Empty);
           // rawString = Regex.Replace(rawString, "&gt", String.Empty);


            for (int i = 0; i < rawString.Length; i++)
            {
               
                if (((int)rawString[i]) < 1425 || ((int)rawString[i]) > 1479)
                {
                    newString += rawString[i].ToString();
                }
            }
            return newString;
        }
        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }



        public static List<string> SplitOnFullWords(string input, string split)
        {
            List<string> result = new List<string>();

            if(string.IsNullOrEmpty(input))
            {
                return result;
            }

            int firstIndexOfSplit = input.IndexOf(split);

            // we have found an occurence of the split string, remove everything after this.
            if (firstIndexOfSplit >= 0)
            {
                string splitString = input.Substring(0, firstIndexOfSplit);

                // Find the last occurance of a space before this index; this will give us all full words before 
                int lastIndexOfSpace = splitString.LastIndexOf(' ');

                // If there are no sapces before this word then just add it and try for more
                if (lastIndexOfSpace >= 0)
                {
                    // Add the words before the word containing the splitter string
                    //result.Add(splitString.Substring(0, lastIndexOfSpace));

                    // Add the word contianing the splitter string
                    string remainingString = input.Substring(lastIndexOfSpace + 1);
                    int firstSpaceAfterWord = remainingString.IndexOf(' ');

                    if (firstSpaceAfterWord >= 0)
                    {
                        result.Add(remainingString.Substring(0, firstSpaceAfterWord));

                        // Look for more after the word containing the splitter string
                        string finalString = remainingString.Substring(firstSpaceAfterWord + 1);
                        result.AddRange(SplitOnFullWords(finalString, split));
                    }
                    else
                    {
                        result.Add(remainingString);
                    }
                }
                else
                {
                    // Add the word contianing the splitter string
                    int firstSpaceAfterWord = input.IndexOf(' ');

                    if (firstSpaceAfterWord >= 0)
                    {
                        result.Add(input.Substring(0, firstSpaceAfterWord));

                        // Look for more after the word containing the splitter string
                        string finalString = input.Substring(firstSpaceAfterWord + 1);
                        result.AddRange(SplitOnFullWords(finalString, split));
                    }
                    else
                    {
                        result.Add(input);
                    }
                }
            }
            else
            {
                // No occurences of the split string, just return the input
               //result.Add(input);
            }

            return result;
        }
    }
}
