using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NesivosHakodesh.Comman;
using NesivosHakodesh.Core;
using NesivosHakodesh.Core.DB;
using NesivosHakodesh.Domain.Entities;
using NesivosHakodesh.Providers.Identity;
using NesivosHakodesh.Providers.Utils;
using NesivosHakodesh.Providers.Utils.Api;

namespace NesivosHakodesh.Providers.Torahs
{
    public class MaamarimProvider
    {
        internal static ProviderResponse GetAllMaamarim(SearchCriteria search)
        {
            var response = new ProviderResponse();
            var listRes = new ListResData<Maamar>(search);
            response.Data = listRes;

            try
            {
                PermissionProvider.PopulateMaamarimSearchBasedOnUserRoles(search);

                var maamarimSearch = AppProvider.GetDBContext().Maamarim

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
                    var phrases = Util.GetWords(search.SearchTerm);

                    foreach (var phrase in phrases)
                    {
                        maamarimSearch = maamarimSearch.Where(x =>
                         x.Title.Contains(phrase) ||
                         x.MaamarID.ToString().Contains(phrase) ||
                         x.Content.Contains(phrase) ||
                         x.Parsha.Contains(phrase) ||
                         x.Year.Contains(phrase) ||
                         x.LocationDetails.Contains(phrase) ||
                         x.BechatzrPrintedWeek.Contains(phrase) ||
                         x.Source.FirstName.Contains(phrase) ||
                         x.Topic.Name.Contains(phrase) ||
                         x.AccuracyDescriptin.Contains(phrase) ||
                         x.SubTopics.Any(x => x.Topic.Category.CategoryName.Contains(phrase)) ||
                         x.SubTopics.Any(x => x.Topic.Name.Contains(phrase)));
                    }
                }

                listRes.TotalCount = maamarimSearch.Count();

                if (search.SortDirection == SortDirection.Ascending)
                {
                    if (search.SortBy == "Topic.Name")
                    {
                        maamarimSearch = maamarimSearch.OrderBy(x => x.Topic.Name);
                    }
                    else if (search.SortBy == "Source.FirstName")
                    {
                        maamarimSearch = maamarimSearch.OrderBy(x => x.Source.FirstName);
                    }
                    else if (search.SortBy == "LiberySource")
                    {
                        maamarimSearch = maamarimSearch.OrderBy(x => x.TitleLibraryId.SortBy);
                    }
                    else
                    {
                        maamarimSearch = maamarimSearch.OrderBy(x => EF.Property<object>(x, search.SortBy));  //.ThenBy(x => x.HebrewFirstName)
                    }
                }
                else
                {
                    if (search.SortBy == "Topic.Name")
                    {
                        maamarimSearch = maamarimSearch.OrderByDescending(x => x.Topic.Name);
                    }
                    else if (search.SortBy == "Source.FirstName")
                    {
                        maamarimSearch = maamarimSearch.OrderByDescending(x => x.Source.FirstName);
                    }
                    else if (search.SortBy == "LiberySource")
                    {
                        maamarimSearch = maamarimSearch.OrderByDescending(x => x.TitleLibraryId.SortBy);
                    }
                    else
                    {
                        maamarimSearch = maamarimSearch.OrderByDescending(x => EF.Property<object>(x, search.SortBy));  //.ThenBy(x => x.HebrewFirstName)
                    }
                }
                //order by
                //  MaamarimSearch = MaamarimSearch.OrderBy(x => x.Year).ThenBy(x => x.WeeklyIndex);

                maamarimSearch = maamarimSearch.Skip(search.PageStartIndex).Take(search.ItemsPerPage);

                listRes.List = maamarimSearch.Select(x => new Maamar
                {
                    MaamarID = x.MaamarID,
                    Title = x.Title,
                    Topic = x.Topic,
                    SubTopics = x.SubTopics.Select(st => new MaamarTopic
                    {
                        MaamarTopicID = st.MaamarTopicID,
                        Topic = new Topic
                        {
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
                    TitleLibraryId = x.TitleLibraryId,

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
                                                .Select(x => new Maamar
                                                {
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
                                                    TitleLibraryId = x.TitleLibraryId,
                                                    TorahLinks = x.TorahLinks.Select(tl => new MaamarTorahLink
                                                    {
                                                        MaamarTorahLinkID = tl.MaamarTorahLinkID,
                                                        CreatedTime = tl.CreatedTime,
                                                        CreatedUser = tl.CreatedUser,
                                                        UpdatedTime = tl.UpdatedTime,
                                                        UpdatedUser = tl.UpdatedUser,
                                                        Torah = new Torah
                                                        {
                                                            TorahID = tl.Torah.TorahID,
                                                            Title = tl.Torah.Title,
                                                            MaarahMakoim = tl.Torah.MaarahMakoim,
                                                            Parsha = tl.Torah.Parsha,
                                                            Index = tl.Torah.Index,
                                                            Sefer = new Sefer
                                                            {
                                                                SeferID = tl.Torah.Sefer.SeferID,
                                                                Name = tl.Torah.Sefer.Name,
                                                            }
                                                        }
                                                    }).ToList()
                                                })
                                                .FirstOrDefault();

                if (maamar != null)
                {
                    if (PermissionProvider.HasPermissionToMaamar(maamar, PermissionType.VIEW))
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
            var response = new ProviderResponse();

            try
            {
                var db = AppProvider.GetDBContext();
                response.Messages.AddRange(ValidateMaamarim(null, maamar, false, db));

                if (response.Success)
                {
                    var hasPermission = (PermissionProvider.HasPermissionToMaamar(maamar, PermissionType.EDIT));

                    if (hasPermission)
                    {
                        var newMaamar = new Maamar
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
                            TitleLibraryId = db.Library.Find(maamar.TitleLibraryId.LibraryId),                           
                            //MaamarParagraphs = maamar.MaamarParagraphs,
                            SubTopics = new List<MaamarTopic>(),
                        };

                        if (maamar.Source.SourceID == 0)
                        {
                            var source = new Source
                            {
                                FirstName = maamar.Source.FirstName,
                            };
                            db.Sources.Add(source);

                            newMaamar.Source = source;
                        }
                        else
                        {
                            var source = db.Sources.Find(maamar.Source.SourceID);
                            newMaamar.Source = source;
                        }

                        //add safer as main safer 11/17/2021
                        //foreach (MaamarTopic MaamarTopics in maamar.SubTopics)
                        //{
                        //    //insert

                        //  //  if add new topice(Safer)
                        //    if (MaamarTopics.Topic.TopicID == 0)
                        //    {
                        //       Topic topice = new Topic
                        //        {
                        //            Name = MaamarTopics.Topic.Name,
                        //            Category = db.Categories.Where(x => x.CategoryName == "ספרים").FirstOrDefault()


                        //        };

                        //        MaamarTopic newSubTopic = new MaamarTopic
                        //        {
                        //            Status = MaamarTopics.Status,
                        //            Topic = topice,
                        //            MainTopic = true,
                        //        };
                        //        newMaamar.SubTopics.Add(newSubTopic);
                        //    }
                        //    else
                        //    {
                        //       // topice = MaamarTopics.Topic;
                        //        MaamarTopic newSubTopic = new MaamarTopic
                        //        {
                        //            Status = MaamarTopics.Status,
                        //            Topic = db.Topics.Find(MaamarTopics.Topic.TopicID),
                        //            MainTopic = true,
                        //        };
                        //        newMaamar.SubTopics.Add(newSubTopic);
                        //    }                                                         
                        //}

                        //Add this to link Maamar Title to library;
                        var library = db.Library.Find(maamar.TitleLibraryId.LibraryId);
                        newMaamar.TitleLibraryId = library;

                        db.Maamarim.Add(newMaamar);
                        db.SaveChanges();

                        response.Data = newMaamar;
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
                           .Include(x => x.TitleLibraryId)
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

                        var source = db.Sources.Find(maamar.Source.SourceID);
                        currMaamar.Source = source;

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
                        //currMaamar.Topic = maamar.Topic;
                        currMaamar.Comments = maamar.Comments;

                        if (maamar.Title != maamar.TitleLibraryId.ParsedText)
                        {
                            if (currMaamar.TitleLibraryId != null)
                            {
                                var CurrMammarTitleLiberylink = db.MaamarLibraryLinks.Where(x => x.Maamar.MaamarID == currMaamar.MaamarID && x.Library.LibraryId == currMaamar.TitleLibraryId.LibraryId).FirstOrDefault();
                                if (CurrMammarTitleLiberylink != null)
                                {

                                    CurrMammarTitleLiberylink.IsDeleted = true;
                                }
                            }

                            currMaamar.TitleLibraryId = null;
                        }

                        else
                        {
                            if (currMaamar.TitleLibraryId != null)
                            {
                                var CurrMammarTitleLiberylink = db.MaamarLibraryLinks.Where(x => x.Maamar.MaamarID == currMaamar.MaamarID && x.Library.LibraryId == currMaamar.TitleLibraryId.LibraryId).FirstOrDefault();
                                if (CurrMammarTitleLiberylink != null)
                                {
                                    CurrMammarTitleLiberylink.Library = db.Library.Find(maamar.TitleLibraryId.LibraryId);
                                    CurrMammarTitleLiberylink.IsDeleted = false;
                                }
                            }
                            else
                            {
                                currMaamar.LibraryLink.Add(new MaamarLibraryLink
                                {
                                    Library = db.Library.Find(maamar.TitleLibraryId.LibraryId),
                                    IsDeleted = false,

                                });
                            }

                            currMaamar.TitleLibraryId = db.Library.Find(maamar.TitleLibraryId.LibraryId);
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
            catch (Exception e)
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
                    list.Add(new NameWithValue
                    {
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
            var maamarLibraryLinks = db.MaamarLibraryLinks.Where(x => x.Maamar.MaamarID == maamar.MaamarID)
                .Include(x => x.Library).ToList();

            foreach (var maamarLibraryLink in maamarLibraryLinks)
            {
                db.MaamarLibraryLinks.Remove(maamarLibraryLink);
            }

            var hrefIds = maamar.Content.Split(@"<a href=""library/");

            foreach (var hrefId in hrefIds)
            {
                if (!char.IsDigit(hrefId.First()))
                {
                    continue;
                }

                var index = hrefId.IndexOf('"');
                var id = hrefId.Substring(0, index);
                int libraryId = int.Parse(id);
           
                var maamarLibraryLink = new MaamarLibraryLink
                {
                    Library = db.Library.Find(libraryId),
                    Maamar = db.Maamarim.Find(maamar.MaamarID),
                };

                db.MaamarLibraryLinks.Add(maamarLibraryLink);
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

                    if (maamar.SubTopics.Count == 0)
                    {
                        //errors.Add("נא למלאות הענין");
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
