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
    public class ProjectChaptersProvider
    {
        internal static ProviderResponse GetAllProjectChapters(SearchCriteria search)
        {
            ProviderResponse response = new ProviderResponse();
            ListResData<ProjectChapter> listRes = new ListResData<ProjectChapter>(search);
            response.Data = listRes;

            try
            {
                var ProjectChaptersSearch = AppProvider.GetDBContext().ProjectChapters
                    .Where(x => string.IsNullOrEmpty(search.SearchTerm) ||
                         x.Name.Contains(search.SearchTerm) ||
                         x.Description.Contains(search.SearchTerm)  ||
                         x.Title.Contains(search.SearchTerm) ||
                         x.SubTitle.Contains(search.SearchTerm));

                listRes.TotalCount = ProjectChaptersSearch.Count();
                ProjectChaptersSearch = ProjectChaptersSearch.Skip(search.PageStartIndex).Take(search.ItemsPerPage);
                listRes.List = ProjectChaptersSearch.ToList();
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }

        internal static ProviderResponse GetProjectChapter(int id)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                ProjectChapter ProjectChapter = AppProvider.GetDBContext().ProjectChapters.Where(x => x.ProjectChapterID == id).Include(x => x.ParentChapter).FirstOrDefault();

                if (ProjectChapter != null)
                {
                    response.Data = ProjectChapter;
                }
                else
                {
                    response.Messages.Add("פרק לא חוקי");
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }
        internal static ProviderResponse AddProjectChapter(ProjectChapter projectChapter)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                response.Messages.AddRange(ValidateProjectChapter(null, projectChapter, false, db));

                if (response.Success)
                {
                    db.ProjectChapters.Add(projectChapter);

                    db.SaveChanges();
                    response.Data = projectChapter;
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }
        internal static ProviderResponse UpdateProjectChapter(int id, ProjectChapter projectChapter)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                ProjectChapter currProjectChapter = db.ProjectChapters.Find(id);
                response.Messages.AddRange(ValidateProjectChapter(currProjectChapter, projectChapter, true, db));

                if (response.Success)
                {
                    currProjectChapter.Project = projectChapter.Project;
                    currProjectChapter.Name = projectChapter.Name;
                    currProjectChapter.ParentChapter = projectChapter.ParentChapter;
                    currProjectChapter.Title = projectChapter.Title;
                    currProjectChapter.SubTitle = projectChapter.SubTitle;
                    currProjectChapter.Description = projectChapter.Description;
                    currProjectChapter.Status = projectChapter.Status;
                }
                db.SaveChanges();
                response.Data = currProjectChapter;
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }
        internal static ProviderResponse DeleteProjectChapter(int id)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();

                ProjectChapter currProjectChapter = db.ProjectChapters.Find(id);

                if (currProjectChapter == null)
                {
                    response.Messages.Add("פרק לא חוקי");
                }
                else
                {
                    currProjectChapter.Status = ProjectChaptersStatus.Deleted;
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
        private static List<string> ValidateProjectChapter(ProjectChapter currProjectChapter, ProjectChapter ProjectChapter, bool update, AppDBContext db)
        {
            List<string> errors = new List<string>();

            if (ProjectChapter == null)
            {
                errors.Add("פרק פרויקט לא חוקי");
            }
            else if (update && currProjectChapter == null)
            {
                errors.Add("מזהה פרק פרויקט לא חוקי");
            }
            else
            {
                if (ProjectChapter.Project != null)
                {
                    ProjectChapter.Project = db.Projects.Find(ProjectChapter.Project.ProjectID);
                    if (ProjectChapter.Project == null)
                    {
                        errors.Add("הפניה לפרויקט לא חוקית");
                    }
                }
                if (string.IsNullOrEmpty(ProjectChapter.Name))
                {
                    errors.Add("חסר שם");
                }
                else
                {
                    if (string.IsNullOrEmpty(ProjectChapter.Title))
                    {
                        errors.Add("חסר כותרת");
                    }
                }
            }
            return errors;
        }
    }
}
