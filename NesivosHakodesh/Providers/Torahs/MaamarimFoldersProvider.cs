using System;
using System.Collections.Generic;
using System.Linq;
using NesivosHakodesh.Core;
using NesivosHakodesh.Core.DB;
using NesivosHakodesh.Domain.Entities;
using NesivosHakodesh.Providers.Identity;
using NesivosHakodesh.Providers.Parsha;
using NesivosHakodesh.Providers.Utils;
using NesivosHakodesh.Providers.Utils.Api;

namespace NesivosHakodesh.Providers.Torahs
{
    public class MaamarimFoldersProvider
    {
        public static ProviderResponse GetFileStructure()
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();

                List<MaamarType> types = PermissionProvider.GetMaamarimTypes();

                //get count per type
                var countByType = db.Maamarim.Where(x => x.Status != MaamarimStatus.Deleted)
                                                .Where(x => types.Contains(x.Type.Value))
                                                .GroupBy(x => x.Type)
                                                .Select(x => new { Key = x.Key.Value, Count = x.Count() })
                                                .ToDictionary(x => x.Key);

                //get total count
                var totalCount = countByType.Values.Sum(x => x.Count);

                

                

                List<MaamarFolderView> folders = new List<MaamarFolderView>
                {
                    new MaamarFolderView
                    {
                        IsHome = true,
                        Text = "ראשי",
                        AllText = "כל הסוגים",
                        SearchType = SearchType.None,
                        TotalCount = totalCount,
                        Folders = new List<MaamarFolderView>()
                    }
                };


                if (types.Contains(MaamarType.PisguminKadishin))
                {
                    //get count per parsha and year
                    var countByParsheAndYear = db.Maamarim.Where(x => x.Status != MaamarimStatus.Deleted && x.Type == MaamarType.PisguminKadishin && !string.IsNullOrEmpty(x.Parsha) && !string.IsNullOrEmpty(x.Year))
                                                    .GroupBy(x => new { x.Parsha, x.Year })
                                                    .Select(x => new { Key = x.Key, Count = x.Count() })
                                                    .ToDictionary(x => x.Key);

                    //par
                    List<ParseBook> books = ParshaProvider.GetBooksWithParshe();

                    //years
                    List<string> years = ParshaProvider.GetYears();
                    years.Reverse();


                    folders[0].Folders.Add(new MaamarFolderView
                    {
                        Text = "פתגמין קדישין",
                        AllText = "כל המאמרים",
                        SearchType = SearchType.Types,
                        SearchValue = MaamarType.PisguminKadishin.ToString(),
                        TotalCount = countByType.ValueForkey(MaamarType.PisguminKadishin)?.Count ?? 0,
                        Folders = new List<MaamarFolderView>
                        {
                            new MaamarFolderView
                            {
                                Text = "לפי הפרשות",
                                AllText = "כל הפרשות",
                                TotalCount = countByParsheAndYear.Select(x => x.Value).Sum(x => x.Count),
                                Folders = books.ConvertAll(book => new MaamarFolderView
                                {
                                    Text = $"{book.Name}",
                                    AllText = "כל הפרשות",
                                    SearchType = SearchType.ParshaBook,
                                    SearchValue = book.Name,
                                    TotalCount = countByParsheAndYear.Where(x => book.Parshas.Contains(x.Key.Parsha)).Select(x => x.Value).Sum(x=> x.Count),
                                    Folders = book.Parshas.ConvertAll(parsha => new MaamarFolderView
                                    {
                                        Text = parsha,
                                        AllText = "כל השנים",
                                        SearchType = SearchType.Parshas,
                                        SearchValue = parsha,
                                        TotalCount = countByParsheAndYear.Where(x => x.Key.Parsha == parsha).Select(x => x.Value).Sum(x => x.Count),
                                        Folders = new List<MaamarFolderView>
                                        {
                                            new MaamarFolderView
                                            {
                                                Text = "לפי השנים",
                                                AllText = "כל השנים",
                                                TotalCount = countByParsheAndYear.Where(x => x.Key.Parsha == parsha).Select(x => x.Value).Sum(x => x.Count),
                                                Folders = years.ConvertAll(year => new MaamarFolderView
                                                {
                                                    Text = year,
                                                    SearchType = SearchType.Year,
                                                    SearchValue = year,
                                                    TotalCount = countByParsheAndYear.Where(x => x.Key.Year == year && x.Key.Parsha == parsha).Select(x => x.Value).Sum(x => x.Count)
                                                })
                                            }
                                        }
                                    })
                                }),
                            },
                            new MaamarFolderView
                            {
                                Text = "לפי השנים",
                                AllText = "כל השנים",
                                TotalCount = countByParsheAndYear.Select(x => x.Value).Sum(x => x.Count),
                                Folders = years.ConvertAll(year => new MaamarFolderView
                                {
                                    Text = year,
                                    AllText = "כל הפרשות",
                                    SearchType = SearchType.Year,
                                    SearchValue = year,
                                    TotalCount = countByParsheAndYear.Where(x => x.Key.Year == year).Select(x => x.Value).Sum(x => x.Count),
                                    Folders = books.ConvertAll(book => new MaamarFolderView
                                    {
                                        Text = $"{book.Name}",
                                        AllText = "כל הפרשות",
                                        SearchType = SearchType.ParshaBook,
                                        SearchValue = book.Name,
                                        TotalCount = countByParsheAndYear.Where(x => x.Key.Year == year && book.Parshas.Contains(x.Key.Parsha)).Select(x => x.Value).Sum(x=> x.Count),
                                        Folders = book.Parshas.ConvertAll(parsha => new MaamarFolderView
                                        {
                                            Text = parsha,
                                            AllText = "כל השנים",
                                            SearchType = SearchType.Parshas,
                                            SearchValue = parsha,
                                            TotalCount = countByParsheAndYear.Where(x => x.Key.Year == year && x.Key.Parsha == parsha).Select(x => x.Value).Sum(x => x.Count),
                                        })
                                    })
                                }),

                            },
                        }
                    });
                }

                if (types.Contains(MaamarType.BH_NoamSheikh))
                {
                    folders[0].Folders.Add(new MaamarFolderView
                    {
                        Text = "נועם שיח",
                        AllText = "כל הספרים",
                        SearchType = SearchType.Types,
                        SearchValue = MaamarType.BH_NoamSheikh.ToString(),
                        TotalCount = countByType.ValueForkey(MaamarType.BH_NoamSheikh)?.Count ?? 0,
                    });
                }
                if (types.Contains(MaamarType.BH_SichesKodesh))
                {
                    folders[0].Folders.Add(new MaamarFolderView
                    {
                        Text = "שיחות קודש",
                        AllText = "כל הספרים",
                        SearchType = SearchType.Types,
                        SearchValue = MaamarType.BH_SichesKodesh.ToString(),
                        TotalCount = countByType.ValueForkey(MaamarType.BH_SichesKodesh)?.Count ?? 0,
                    });
                }
                if (types.Contains(MaamarType.BH_BerchesKodesh))
                {
                    folders[0].Folders.Add(new MaamarFolderView
                    {
                        Text = "ברכות קודש והמסתעף",
                        AllText = "כל הספרים",
                        SearchType = SearchType.Types,
                        SearchValue = MaamarType.BH_BerchesKodesh.ToString(),
                        TotalCount = countByType.ValueForkey(MaamarType.BH_BerchesKodesh)?.Count ?? 0,
                    });
                }
                if (types.Contains(MaamarType.BH_MaameremProtem))
                {
                    folders[0].Folders.Add(new MaamarFolderView
                    {
                        Text = "מאמרים פרטיים",
                        AllText = "כל הספרים",
                        SearchType = SearchType.Types,
                        SearchValue = MaamarType.BH_MaameremProtem.ToString(),
                        TotalCount = countByType.ValueForkey(MaamarType.BH_MaameremProtem)?.Count ?? 0,
                    });
                }
                if (types.Contains(MaamarType.BH_SeperiKodesh))
                {
                    folders[0].Folders.Add(new MaamarFolderView
                    {
                        Text = "סיפורי קודש",
                        AllText = "כל הספרים",
                        SearchType = SearchType.Types,
                        SearchValue = MaamarType.BH_SeperiKodesh.ToString(),
                        TotalCount = countByType.ValueForkey(MaamarType.BH_SeperiKodesh)?.Count ?? 0,
                    });
                }

                if (types.Contains(MaamarType.HadrachosYesharos))
                {
                    folders[0].Folders.Add(new MaamarFolderView
                    {
                        Text = "הדרכות ישרות",
                        AllText = "כל הספרים",
                        SearchType = SearchType.Types,
                        SearchValue = MaamarType.HadrachosYesharos.ToString(),
                        TotalCount = countByType.ValueForkey(MaamarType.HadrachosYesharos)?.Count ?? 0,
                    });
                }

                if (types.Contains(MaamarType.Personals))
                {
                    folders[0].Folders.Add(new MaamarFolderView
                    {
                        Text = "הדרכות פרטיות",
                        AllText = "כל הספרים",
                        SearchType = SearchType.Types,
                        SearchValue = MaamarType.Personals.ToString(),
                        TotalCount = countByType.ValueForkey(MaamarType.Personals)?.Count ?? 0,
                    });
                }

                response.Data = folders;
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
