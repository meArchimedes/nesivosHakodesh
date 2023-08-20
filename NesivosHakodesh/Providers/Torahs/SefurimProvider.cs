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
using Newtonsoft.Json;

namespace NesivosHakodesh.Providers.Torahs
{
    public class SefurimProvider
    {
        internal static ProviderResponse GetAllSefurim(SearchCriteria search)
        {
            ProviderResponse response = new ProviderResponse();
            ListResData<Sefer> listRes = new ListResData<Sefer>(search);
            response.Data = listRes;

            try
            {
                var SefurimSearch = AppProvider.GetDBContext().Sefurim
                    .Where(x => string.IsNullOrEmpty(search.SearchTerm) ||
                         x.Name.Contains(search.SearchTerm) ||
                         x.Description.Contains(search.SearchTerm));

                listRes.TotalCount = SefurimSearch.Count();

               
                //if (search.SortDirection == SortDirection.Ascending)
                //{

                //    SefurimSearch = SefurimSearch.OrderBy(x => EF.Property<object>(x, search.SortBy));  //.ThenBy(x => x.HebrewFirstName)
                   
                //}
                //else
                //{

                //    SefurimSearch = SefurimSearch.OrderByDescending(x => EF.Property<object>(x, search.SortBy));  //.ThenBy(x => x.HebrewFirstName)
                   
                //}

                SefurimSearch = SefurimSearch.Skip(search.PageStartIndex).Take(search.ItemsPerPage);

                listRes.List = SefurimSearch.Select(x => new Sefer { 
                    SeferID = x.SeferID,
                    Name = x.Name,
                    Description = x.Description,
                    Status = x.Status,
                    OutlineJson = x.OutlineJson,
                    AuthorSefer = x.AuthorSefer,
                    SeferDetails = x.SeferDetails,
                    PrintYear = x.PrintYear,
                    Torahs = new List<Torah>()
                }).ToList();
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }

        internal static ProviderResponse GetSefurimList()
        {
            var response = new ProviderResponse();
            
            try
            {
                var sefurim = AppProvider.GetDBContext().Sefurim
                    .Select(x => new { x.SeferID, x.Name })
                    .ToList();
            
                response.Data = sefurim;
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }

        internal static ProviderResponse GetSefer(int id) 
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                Sefer sefer = AppProvider.GetDBContext().Sefurim
                                            .Where(x => x.SeferID == id)

                                            .Include(x => x.Torahs)
                                                .ThenInclude(x => x.SeferLinks)
                                            .Include(x => x.Torahs)
                                                .ThenInclude(x => x.MaamarLinks)                                               
                                                    .ThenInclude(x => x.Maamar)
                                                        .ThenInclude(x => x.CreatedUser)
                                                        //.ThenInclude(x => x.MaamarParagraphs)
                                                    
                                            .FirstOrDefault();

                if (sefer != null)
                {
                    foreach (var torah in sefer.Torahs)
                    {
                        //TODO chagne to select, and do the filter at the select
                        //remove maamrim, that this user does not have access to
                        torah.MaamarLinks = torah.MaamarLinks.Where(x => PermissionProvider.HasPermissionToMaamar(x.Maamar, PermissionType.VIEW)).ToList();
                    }

                    response.Data = sefer;
                }
                else
                {
                    response.Messages.Add("ספר לא חוקי");
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }

        internal static ProviderResponse AddSefer(Sefer sefer)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                response.Messages.AddRange(ValidateSefer(null, sefer, false, db));

                if (response.Success)
                {

                    Sefer NewSefer = new Sefer
                    {
                        AuthorSefer = sefer.AuthorSefer,
                        Description = sefer.Description,
                        FileUrl = sefer.FileUrl,
                        Name = sefer.Name,
                       OutlineJson = sefer.OutlineJson,
                       PrintYear = sefer.PrintYear,
                       SeferDetails = sefer.SeferDetails,
                       

                    };
                    

                    db.Sefurim.Add(NewSefer);

                    db.SaveChanges();
                    response.Data = sefer;
                 }
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }

        internal static ProviderResponse UpdateSefer( Sefer sefer)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                Sefer currSefer = db.Sefurim.Where(x => x.SeferID == sefer.SeferID)
                                            .Include(x => x.Torahs)
                                                .ThenInclude(x => x.MaamarLinks)
                                                    .ThenInclude(x => x.Maamar)
                                                        .ThenInclude(x => x.CreatedUser)
                                                        //.ThenInclude(x => x.MaamarParagraphs)
                                            .FirstOrDefault();

                response.Messages.AddRange(ValidateSefer(currSefer, sefer, true, db));

                if (response.Success)
                {
                    currSefer.Name = sefer.Name;
                    currSefer.Description = sefer.Description;
                    currSefer.FileUrl = sefer.FileUrl;
                    currSefer.AuthorSefer = sefer.AuthorSefer;
                    currSefer.PrintYear = sefer.PrintYear;
                    currSefer.SeferDetails = sefer.SeferDetails;

                    //currSefer.OutlineJson = GetOutlineTest();

                    //
                    foreach (Torah torah in sefer.Torahs)
                    {
                        //what if new
                        Torah currTorah = currSefer.Torahs.Find(x => x.TorahID == torah.TorahID);

                        if(currTorah != null)
                        {
                            currTorah.Parsha = torah.Parsha;
                            currTorah.MaarahMakoim = torah.MaarahMakoim;
                            currTorah.Title = torah.Title;
                            currTorah.OriginalFileName = torah.OriginalFileName;
                            currTorah.Status = torah.Status;
                            currTorah.AnnX = torah.AnnX;
                            currTorah.AnnY = torah.AnnY;
                            currTorah.AnnWidth = torah.AnnWidth;
                            currTorah.AnnHeight = torah.AnnHeight;
                        }
                        else
                        {
                           
                            //insert
                            Torah newTo = new Torah
                            {   
                                Parsha = torah.Parsha,
                                Status = Status.Active,
                                MaarahMakoim = torah.MaarahMakoim,
                                Title = torah.Title,
                                OriginalFileName = torah.OriginalFileName,
                                AnnX = torah.AnnX,
                                AnnY = torah.AnnY,
                                AnnWidth = torah.AnnWidth,
                                AnnHeight = torah.AnnHeight,
                                AnnPageNumber = torah.AnnPageNumber,
                                MaamarLinks = new List<MaamarTorahLink>()
                            };

                            currSefer.Torahs.Add(newTo);
                        }
                    }

                    db.SaveChanges();

                    foreach (var torah in currSefer.Torahs)
                    {
                        //TODO chagne to select, and do the filter at the select
                        //remove maamrim, that this user does not have access to
                        torah.MaamarLinks = torah.MaamarLinks.Where(x => PermissionProvider.HasPermissionToMaamar(x.Maamar, PermissionType.VIEW)).ToList();
                    }

                    response.Data = currSefer;
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }

        private static string GetOutlineTest()
        {
            List<OutlineItem> items = new List<OutlineItem> { 
                new OutlineItem{ 
                    Title = "שער",
                    PageNumber = 1
                },
                new OutlineItem{
                    Title = "צעטיל קטן",
                    PageNumber = 10
                },
                new OutlineItem{
                    Title = "בראשית",
                    PageNumber = 14
                },
                new OutlineItem{
                    Title = "נח",
                    PageNumber = 17
                },new OutlineItem{
                    Title = "לך לך",
                    PageNumber = 21
                },
                new OutlineItem{
                    Title = "וירא",
                    PageNumber = 27
                },
                new OutlineItem{
                    Title = "חיי שרה",
                    PageNumber = 32
                },
                new OutlineItem{
                    Title = "תולדות",
                    PageNumber = 36
                },
                new OutlineItem{
                    Title = "ויצא",
                    PageNumber = 40
                },
                new OutlineItem{
                    Title = "וישלך",
                    PageNumber = 43
                }
            };

            return JsonConvert.SerializeObject(items);
        }

        internal static ProviderResponse DeleteSefer(int id)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();

                Sefer currSefer = db.Sefurim.Find(id);

                if (currSefer == null)
                {
                    response.Messages.Add("ספר לא חוקי");
                }
                else
                {
                    List<Torah> SefurimTorahs = db.Torahs.Where(x => x.Sefer.SeferID == currSefer.SeferID).ToList();
                    if (SefurimTorahs != null)
                    {
                        response.Messages.Add("אתה לא יכול למחוק ספר שקשר לתורה ");
                    }
                    currSefer.Status = SefurimStatus.Deleted;
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }

        private static List<string> ValidateSefer(Sefer currSefer, Sefer Sefer, bool update,AppDBContext db)
        {
            List<string> errors = new List<string>();
            if (Sefer == null)
            {
                errors.Add("ספר לא חוקי");

            }
            else if (update && currSefer == null)
            {
                errors.Add("מזהה ספר לא חוקי");
            }
            else
            {
                if (string.IsNullOrEmpty(Sefer.Name))
                {
                    errors.Add("חסר שם");
                }
              
                //else
                //{
                //    if (string.IsNullOrEmpty(Sefer.Description))
                //    {
                //        errors.Add("Missing Description");
                //    }
                //}
            }
            return errors;
        }
    }
}
