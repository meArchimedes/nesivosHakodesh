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

namespace NesivosHakodesh.Providers.Project
{
    public class ProjectUsersProvider
    {
        internal static ProviderResponse GetAllProjectUsers(SearchCriteria search)
        {
            ProviderResponse response = new ProviderResponse();
            ListResData<ProjectUser> listRes = new ListResData<ProjectUser>(search);
            response.Data = listRes;

            try
            {
                var ProjectUsersSearch = AppProvider.GetDBContext().ProjectUsers
                    .Where(x => string.IsNullOrEmpty(search.SearchTerm)).ToList();

                response.Data = ProjectUsersSearch;
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }

        internal static ProviderResponse GetProjectUser(int id)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                ProjectUser projectUser = AppProvider.GetDBContext().ProjectUsers.Where(x => x.ProjectUserID == id).Include(x => x.Project).Include(x => x.User).FirstOrDefault();

                if (projectUser != null)
                {
                    response.Data = projectUser;
                }
                else
                {
                    response.Messages.Add("משתמש פרוייקט לא חוקי");
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;

        }

        internal static ProviderResponse AddProjectUser(ProjectUser projectUser)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                response.Messages.AddRange(ValidateProjectUser(null, projectUser, false, db));

                if (response.Success)
                {
                    db.ProjectUsers.Add(projectUser);

                    db.SaveChanges();
                    response.Data = projectUser;
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }

        internal static ProviderResponse UpdateProjectUser(int id, ProjectUser projectUser)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                ProjectUser currProjectUser = db.ProjectUsers.Find(id);
                response.Messages.AddRange(ValidateProjectUser(currProjectUser, projectUser, true, db));

                if (response.Success)
                {
                    currProjectUser.Project = projectUser.Project;
                    currProjectUser.User = projectUser.User;
                    currProjectUser.Status = projectUser.Status;
                }
                db.SaveChanges();
                response.Data = currProjectUser;
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }

        internal static ProviderResponse DeleteProjectUser(int id)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();

                ProjectUser currProjectUser = db.ProjectUsers.Find(id);

                if (currProjectUser == null)
                {
                    response.Messages.Add("משתמש פרוייקט לא חוקי");
                }
                else
                {
                    currProjectUser.Status = ProjectUsersStatus.Deleted;
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


        private static List<string> ValidateProjectUser(ProjectUser currProjectUser, ProjectUser projectUser, bool update, AppDBContext db)
        {
            List<string> errors = new List<string>();

            if (projectUser == null)
            {
                errors.Add("משתמש פרוייקט לא חוקי");
            }
            else if (update && currProjectUser == null)
            {
                errors.Add("מזהה משתמש לא חוקי של הפרויקט");
            }
            else
            {
                if (projectUser.Project == null)
                {
                    errors.Add("חסר פרויקט");
                }
                else
                {
                    if (projectUser.User == null)
                    {
                        errors.Add("משתמש חסר");
                    }
                }
            }
            return errors;
        }
    }
}
