using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NesivosHakodesh.Comman;
using NesivosHakodesh.Core;
using NesivosHakodesh.Core.Config;
using NesivosHakodesh.Core.DB;
using NesivosHakodesh.Domain.Entities;
using NesivosHakodesh.Providers.Identity;
using NesivosHakodesh.Providers.Utils;
using NesivosHakodesh.Providers.Utils.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NesivosHakodesh.Providers.Torahs
{
    public class MaamarimProvider
    {
        internal static ProviderResponse GetAllMaamarim(SearchCriteria search)
        {
            ProviderResponse response = new ProviderResponse();
            ListResData<Maamar> listRes = new ListResData<Maamar>(search);
            response.Data = listRes;

            try
            {
                PermissionProvider.PopulateMaamarimSearchBasedOnUserRoles(search);

                var MaamarimSearch = AppProvider.GetDBContext().Maamarim
                
                   .Where(x => search.Type == null || !search.Type.Any() || search.Type.Contains(x.Type.Value))
                   .Where(x => search.Topic == null || !search.Topic.Any() || search.Topic.Contains(x.Topic.Name) || x.SubTopics.Any(s => search.Topic.Contains(s.Topic.Name)) )
                   .Where(x => search.Parsha == null || !search.Parsha.Any() || search.Parsha.Contains(x.Parsha))
                   .Where(x => search.Years == null || !search.Years.Any() || search.Years.Contains(x.Year))
                   .Where(x => search.Status == null || !search.Status.Any() || search.Status.Contains(x.Status))
                   .Where(x => search.Source == null || !search.Source.Any() || search.Source.Contains(x.Source.SourceID))
                   .Where(x => search.Sefurim == null || !search.Sefurim.Any() || x.TorahLinks.Any(s => search.Sefurim.Contains(s.Torah.Sefer.Name)) || x.TorahLinks.Any(e => e.Torah.SeferLinks.Any(s => search.Sefurim.Contains(s.Torah.Sefer.Name))) )
                   .Where(x => search.DateRange == null || !search.DateRange.HasValues() || (x.Date >= search.DateRange.StartDate.Value && x.Date <= search.DateRange.EndDate.Value))
                   .Where(x => search.CreatedUserId == null || !x.Locked || search.CreatedUserId.Value == x.CreatedUser.Id);

                if (!string.IsNullOrEmpty(search.SearchTerm))
                {
                    List<string> words = Util.GetWords(search.SearchTerm);

                    foreach (string word in words)
                    {
                      MaamarimSearch = MaamarimSearch.Where(x =>
                       //x.OriginalFileName.Contains(word) ||
                       x.Title.Contains(word) ||
                       x.MaamarID.ToString().Contains(word) ||
                       // x.Type.Value.Contains(search.SearchTerm) ||
                       x.Parsha.Contains(word) ||
                       x.Year.Contains(word) ||
                      // x.MaarahMakoim.Contains(word) ||
                       x.LocationDetails.Contains(word) ||
                       x.BechatzrPrintedWeek.Contains(word) ||
                       x.Source.FirstName.Contains(word) ||
                       x.Topic.Name.Contains(word) ||
                       x.AccuracyDescriptin.Contains(word) ||
                       x.SubTopics.Any(x => x.Topic.Category.CategoryName.Contains(word)) ||
                       x.SubTopics.Any(x => x.Topic.Name.Contains(word)));

                    }
                }
                     

                listRes.TotalCount = MaamarimSearch.Count();

                if (search.SortDirection == SortDirection.Ascending)
                {
                    if (search.SortBy == "Topic.Name")
                    {
                        MaamarimSearch = MaamarimSearch.OrderBy(x => x.Topic.Name);
                    }
                    else if (search.SortBy == "Source.FirstName")
                    {
                        MaamarimSearch = MaamarimSearch.OrderBy(x => x.Source.FirstName);
                    }
                    else if (search.SortBy == "LiberySource")
                    {
                        MaamarimSearch = MaamarimSearch.OrderBy(x => x.LiberyTitleId.SortBy);
                    }
                    else
                    {
                        MaamarimSearch = MaamarimSearch.OrderBy(x => EF.Property<object>(x, search.SortBy));  //.ThenBy(x => x.HebrewFirstName)
                    }
                  

                   
                }
                else
                {
                    if (search.SortBy == "Topic.Name")
                    {
                        MaamarimSearch = MaamarimSearch.OrderByDescending(x => x.Topic.Name);
                    }
                    else if (search.SortBy == "Source.FirstName")
                    {
                        MaamarimSearch = MaamarimSearch.OrderByDescending(x => x.Source.FirstName);
                    }
                    else if (search.SortBy == "LiberySource")
                    {
                        MaamarimSearch = MaamarimSearch.OrderByDescending(x => x.LiberyTitleId.SortBy);
                    }
                    else
                    {
                        MaamarimSearch = MaamarimSearch.OrderByDescending(x => EF.Property<object>(x, search.SortBy));  //.ThenBy(x => x.HebrewFirstName)
                    }
                   
                }
                //order by
              //  MaamarimSearch = MaamarimSearch.OrderBy(x => x.Year).ThenBy(x => x.WeeklyIndex);


                MaamarimSearch = MaamarimSearch.Skip(search.PageStartIndex).Take(search.ItemsPerPage);

                listRes.List = MaamarimSearch.Select(x => new Maamar { 
                                                MaamarID = x.MaamarID,
                                                Title = x.Title,
                                                Topic = x.Topic,
                                                SubTopics = x.SubTopics.Select(st => new MaamarTopic {
                                                    MaamarTopicID = st.MaamarTopicID,
                                                    Topic = new Topic {
                                                      Name = st.Topic.Name,
                                                      Status = st.Topic.Status,
                                                      TopicID = st.Topic.TopicID,
                                                      Category = st.Topic.Category
                                                    },
                                                    MainTopic = st.MainTopic,
                                                    CategoryName = st.Topic.Category.CategoryName,
                                                }).ToList(),
                                           
                                                Type = x.Type,
                                                Date = x.Date,
                                                Parsha = x.Parsha,
                                                WeeklyIndex = x.WeeklyIndex,
                                                Year = x.Year,
                                                Source = x.Source,
                                                Status = x.Status,
                                                StatusDetails = x.StatusDetails,
                                                BechatzrPrinted = x.BechatzrPrinted,
                                                BechatzrPrintedWeek = x.BechatzrPrintedWeek,

                                                CreatedTime = x.CreatedTime,
                                                CreatedUser = x.CreatedUser,
                                                UpdatedTime = x.UpdatedTime,
                                                UpdatedUser = x.UpdatedUser,
                                                LocationDetails = x.LocationDetails,
                                                OriginalFileName = x.OriginalFileName,
                                                PdfFileName = x.PdfFileName,
                                                AudioFileName = x.AudioFileName,
                                                Content = x.Content,
                                                Comments = x.Comments,
                                                LiberyTitleId = x.LiberyTitleId,

                })
                                            .ToList();
                response.Data = listRes;

            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }


        internal static ProviderResponse GetMaamar(int id)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                Maamar maamar = AppProvider.GetDBContext().Maamarim.Where(x => x.MaamarID == id)
                                                .Select(x => new Maamar {
                                                    MaamarID = x.MaamarID,
                                                    Title = x.Title,
                                                    Topic = x.Topic,
                                                    SubTopics = x.SubTopics.Select(st => new MaamarTopic
                                                    {
                                                        MaamarTopicID = st.MaamarTopicID,
                                                        MainTopic = st.MainTopic,
                                                        Status = st.Status,
                                                        Topic = new Topic
                                                        {
                                                            TopicID = st.Topic.TopicID,
                                                            Name = st.Topic.Name,
                                                            Category = st.Topic.Category
                                                        }
                                                    }).ToList(),
                                                    Type = x.Type,
                                                    Date = x.Date,
                                                    Parsha = x.Parsha,
                                                    WeeklyIndex = x.WeeklyIndex,
                                                    Year = x.Year,
                                                    Source = x.Source,
                                                    Status = x.Status,
                                                    StatusDetails = x.StatusDetails,
                                                    CreatedTime = x.CreatedTime,
                                                    CreatedUser = x.CreatedUser,
                                                    UpdatedTime = x.UpdatedTime,
                                                    UpdatedUser = x.UpdatedUser,
                                                    LocationDetails = x.LocationDetails,
                                                    OriginalFileName = x.OriginalFileName,
                                                    PdfFileName = x.PdfFileName,
                                                    AudioFileName = x.AudioFileName,
                                                    Content = x.Content,
                                                    BechatzrPrinted = x.BechatzrPrinted,
                                                    BechatzrPrintedWeek = x.BechatzrPrintedWeek,
                                                    Comments = x.Comments,
                                                    LiberyTitleId = x.LiberyTitleId,
                                                    TorahLinks = x.TorahLinks.Select(tl => new MaamarTorahLink {
                                                        MaamarTorahLinkID = tl.MaamarTorahLinkID,
                                                        CreatedTime = tl.CreatedTime,
                                                        CreatedUser = tl.CreatedUser,
                                                        UpdatedTime = tl.UpdatedTime,
                                                        UpdatedUser = tl.UpdatedUser,
                                                        Torah = new Torah { 
                                                            TorahID = tl.Torah.TorahID,
                                                            Title = tl.Torah.Title,
                                                            MaarahMakoim = tl.Torah.MaarahMakoim,
                                                            Parsha = tl.Torah.Parsha,
                                                            Index = tl.Torah.Index,
                                                            Sefer = new Sefer { 
                                                                SeferID = tl.Torah.Sefer.SeferID,
                                                                Name = tl.Torah.Sefer.Name,
                                                            }
                                                        }
                                                    }).ToList()
                                                })
                                                .FirstOrDefault();

                if (maamar != null) 
                {
                    if(PermissionProvider.HasPermissionToMaamar(maamar, PermissionType.VIEW))
                    {
                        //maamarim.MaamarParagraphs = maamarim.MaamarParagraphs.OrderBy(x => x.Sort).ToList();
                        maamar.LastUpdatedObject = Util.GetLastUpdatedObject(maamar, null, maamar.TorahLinks?.ToList<BaseEntity>());
                        response.Data = maamar;
                    }
                    else
                    {
                        response.Messages.Add(PermissionProvider.Error());
                    }
                }
                else
                {
                    response.Messages.Add("מאאמר לא חוקי");
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }

        internal static ProviderResponse AddMaamar(Maamar maamar)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                response.Messages.AddRange(ValidateMaamarim(null, maamar, false, db));

                if (response.Success)
                {
                    
                    if (PermissionProvider.HasPermissionToMaamar(maamar, PermissionType.EDIT))
                    {
                        Maamar NewMaamar = new Maamar
                        {
                           // Source = maamar.Source,
                            Topic = maamar.Topic,
                            Type = maamar.Type,
                            Status = MaamarimStatus.NewWithOutDetails,
                            OriginalFileName = maamar.OriginalFileName,
                            PdfFileName = maamar.PdfFileName,
                            AudioFileName = maamar.AudioFileName,
                            Title = maamar.Title,
                            Parsha = maamar.Parsha,
                            Year = maamar.Year,
                            WeeklyIndex = maamar.WeeklyIndex,
                            LocationDetails = maamar.LocationDetails,
                            Date = maamar.Date,
                            BechatzrPrinted = maamar.BechatzrPrinted,
                            BechatzrPrintedWeek = maamar.BechatzrPrintedWeek,
                            Comments = maamar.Comments,
                            LiberyTitleId = db.Library.Find(maamar.LiberyTitleId.LibraryId),
                            
                            //MaamarParagraphs = maamar.MaamarParagraphs,
                            SubTopics = new List<MaamarTopic>(),
                        };



                        //add new source  11/18/2021

                        if (maamar.Source.SourceID == 0)
                        {
                            Source NewSource = new Source
                            {
                                FirstName = maamar.Source.FirstName,
                            };
                            db.Sources.Add(NewSource);

                            NewMaamar.Source = NewSource;
                        }
                        else
                        {
                            NewMaamar.Source = maamar.Source;
                        }



                        //add safer as main safer 11/17/2021
                        foreach (MaamarTopic MaamarTopics in maamar.SubTopics)
                        {
                            //insert

                            

                          //  if add new topice(Safer)
                            if (MaamarTopics.Topic.TopicID == 0)
                            {
                               Topic topice = new Topic
                                {
                                    Name = MaamarTopics.Topic.Name,
                                    Category = db.Categories.Where(x => x.CategoryName == "ספרים").FirstOrDefault()
                                    

                                };

                                MaamarTopic newSubTopic = new MaamarTopic
                                {
                                    Status = MaamarTopics.Status,
                                    Topic = topice,
                                    MainTopic = true,
                                };
                                NewMaamar.SubTopics.Add(newSubTopic);
                            }
                            else
                            {
                               // topice = MaamarTopics.Topic;
                                MaamarTopic newSubTopic = new MaamarTopic
                                {
                                    Status = MaamarTopics.Status,
                                    Topic = db.Topics.Find(MaamarTopics.Topic.TopicID),
                                    MainTopic = true,
                                };
                                NewMaamar.SubTopics.Add(newSubTopic);
                            }

                            
                                
                            
                        }


                        db.Maamarim.Add(NewMaamar);


                        //Add this to link Maamar Title to library;
                        MaamarLibraryLink maamarLibraryLink = new MaamarLibraryLink
                        {
                            Library = db.Library.Find(maamar.LiberyTitleId.LibraryId),
                            Maamar = NewMaamar,
                            IsTitle = true,

                        };
                        db.MaamarLibraryLinks.Add(maamarLibraryLink);


                        AddMaamarLibrarylink(maamar, db);

                        db.SaveChanges();
                        response.Data = NewMaamar;
                    }
                    else
                    {
                        response.Messages.Add(PermissionProvider.Error());
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }

        internal static ProviderResponse UpdateMaamar(Maamar maamar)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                Maamar currMaamar = db.Maamarim.Where(x => x.MaamarID == maamar.MaamarID)
                           .Include(x => x.Source)
                           .Include(x => x.Topic)
                           .Include(x => x.SubTopics)
                                .ThenInclude(x => x.Topic)
                           .Include(x => x.LiberyTitleId)
                           .Include(x => x.LibraryLink)
                           .FirstOrDefault();

                response.Messages.AddRange(ValidateMaamarim(currMaamar, maamar, true, db));

                AddMaamarLibrarylink(maamar, db);


                if (maamar.Content == null || maamar.Content == "")
                {
                    currMaamar.Status = MaamarimStatus.NewWithOutDetails;
                }
               else if (maamar.Content != null && maamar.Topic == null)
                {
                    currMaamar.Status = MaamarimStatus.NeedDetails;
                }

                else if (maamar.Topic != null && maamar.Content != null && currMaamar.Status == MaamarimStatus.NeedDetails || currMaamar.Status == MaamarimStatus.NewWithOutDetails)
                {
                    currMaamar.Status = MaamarimStatus.NotMuggedYet;
                }
                else
                {
                    currMaamar.Status = maamar.Status;
                }


                if (response.Success)
                {
                    if (PermissionProvider.HasPermissionToMaamar(currMaamar, PermissionType.EDIT))
                    {
                        currMaamar.Content = maamar.Content;
                        currMaamar.Source = maamar.Source;
                        currMaamar.Type = maamar.Type;
                        currMaamar.StatusDetails = maamar.StatusDetails;
                        currMaamar.OriginalFileName = maamar.OriginalFileName;
                        currMaamar.PdfFileName = maamar.PdfFileName;
                        currMaamar.AudioFileName = maamar.AudioFileName;
                        currMaamar.Title = maamar.Title;
                        currMaamar.Parsha = maamar.Parsha;
                        currMaamar.Year = maamar.Year;
                        currMaamar.WeeklyIndex = maamar.WeeklyIndex;
                        currMaamar.LocationDetails = maamar.LocationDetails;
                        currMaamar.Date = maamar.Date;
                        currMaamar.BechatzrPrinted = maamar.BechatzrPrinted;
                        currMaamar.BechatzrPrintedWeek = maamar.BechatzrPrintedWeek;
                        currMaamar.AccuracyRate = maamar.AccuracyRate;
                        currMaamar.AccuracyDescriptin = maamar.AccuracyDescriptin;
                        //currMaamar.Status = maamar.Status;
                        currMaamar.Topic = maamar.Topic;
                        currMaamar.Source = maamar.Source;
                        currMaamar.Comments = maamar.Comments;


                        if (maamar.Title != maamar.LiberyTitleId.ParsedText)
                        {
                            

                            if (currMaamar.LiberyTitleId != null)
                            {
                                var CurrMammarTitleLiberylink = db.MaamarLibraryLinks.Where(x => x.Maamar.MaamarID == currMaamar.MaamarID && x.Library.LibraryId == currMaamar.LiberyTitleId.LibraryId).FirstOrDefault();
                                if (CurrMammarTitleLiberylink != null)
                                {
                                   
                                    CurrMammarTitleLiberylink.IsDeleted = true;
                                }


                            }

                            currMaamar.LiberyTitleId = null;
                        }

                        else
                        {
                            if (currMaamar.LiberyTitleId != null)
                            {
                                var CurrMammarTitleLiberylink = db.MaamarLibraryLinks.Where(x => x.Maamar.MaamarID == currMaamar.MaamarID && x.Library.LibraryId == currMaamar.LiberyTitleId.LibraryId).FirstOrDefault();
                                if (CurrMammarTitleLiberylink != null)
                                {
                                    CurrMammarTitleLiberylink.Library = db.Library.Find(maamar.LiberyTitleId.LibraryId);
                                    CurrMammarTitleLiberylink.IsDeleted = false;
                                }


                            }
                            else
                            {
                                currMaamar.LibraryLink.Add(new MaamarLibraryLink
                                {
                                    Library = db.Library.Find(maamar.LiberyTitleId.LibraryId),
                                    IsDeleted = false,

                                });
                            }

                            currMaamar.LiberyTitleId = db.Library.Find(maamar.LiberyTitleId.LibraryId);
                        }

                    


                        foreach (MaamarTopic MaamarTopics in maamar.SubTopics)
                        {
                            //check if this contact already exists
                            MaamarTopic currMaamarTopic = null;

                            if (MaamarTopics.MaamarTopicID != 0)
                            {
                                currMaamarTopic = currMaamar.SubTopics.Find(x => x.MaamarTopicID == MaamarTopics.MaamarTopicID);
                            }

                            //update
                            if (currMaamarTopic != null)
                            {
                                currMaamarTopic.Status = MaamarTopics.Status;
                            }
                            //insert
                            else
                            {
                                MaamarTopic newSubTopic = new MaamarTopic
                                {
                                    Status = MaamarTopics.Status,
                                    Topic = db.Topics.Find(MaamarTopics.Topic.TopicID),
                                    MainTopic = MaamarTopics.MainTopic
                                };
                                currMaamar.SubTopics.Add(newSubTopic);
                            }
                        }

                        /*foreach (MaamarParagraph Paragraphs in maamar.MaamarParagraphs)
                        {
                            //check if this details already exists in currMaamar
                            MaamarParagraph currMaamarimParagraph = db.MaamarimParagraphs.Find(Paragraphs.MaamarParagraphID);

                            int index = 0;
                            //update
                            if (currMaamarimParagraph != null)
                            {
                                currMaamarimParagraph.Text = Paragraphs.Text;
                                currMaamarimParagraph.ParagraphType = Paragraphs.ParagraphType;
                                currMaamarimParagraph.Sort = index.ToString();
                                currMaamarimParagraph.Status = Paragraphs.Status;
                            }

                            //insert
                            else
                            {
                                currMaamarimParagraph = new MaamarParagraph
                                {
                                    Text = Paragraphs.Text,
                                    ParagraphType = Paragraphs.ParagraphType,
                                    Sort = index.ToString(),
                                    Status = Paragraphs.Status,
                                 };
                                currMaamar.MaamarParagraphs.Add(currMaamarimParagraph);
                            }
                            index++;

                        }*/
                    db.SaveChanges();
                    response.Data = currMaamar;
                    }
                    else
                    {
                        response.Messages.Add(PermissionProvider.Error());
                    }
                }
            }
            catch (Exception e )
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }

        public static ProviderResponse GetMaamarTypes()
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                List<NameWithValue> list = new List<NameWithValue>();

                foreach (var item in PermissionProvider.GetMaamarimTypes())
                {
                    list.Add(new NameWithValue { 
                        Name = item.ToString(),
                        TypeValue = Util.GetEnumValue(item)
                    });
                }

                response.Data = list;
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }

        public static ProviderResponse GetMaamarStatusOptions()
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                List<NameWithValue> list = new List<NameWithValue>();
                foreach (Enum x in Enum.GetValues(typeof(MaamarimStatus)))
                {
                    if (x.ToString() != "Deleted")
                    {
                        list.Add(new NameWithValue
                        {
                            Name = x.ToString(),
                            TypeValue = Util.GetEnumValue(x)
                        });
                    }
                   
                }

                response.Data = list;
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }

        internal static ProviderResponse DeleteMaamar(int id)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                Maamar currMaamar = db.Maamarim.Where(x => x.MaamarID == id).FirstOrDefault();



                if (currMaamar == null)
                {
                    response.Messages.Add("מאאמר לא חוקי");
                }
                else
                {
                    if (PermissionProvider.HasPermissionToMaamar(currMaamar, PermissionType.EDIT))
                    {
                        var maamarLinks = db.MaamarTorahLinks.Where(x => x.Maamar.MaamarID == currMaamar.MaamarID).ToList();

                        if (maamarLinks.Count != 0)
                        {
                            response.Messages.Add("אתה לא יכול למחות שם מאמר שקשור לתורה, קודם צריך למחות את הקשר לתורה");
                        }
                        else
                        {
                            currMaamar.Status = MaamarimStatus.Deleted;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        response.Messages.Add(PermissionProvider.Error());
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }

        public static void AddMaamarLibrarylink(Maamar maamar, AppDBContext db)
        {
            //List<int> maamarLibrariesLinks = db.MaamarLibraryLinks.Where(x => x.Maamar.MaamarID == maamar.MaamarID).Select(x => x.Library.LibraryId).ToList();

            List<MaamarLibraryLink> maamarLibrariesLinks = db.MaamarLibraryLinks.Where(x => x.Maamar.MaamarID == maamar.MaamarID)
                          .Where(x => x.IsTitle == false)
                         .Include(x => x.Library).ToList();




            List<string> LibraryId = new List<string>();

            foreach (string word in Util.SplitOnFullWords(maamar.Content, "href=\"library/"))
            {
                
                string toBeSearched = "href=\"library/";
                
                string ipaddr = word.Substring(word.IndexOf(toBeSearched) + toBeSearched.Length );
                string ipaddr2 = ipaddr.Substring(0, ipaddr.IndexOf("\""));

                int newId = Int32.Parse(ipaddr2);
                var link = maamarLibrariesLinks.Where(x => x.Library.LibraryId == newId).FirstOrDefault();

                if (link == null)
                {
                    MaamarLibraryLink maamarLibraryLink = new MaamarLibraryLink
                    {
                        Library = db.Library.Find(int.Parse(ipaddr2)),
                        Maamar = db.Maamarim.Find(maamar.MaamarID),
                        
                    };
                    db.MaamarLibraryLinks.Add(maamarLibraryLink);
                }

                LibraryId.Add(ipaddr2);


            }

            List<string> StringLinkId = maamarLibrariesLinks.ConvertAll<string>(x => x.Library.LibraryId.ToString());
            var isRemovedLinked = StringLinkId.Except(LibraryId).ToList();

            if (isRemovedLinked.Count != 0)
            {

                foreach (var RemovedLinked in isRemovedLinked)
                {
                   MaamarLibraryLink Delete = maamarLibrariesLinks.Where(x => x.Library.LibraryId == int.Parse(RemovedLinked)).FirstOrDefault();
                    Delete.IsDeleted = true;

                }
                
            }


        }

         private static List<string> ValidateMaamarim(Maamar currMaamar, Maamar maamar, bool update, AppDBContext db)
        {
            List<string> errors = new List<string>();

            if (maamar == null)
            {
                errors.Add("מאמר לא חוקי");
            }
            else if (update && currMaamar == null)
            {
                errors.Add("תעודת זהות ממאמר");
            }
            else
            {

                // 11/19/2021 take this off becose its not required 
                /*if (maamar.Source != null)
                {
                    if (maamar.Source.SourceID != 0)
                    {
                        maamar.Source = db.Sources.Find(maamar.Source.SourceID);
                        if (maamar.Source == null)
                        {
                            errors.Add("נא למלאות המקור");
                        }
                    }
                    else
                    {
                        if (maamar.Source.FirstName == null)
                        {
                            errors.Add("נא למלאות המקור");
                        }
                    }
                    
                }*/
                if (maamar.Topic != null)
                {
                    maamar.Topic = db.Topics.Find(maamar.Topic.TopicID);
                   
                    if (maamar.Topic == null)
                    {
                        errors.Add("נא למלאות הענין");
                    }
                }


                // take this out 11/17/2021 becose we wont to add new topices here.

                if (maamar.SubTopics != null)
                {

                    if (maamar.SubTopics.Count ==0)
                    {
                        errors.Add("נא למלאות הענין");
                    }
                    
                   /* foreach (MaamarTopic MaamarTopic in maamar.SubTopics)
                    {
                        if (MaamarTopic.Topic != null)
                        {
                            MaamarTopic.Topic = db.Topics.Find(MaamarTopic.Topic.TopicID);
                            if (MaamarTopic.Topic == null)
                            {
                                errors.Add("נא למלאות הענין");
                            }
                        }
                    }*/
                }

                if (maamar.Type == null || maamar.Type == MaamarType.None)
                {
                    errors.Add("נא למלאות הסוג");
                }
                if (string.IsNullOrEmpty(maamar.Title))
                {
                    errors.Add("נא למלאות הכותרת");
                }

            }

            return errors;
        }
    }
}
