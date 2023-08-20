using Microsoft.AspNetCore.Mvc;
using NesivosHakodesh.Comman;
using NesivosHakodesh.Providers.Torahs;
using NesivosHakodesh.Providers.Utils;
using NesivosHakodesh.Providers.Utils.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NesivosHakodesh.Providers.Parsha
{

    public class ParshaProvider
    {
        internal static ProviderResponse GetAllParshas()
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                response.Data = GetParseList();
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }

        internal static ProviderResponse GetAllYears()
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                List<string> years = GetYears();
                years.Reverse();
                response.Data = years.ConvertAll(x => new ParseBook { Name = x });
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }

        internal static ProviderResponse GetAllParshas2()
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                response.Data = GetBooksWithParshe();
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }

        public static List<ParseBook> GetBooksWithParshe()
        {
            return new List<ParseBook>
            {
                new ParseBook  {
                    Name = "ספר בראשית",
                    Parshas = new List<string>
                    {
                        "בראשית",
                        "נח",
                        "לך לך",
                        "וירא",
                        "חיי שרה",
                        "תולדות",
                        "ויצא",
                        "וישלח",
                        "וישב",
                        "מקץ",
                        "ויגש",
                        "ויחי",
                    }
                },
                new ParseBook  {
                    Name = "ספר שמות",
                    Parshas = new List<string>
                    {
                        "שמות",
                        "וארא",
                        "בא",
                        "בשלח",
                        "יתרו",
                        "משפטים",
                        "תרומה",
                        "תצוה",
                        "כי תשא",
                        "ויקהל",
                        "פקודי",
                    }
                },
                new ParseBook  {
                    Name = "ספר ויקרא",
                    Parshas = new List<string>
                    {
                        "ויקרא",
                        "צו",
                        "שמיני",
                        "תזריע",
                        "מצרע",
                        "אחרי",
                        "קדשים",
                        "אמור",
                        "בהר",
                        "בחקתי",
                    }
                },
                new ParseBook  {
                    Name = "ספר במדבר",
                    Parshas = new List<string>
                    {
                       "במדבר",
                        "נשא",
                        "בהעלתך",
                        "שלח",
                        "קרח",
                        "חקת",
                        "בלק",
                        "פינחס",
                        "מטות",
                        "מסעי",
                    }
                },
                new ParseBook  {
                    Name = "ספר דברים",
                    Parshas = new List<string>
                    {
                        "דברים",
                        "ואתחנן",
                        "עקב",
                        "ראה",
                        "שופטים",
                        "כי תצא",
                        "כי תבוא",
                        "נצבים",
                        "וילך",
                        "האזינו",
                        "וזאת הברכה",
                    }
                },
            };
        }

        public static List<string> GetParseList()
        {
            return GetBooksWithParshe().SelectMany(x => x.Parshas).ToList();
            /*
            return new List<string>()
                {
                    "בראשית",
                    "נח",
                    "לך-לך",
                    "וירא",
                    "חיי שרה",
                    "תולדת",
                    "ויצא",
                    "וישלח",
                    "וישב",
                    "מקץ",
                    "ויגש",
                    "ויחי",
                    "שמות",
                    "וארא",
                    "בא",
                    "בשלח",
                    "יתרו",
                    "משפטים",
                    "תרומה",
                    "תצוה",
                    "כי תשא",
                    "ויקהל",
                     "פקודי",
                    "ויקרא",
                    "צו",
                    "שמיני",
                    "תזריע",
                    "מצרע",
                    "אחרי",
                    "קדשים",
                    "אמור",
                    "בהר",
                    "בחקתי",
                    "במדבר",
                    "נשא",
                    "בהעלתך",
                    "שלח",
                    "קרח",
                    "חקת",
                    "בלק",
                    "פינחס",
                    "מטות",
                    "מסעי",
                    "דברים",
                    "ואתחנן",
                    "עקב",
                    "ראה",
                    "שפטים",
                    "כי תצא",
                    "כי תבוא",
                    "נצבים",
                    "וילך",
                    "האזינו",
                    "וזאת הברכה",

                };*/
        }

        public static List<string> GetYears()
        {
            return new List<string> {
                "תש״א",
                "תש״ב",
                "תש״ג",
                "תש״ד",
                "תש״ה",
                "תש״ו",
                "תש״ז",
                "תש״ח",
                "תש״ט",
                "תש״י",
                "תשי״א",
                "תשי״ב",
                "תשי״ג",
                "תשי״ד",
                "תשט״ו",
                "תשט״ז",
                "תשי״ז",
                "תשי״ח",
                "תשי״ט",
                "תש״כ",
                "תשכ״א",
                "תשכ״ב",
                "תשכ״ג",
                "תשכ״ד",
                "תשכ״ה",
                "תשכ״ו",
                "תשכ״ז",
                "תשכ״ח",
                "תשכ״ט",
                "תש״ל",
                "תשל״א",
                "תשל״ב",
                "תשל״ג",
                "תשל״ד",
                "תשל״ה",
                "תשל״ו",
                "תשל״ז",
                "תשל״ח",
                "תשל״ט",
                "תש״מ",
                "תשמ״א",
                "תשמ״ב",
                "תשמ״ג",
                "תשמ״ד",
                "תשמ״ה",
                "תשמ״ו",
                "תשמ״ז",
                "תשמ״ח",
                "תשמ״ט",
                "תש״נ",
                "תשנ״א",
                "תשנ״ב",
                "תשנ״ג",
                "תשנ״ד",
                "תשנ״ה",
                "תשנ״ו",
                "תשנ״ז",
                "תשנ״ח",
                "תשנ״ט",
                "תש״ס",
                "תשס״א",
                "תשס״ב",
                "תשס״ג",
                "תשס״ד",
                "תשס״ה",
                "תשס״ו",
                "תשס״ז",
                "תשס״ח",
                "תשס״ט",
                "תש״ע",
                "תשע״א",
                "תשע״ב",
                "תשע״ג",
                "תשע״ד",
                "תשע״ה",
                "תשע״ו",
                "תשע״ז",
                "תשע״ח",
                "תשע״ט",
                "תש״פ",
                "תשפ״א",
                "תשפ״ב",
            };
        }
    }
}
