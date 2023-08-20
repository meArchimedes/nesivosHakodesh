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
    public class SourcesProvider
    {
        internal static ProviderResponse GetAllSources(SearchCriteria search)
        {
            ProviderResponse response = new ProviderResponse();
            ListResData<Source> listRes = new ListResData<Source>(search);
            response.Data = listRes;

            try
            {
                PermissionProvider.PopulateSourcesSearchBasedOnUserRoles(search);

                var SourcesSearch = AppProvider.GetDBContext().Sources
                    .Where(x => search.IncludePersonalSources || x.SourceType == SourceTypes.Maamarim);

                if (!string.IsNullOrEmpty(search.SearchTerm))
                {
                    List<string> words = Util.GetWords(search.SearchTerm);

                    foreach (string word in words)
                    {
                        // var SourceSearch = AppProvider.GetDBContext().Sources.

                        SourcesSearch = SourcesSearch.Where(x =>
                       x.FirstName.Contains(word) ||
                       x.Notes.Contains(word) ||
                       x.SourceDetails.Contains(word));


                        
                    }

                    

                }
                listRes.TotalCount = SourcesSearch.Count();
                if (search.SortDirection == SortDirection.Ascending)
                {

                    SourcesSearch = SourcesSearch.OrderBy(x => EF.Property<object>(x, search.SortBy));  //.ThenBy(x => x.HebrewFirstName)

                }
                else
                {

                    SourcesSearch = SourcesSearch.OrderByDescending(x => EF.Property<object>(x, search.SortBy));  //.ThenBy(x => x.HebrewFirstName)

                }

                SourcesSearch = SourcesSearch.Skip(search.PageStartIndex).Take(search.ItemsPerPage);
                listRes.List = SourcesSearch.Include(x => x.AssingedUser).ToList();

                response.Data = listRes;
            }
            

            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }


            return response;
        }

        internal static ProviderResponse GetSource(int id)
        {
            ProviderResponse response = new ProviderResponse();


            try
            {
                Source source = AppProvider.GetDBContext().Sources.Where(x => x.SourceID == id).Include(x => x.AssingedUser).FirstOrDefault();

                if (source != null)
                {
                    if(PermissionProvider.HasPermissionToSource(source, PermissionType.VIEW))
                    {
                        response.Data = source;
                    }
                    else
                    {
                        response.Messages.Add(PermissionProvider.Error());
                    }
                }
                else
                {
                    response.Messages.Add("מקור לא חוקי");
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }

        internal static ProviderResponse AddSource(Source Source)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                response.Messages.AddRange(ValidateSources(null, Source, false, db));

                if (response.Success)
                {
                    if (PermissionProvider.HasPermissionToSource(Source, PermissionType.EDIT))
                    {
                        db.Sources.Add(Source);

                        db.SaveChanges();
                        response.Data = Source;
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

        internal static ProviderResponse UpdateSource( Source Source)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                Source currSource = db.Sources.Find(Source.SourceID);

                response.Messages.AddRange(ValidateSources(currSource, Source, true, db));

                if (response.Success)
                {
                    if (PermissionProvider.HasPermissionToSource(currSource, PermissionType.EDIT))
                    {
                        currSource.FirstName = Source.FirstName;
                        //currSource.LastName = Source.LastName;
                        currSource.PhoneNumber = Source.PhoneNumber;
                        currSource.AssingedUser = Source.AssingedUser;
                        currSource.Notes = Source.Notes;
                        currSource.SourceDetails = Source.SourceDetails;
                        currSource.SourceType = Source.SourceType;

                        db.SaveChanges();
                        response.Data = currSource;
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

        internal static ProviderResponse DeleteSource(int id)
        {
            ProviderResponse response = new ProviderResponse();
            
            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                Source currSource = db.Sources.Find(id);

                if (currSource == null)
                {
                    response.Messages.Add("מקור לא חוקי");
                }
                else
                {
                    if (PermissionProvider.HasPermissionToSource(currSource, PermissionType.EDIT))
                    {
                        List<Maamar> SourceMaamarim = db.Maamarim.Where(x => x.Source.SourceID == currSource.SourceID).ToList();
                        if (SourceMaamarim.Count != 0)
                        {
                            response.Messages.Add("אינך יכול למחוק מקור שקושר");
                        }
                        else
                        {
                            currSource.Status = SourcesStatus.Deleted;
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
        private static List<string> ValidateSources(Source currSource, Source Source, bool update, AppDBContext db)
        {
            List<string> errors = new List<string>();

            if (Source == null)
            {
                errors.Add("מקור לא חוקי");
            }
            else if (update && currSource == null)
            {
                errors.Add("מזהה מקור לא חוקי");
            }
            else
            {
                if (Source.FirstName == null)
                {
                    errors.Add("חסר שם פרטי");
                }
                /*else if (Source.LastName == null)
                {
                    errors.Add("Missing Last Name");
                }*/

                if (Source.AssingedUser != null)
                {
                    Source.AssingedUser  = db.Users.Find(Source.AssingedUser.Id);
                    if (Source.AssingedUser == null)
                    {
                        errors.Add("משתמש שגוי");
                    }
                }
            }

            return errors;

        }
    }
}
