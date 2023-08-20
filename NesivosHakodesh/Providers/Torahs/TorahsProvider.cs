using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NesivosHakodesh.Comman;
using NesivosHakodesh.Core;
using NesivosHakodesh.Core.DB;
using NesivosHakodesh.Domain.Entities;
using NesivosHakodesh.Providers.Utils;
using NesivosHakodesh.Providers.Utils.Api;

namespace NesivosHakodesh.Providers.Torahs
{
    public class TorahsProvider
    {
        internal static ProviderResponse GetAllTorahs(SearchCriteria search)
        {
            ProviderResponse response = new ProviderResponse();
            ListResData<Torah> listRes = new ListResData<Torah>(search);
            response.Data = listRes;

            try
            {
                var TorahSearch = AppProvider.GetDBContext().Torahs
                    .Where(x => string.IsNullOrEmpty(search.SearchTerm) ||
                      x.MaarahMakoim.Contains(search.SearchTerm) ||
                      x.Title.Contains(search.SearchTerm) ||
                      x.Parsha.Contains(search.SearchTerm) ||
                      x.TorahID.ToString().Contains(search.SearchTerm) ||
                      x.Sefer.Name.Contains(search.SearchTerm))

                 .Where(x => search.Sefurim == null || !search.Sefurim.Any() || search.Sefurim.Contains(x.Sefer.Name))
                 .Where(x => search.Parsha == null || !search.Parsha.Any() || search.Parsha.Contains(x.Parsha));

             listRes.TotalCount = TorahSearch.Count();


                if (search.SortDirection == SortDirection.Ascending)
                {
                    if (search.SortBy == "Sefer")
                    {
                        TorahSearch = TorahSearch.OrderBy(x => x.Sefer.Name);
                       // TorahSearch = TorahSearch.OrderBy(x => x.TorahID);
                    }
                    
                    else
                    {
                        TorahSearch = TorahSearch.OrderBy(x => EF.Property<object>(x, search.SortBy));  //.ThenBy(x => x.HebrewFirstName)
                    }



                }
                else
                {
                    if (search.SortBy == "Sefer")
                    {
                        TorahSearch = TorahSearch.OrderByDescending(x => x.Sefer.Name);
                    }
                   
                    else
                    {
                        TorahSearch = TorahSearch.OrderByDescending(x => EF.Property<object>(x, search.SortBy));  //.ThenBy(x => x.HebrewFirstName)
                    }

                }

                TorahSearch = TorahSearch.Skip(search.PageStartIndex).Take(search.ItemsPerPage);

             /*listRes.List = TorahSearch
                    .Include(x => x.TorahParagraphs)
                    .Include(x => x.Sefer)
                    .ToList();*/

                listRes.List = TorahSearch
                    .Select(x => new Torah { 
                        TorahID = x.TorahID,
                        Title = x.Title,
                        Parsha = x.Parsha,
                        CreatedTime = x.CreatedTime,
                        UpdatedTime = x.UpdatedTime,
                        Sefer = new Sefer { 
                            SeferID = x.Sefer.SeferID,
                            Name = x.Sefer.Name,
                        },
                        MaarahMakoim = x.MaarahMakoim,
                    })
                    .ToList();

            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }


        /*internal static ProviderResponse GetTorah(int id)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                Torah torah = AppProvider.GetDBContext().Torahs.Where(x => x.TorahID == id)
                                  .Include(x => x.Sefer)
                                  .Include(x => x.TorahParagraphs)
                                  .Include(x => x.MaamarLinks)
                                        .ThenInclude(x => x.Maamar)
                                            //.ThenInclude(x => x.MaamarParagraphs)
                                 .FirstOrDefault();



                //  torah.TorahParagraphs = AppProvider.GetDBContext().TorahParagraphs.Where(x => x.To).Select(xx => new TorahParagraph { SortIndex = xx.SortIndex, Text = xx.Text, TorahParagraphID = xx.TorahParagraphID,  })
                //  .OrderBy(x => x.SortIndex).ToList();

                torah.TorahParagraphs = torah.TorahParagraphs.OrderBy(x => x.SortIndex).ToList();

                torah.LastUpdatedObject = Util.GetLastUpdatedObject(torah, torah.TorahParagraphs?.ConvertAll(x => (BaseEntity)x), torah.MaamarLinks?.ConvertAll(x => (BaseEntity)x));

                if (torah != null )
                {

                    response.Data = torah;
                }
                else
                {
                    response.Messages.Add("לא חוקי");
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }*/

        /*internal static ProviderResponse AddTorah(Models.Torah Torah)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                response.Messages.AddRange(ValidateTorah(null, Torah, false, db));

                if (response.Success)
                {
                    Torah NewTorah = new Torah
                    {
                        Parsha = Torah.Parsha,
                        MaarahMakoim = Torah.MaarahMakoim,
                        Sefer = db.Sefurim.Find(Torah.Sefer.SeferID),
                        Title = Torah.Title,
                    };
                    db.Torahs.Add(NewTorah);

                    db.SaveChanges();
                    response.Data = NewTorah;
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }*/

        /*internal static ProviderResponse UpdateTorah(Models.Torah Torah)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                Torah currTorah = db.Torahs.Where(x => x.TorahID == Torah.TorahID)
                                                .Include(x => x.TorahParagraphs)
                                                .Include(x => x.Sefer)
                                                .FirstOrDefault();

                response.Messages.AddRange(ValidateTorah(currTorah, Torah, true, db));

                if (response.Success)
                {
                    currTorah.Sefer = Torah.Sefer;
                    currTorah.Parsha = Torah.Parsha;
                    currTorah.MaarahMakoim = Torah.MaarahMakoim;
                    currTorah.Title = Torah.Title;
                    
                    currTorah.OriginalFileName = Torah.OriginalFileName;


                   
                    int index = 0;

                    foreach (TorahParagraph Paragraph in Torah.TorahParagraphs)
                    {
                        if (Paragraph.Text != null)
                        {
                            //check if this contact already exists
                            TorahParagraph currTorahParagraph = currTorah.TorahParagraphs.Where(x => x.TorahParagraphID == Paragraph.TorahParagraphID).FirstOrDefault();

                            //update
                            if (currTorahParagraph != null)
                            {
                                currTorahParagraph.Text = Paragraph.Text;
                                //currTorahParagraph.SortIndex = index.ToString();
                                currTorahParagraph.IsDeleted = Paragraph.IsDeleted;

                            }
                            //insert
                            else
                            {
                                currTorahParagraph = new TorahParagraph
                                {
                                    Text = Paragraph.Text,
                                    SortIndex = index.ToString(),
                                };

                                currTorah.TorahParagraphs.Add(currTorahParagraph);
                            }
                            index++;
                        }
                       
                    }

                }
                db.SaveChanges();
                response.Data = currTorah;
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }*/

        internal static ProviderResponse DeleteTorah(int id)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                Torah currTorah = db.Torahs.Find(id);

                if (currTorah == null)
                {
                    response.Messages.Add("לא חוקי");
                }
                else
                {

                    var TorahLinks = db.MaamarTorahLinks.Where(x => x.Torah.TorahID == currTorah.TorahID && !x.IsDeleted).ToList();

                    if (TorahLinks.Count >= 0)
                    {
                        response.Messages.Add("אתה לא יכול למחות שם מאמר שקשור לתורה, קודם צריך למחות את הקשר לתורה");
                    }
                    else
                    {
                        currTorah.Status = Status.Deleted;
                        db.SaveChanges();
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
        private static List<string> ValidateTorah(Domain.Entities.Torah currTorah, Domain.Entities.Torah Torah, bool update, AppDBContext db)
        {
            List<string> errors = new List<string>();

            if (Torah == null)
            {
                errors.Add("Invalid Torah");
            }
            else if (update && currTorah == null)
            {
                errors.Add("Invaild Torah ID");
            }
            else
            {
                if (Torah.Sefer == null)
                {
                    errors.Add("Missing Sefer");
                }
                else
                {
                    Torah.Sefer = db.Sefurim.Find(Torah.Sefer.SeferID);
                    if (Torah.Sefer == null)
                    {
                        errors.Add("Invalid Sefer");
                    }
                }
                // Need to fix ===       if (string.IsNullOrEmpty(Torah.Name))
                // Need to fix ===    {
                // Need to fix ===     errors.Add("Missing  Name");
                // Need to fix === }


                if (string.IsNullOrEmpty(Torah.Title))
                {
                    errors.Add("Missing Title");
                }

                if (string.IsNullOrEmpty(Torah.MaarahMakoim))
                {
                    //errors.Add("Missing Maarah Makoim");
                }
             }
            return errors;
        }

    }
}
