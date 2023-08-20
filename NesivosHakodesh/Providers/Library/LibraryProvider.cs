using Microsoft.EntityFrameworkCore;
using NesivosHakodesh.Comman;
using NesivosHakodesh.Core;
using NesivosHakodesh.Core.DB;
using NesivosHakodesh.Domain.Entities;
using NesivosHakodesh.Providers.Utils;
using NesivosHakodesh.Providers.Utils.Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NesivosHakodesh.Providers.Library
{
    public class LibraryProvider
    {
        public static ProviderResponse GetAllLibrary(SearchCriteria search)
        {
            ProviderResponse response = new ProviderResponse();
            var listRes = new ListResData<Domain.Entities.Library>(search);
            response.Data = listRes;


            try
            {
                var LibrarySearch = AppProvider.GetDBContext().Library
                    .Where(x => search.LibraryCategory == null || !search.LibraryCategory.Any() || search.LibraryCategory.Contains(x.Category))
                    .Where(x => search.LibraryType == null || !search.LibraryType.Any() || search.LibraryType.Contains(x.Type))
                    .Where(x => search.LibrarySection == null || !search.LibrarySection.Any() || search.LibrarySection.Contains(x.Section))
                    .Where(x => search.LibraryChepter == null || !search.LibraryChepter.Any() || search.LibraryChepter.Contains(x.Chepter));

                if (!string.IsNullOrEmpty(search.SearchTerm))
                {
                    search.SearchTerm =  search.SearchTerm.Replace("ה'", "יהוה").Replace("אלקי", "אלהי");
                    List<string> words = Util.GetWords(search.SearchTerm);

                    foreach (var word in words)
                    {
                        LibrarySearch = LibrarySearch.Where(x =>
                       //x.OriginalFileName.Contains(word) ||
                       x.Text.Contains(word)
                       );
                    }

                    

                    
                }
                LibrarySearch =  LibrarySearch.OrderBy(x => x.SortBy);

                LibrarySearch = LibrarySearch.Skip(search.PageStartIndex).Take(search.ItemsPerPage);

                listRes.List = LibrarySearch.ToList();

                response.Data = listRes;
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }

        public static ProviderResponse ScrollLibrarydetails(SearchCriteria search)
        {
            ProviderResponse response = new ProviderResponse();
            var listRes = new ListResData<Domain.Entities.Library>(search);
            response.Data = listRes;


            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                var LibraryId = db.Library.Find(search.LibraryId);

                //add this if its the first chapter and click prev button
                if (search.ChepterId < 1)
                {
                    search.ChepterId = 1;
                }
                var Chepter = Util.FormatHebrew(search.ChepterId);
                var libraryResolt = db.Library.Where(x => x.Category == LibraryId.Category)
                                               .Where(x => x.Type == LibraryId.Type)
                                               .Where(x => x.Section == LibraryId.Section)
                                               .Where(x => x.Chepter == Chepter);
                                              // .Where(x =>   x.SortBy >= search.StartSort && x.SortBy <= search.EndSort);


                libraryResolt = libraryResolt.OrderBy(x => x.SortBy);

               // libraryResolt = libraryResolt.Skip(search.PageStartIndex).Take(search.ItemsPerPage);

                listRes.List = libraryResolt.ToList();

                response.Data = listRes;
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }


        public static ProviderResponse GetLibrarydetails(SearchCriteria search)
        {
            ProviderResponse response = new ProviderResponse();
         


            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                var LibraryId = db.Library.Find(search.LibraryId);

                var minId = LibraryId.SortBy - 9;
                var maxId = LibraryId.SortBy + 10;
                var libraryResolt = db.Library.Where(x => x.Category == LibraryId.Category)
                                               .Where(x => x.Type == LibraryId.Type)
                                               .Where(x => x.Section == LibraryId.Section)
                                               
                                               //.Where(x => x.Chepter == LibraryId.Chepter )
                                               .Where(x => x.SortBy >= minId )
                                               .Where(x => x.SortBy <= maxId)
                                               .OrderBy(x => x.SortBy).ToList();

                response.Data = libraryResolt;

            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }


        public static ProviderResponse GetCategoryOptions()
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                var sections = db.LibrarySections.OrderBy(x => x.Sort).ToList();

                List<string> cats = new List<string>();
                foreach (var item in sections)
                {
                    if (!cats.Contains(item.Category))
                        cats.Add(item.Category);
                }


                
                response.Data = cats;
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("An error occurred");
            }
            return response;
        }

        public static ProviderResponse GetTypeOptions(List<string> Category)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();

               // var type = db.Library.Where(x => Category.Contains(x.Category)).GroupBy(x => x.Type).Select(e => new { e.Key }).Where(x => x.Key != null).ToDictionary(e => e.Key).Keys.ToList();

                var sections = db.LibrarySections.Where(x => Category.Contains(x.Category)).OrderBy(x => x.Sort).ToList();


                
                List<string> cats = new List<string>();
                foreach (var item in sections)
                {
                    if (!cats.Contains(item.Type))
                        cats.Add(item.Type);
                }

                response.Data = cats;
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("An error occurred");
            }
            return response;
        }

        public static ProviderResponse GetSectionOptions(SearchCriteria search)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();

                // var Section = db.Library.Where(x => search.LibraryCategory.Contains(x.Category) && search.LibraryType.Contains(x.Type)).GroupBy(x => x.Section).Select(e => new { e.Key }).Where(x => x.Key != null).ToDictionary(e => e.Key).Keys.ToList();
                var sections = db.LibrarySections.Where(x => search.LibraryCategory.Contains(x.Category) && search.LibraryType.Contains(x.Type)).OrderBy(x => x.Sort).ToList();

                List<string> cats = new List<string>();
                foreach (var item in sections)
                {
                    if (!cats.Contains(item.Section))
                        cats.Add(item.Section);
                }



                response.Data = cats;
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("An error occurred");
            }
            return response;
        }

        public static ProviderResponse GetChopterOptions(SearchCriteria search)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();

                var Chepter = db.Library.Where(x => search.LibraryCategory.Contains(x.Category) && search.LibraryType.Contains(x.Type) && search.LibrarySection.Contains(x.Section)).GroupBy(x => x.Chepter).Select(e => new { e.Key }).Where(x => x.Key != null).ToDictionary(e => e.Key).Keys.ToList();

                response.Data = Chepter;
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("An error occurred");
            }
            return response;
        }

        public static ProviderResponse AddToLibrary(LibraryObject LibraryObject)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();

                var Library = new List<Domain.Entities.Library>();
                var chap = 1;
                foreach (var Chapter in LibraryObject.text)
                {
                   
                   // var Ch = Util.FormatHebrew(chap);

                    var ver = 1;
                    foreach (var Verse in Chapter)
                    {
                        
                        var RemoveVowels = Util.RemovingVowels(Verse);
                        RemoveVowels = RemoveVowels.Replace("\n", "");
                        Library.Add(new Domain.Entities.Library
                        {
                            Category = LibraryObject.Category,
                            Type = LibraryObject.Type,
                            Section = LibraryObject.Section,
                            Chepter = Util.FormatHebrew(chap),
                            Verse = Util.FormatHebrew(ver),
                            Text = RemoveVowels,
                        });
                        ver ++;
                    }
                    chap ++;
                   
                }

                db.Library.AddRange(Library);
                db.SaveChanges();

                response.Data = Library;
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }

        public static ProviderResponse AddToLibraryTalmud(LibraryObject LibraryObject)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();

                var Library = new List<Domain.Entities.Library>();
                var Daf = 1;
                var Emod = 1;
                foreach (var Chapter in LibraryObject.text)
                {

                    // var Ch = Util.FormatHebrew(chap);

                  
                    var Line = 1;
                    foreach (var Verse in Chapter)
                    {

                        var RemoveVowels = Util.RemovingVowels(Verse);
                        RemoveVowels = RemoveVowels.Replace("\n", "");
                        Library.Add(new Domain.Entities.Library
                        {
                            Category = LibraryObject.Category,
                            Type = LibraryObject.Type,
                            Section = LibraryObject.Section,
                            Chepter = Util.FormatHebrew(Daf),
                            Verse = Util.FormatHebrew(Emod),
                            Text = RemoveVowels,
                            Line = Line,
                        });
                        Line++;
                    }

                    if (Emod == 2)
                    {
                        Daf++;
                        Emod = 1;
                    }
                    else
                    {
                        Emod = 2;
                    }
                   
                    

                }

                db.Library.AddRange(Library);
                db.SaveChanges();

                response.Data = Library;
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }

        public static ProviderResponse GetMaamarLinks(int LibraryId)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                List<MaamarLibraryLink> Maamarim = db.MaamarLibraryLinks.Where(x => x.Library.LibraryId == LibraryId)
                        .Include(x => x.Maamar).ToList();

                response.Data = Maamarim;
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }

        public static ProviderResponse DeleteMaamarLink(int Id)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                MaamarLibraryLink Delete = db.MaamarLibraryLinks.Where(x => x.MaamarLibraryLinkId == Id).FirstOrDefault();

                if (Delete != null)
                {
                    Delete.IsDeleted = true;
                }
                else
                {
                    response.Messages.Add("מאאמר לא חוקי");
                }

                db.SaveChanges();

            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }


    }
}
