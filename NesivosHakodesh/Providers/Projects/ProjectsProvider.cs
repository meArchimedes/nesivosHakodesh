using System;
using System.Collections.Generic;
using System.Linq;
using NesivosHakodesh.Comman;
using NesivosHakodesh.Core;
using NesivosHakodesh.Core.DB;
using NesivosHakodesh.Domain.Entities;
using NesivosHakodesh.Providers.Utils;
using NesivosHakodesh.Providers.Utils.Api;

namespace NesivosHakodesh.Providers.Projects
{
    public class ProjectsProvider
    {
        internal static ProviderResponse GetAllProjects(SearchCriteria search)
        {
            ProviderResponse response = new ProviderResponse();
            var listRes = new ListResData<Domain.Entities.Project>(search);
            response.Data = listRes;

            try
            {
                var ProjectSearch = AppProvider.GetDBContext().Projects
                    .Where(x => string.IsNullOrEmpty(search.SearchTerm) ||
                    x.Name.Contains(search.SearchTerm));

                listRes.TotalCount = ProjectSearch.Count();
                ProjectSearch = ProjectSearch.Skip(search.PageStartIndex).Take(search.ItemsPerPage);
                listRes.List = ProjectSearch.ToList();
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }

       internal static ProviderResponse GetProject(int id)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                var project = AppProvider.GetDBContext().Projects.Where(x => x.ProjectID == id).FirstOrDefault();

                if (project != null)
                {
                    response.Data = project;
                }
                else
                {
                    response.Messages.Add("פרויקט לא חוקי");
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }

        internal static ProviderResponse AddProject(Domain.Entities.Project project)
        {
            ProviderResponse response = new ProviderResponse();
            
            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                response.Messages.AddRange(ValidateProject(null, project, false, db));

                if (response.Success)
                {
                    db.Projects.Add(project);

                    db.SaveChanges();
                    response.Data = project;
                }

            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }

        internal static ProviderResponse UpdateProject(int id, Domain.Entities.Project project)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                var currProject = db.Projects.Find(id);
                response.Messages.AddRange(ValidateProject(currProject, project, true, db));

                if (response.Success)
                {
                    currProject.Name = project.Name;
                    currProject.Status = project.Status;
                    currProject.StartDate = project.StartDate;
                    currProject.ExpectedDate = project.ExpectedDate;
                    currProject.FinishDate = project.FinishDate;
                    currProject.ProjectManager = project.ProjectManager;
                }
                db.SaveChanges();
                response.Data = currProject;
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }

        internal static ProviderResponse DeleteProject(int id)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();

                Domain.Entities.Project currProject = db.Projects.Find(id);

                if (currProject == null)
                {
                    response.Messages.Add("פרויקט לא חוקי");
                }
                else
                {
                    currProject.Status = ProjectsStatus.Deleted;
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

        private static List<string> ValidateProject(Domain.Entities.Project currProject, Domain.Entities.Project project, bool update, AppDBContext db)
        {
            List<string> errors = new List<string>();

            if (project == null)
            {
                errors.Add("פרויקט לא חוקי");
            }
            else if (update && currProject == null)
            {
                errors.Add("מזהה פרויקט לא חוקי");
            }
            else
            {
                if (string.IsNullOrEmpty(project.Name))
                {
                    errors.Add("חסר שם");
                }
                else
                {
                    if (project.StartDate == null)
                    {
                        errors.Add("תאריך התחלה חסר");
                    }
                }
            }

            return errors;
        }
    }
}
