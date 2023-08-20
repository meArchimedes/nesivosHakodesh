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
    public class ProjectAssignmentProvider
    {
        internal static ProviderResponse GetAllProjectAssignments(SearchCriteria search)
        {
            ProviderResponse response = new ProviderResponse();
            ListResData<ProjectAssignment> listRes = new ListResData<ProjectAssignment>(search);
            response.Data = listRes;

            try
            {
                var ProjectAssignments = AppProvider.GetDBContext().ProjectAssignments
                    .Where(x => string.IsNullOrEmpty(search.SearchTerm) ||
                       //  x.Type.Contains(search.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                         x.Title.Contains(search.SearchTerm) ||
                         x.Description.Contains(search.SearchTerm));



                listRes.TotalCount = ProjectAssignments.Count();
                ProjectAssignments = ProjectAssignments.Skip(search.PageStartIndex).Take(search.ItemsPerPage);
                listRes.List = ProjectAssignments.Include(x => x.ProjectUser).Include(x => x.AssignmentResults).ToList();
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }

        internal static ProviderResponse GetProjectAssignment(int id)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                ProjectAssignment projectAssignment = AppProvider.GetDBContext().ProjectAssignments
                        .Where(x => x.ProjectAssignmentID == id)
                              .Include(x => x.ProjectUser)
                               .Include(x => x.AssignmentResults)
                                   .ThenInclude(x => x.Maamar)
                                                   .FirstOrDefault();

                if (projectAssignment != null)
                {
                    response.Data = projectAssignment;
                }
                else
                {
                    response.Messages.Add("נושא לא חוקי");
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }

        internal static ProviderResponse AddProjectAssignment(ProjectAssignment projectAssignment)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                response.Messages.AddRange(ValidateProjectAssignment(null, projectAssignment, false, db));

                if (response.Success)
                {

                    ProjectAssignment NewProjectAssignment = new ProjectAssignment
                    {
                        ProjectUser = projectAssignment.ProjectUser,
                        Type = projectAssignment.Type,
                        Status = projectAssignment.Status,
                        Title = projectAssignment.Title,
                        Description = projectAssignment.Description,
                        AssignmentResults = new List<AssignmentResult>(),
                    };

                    foreach (AssignmentResult AssignmentResults in projectAssignment.AssignmentResults)
                    {
                        AssignmentResult newAssignmentResults = new AssignmentResult
                        {
                            Maamar = AssignmentResults.Maamar,
                            Note = AssignmentResults.Note,
                            Status = AssignmentResults.Status,

                        };
                        NewProjectAssignment.AssignmentResults.Add(newAssignmentResults);
                    }
                    db.ProjectAssignments.Add(NewProjectAssignment);

                    db.SaveChanges();
                    response.Data = projectAssignment;
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }

        internal static ProviderResponse UpdateProjectAssignment(int id, ProjectAssignment projectAssignment)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                ProjectAssignment currProjectAssignment = db.ProjectAssignments.Where(x => x.ProjectAssignmentID == id).Include(x => x.ProjectUser).Include(x => x.AssignmentResults).FirstOrDefault();
                response.Messages.AddRange(ValidateProjectAssignment(currProjectAssignment, projectAssignment, true, db));

                if (response.Success)
                {
                    currProjectAssignment.ProjectUser = projectAssignment.ProjectUser;
                    currProjectAssignment.Type = projectAssignment.Type;
                    currProjectAssignment.Status = projectAssignment.Status;
                    currProjectAssignment.Title = projectAssignment.Title;
                    currProjectAssignment.Description = projectAssignment.Description;
                    currProjectAssignment.Status = projectAssignment.Status;

                    foreach (AssignmentResult AssignmentResults in projectAssignment.AssignmentResults)
                    {
                        AssignmentResult currAssignmentResults = currProjectAssignment.AssignmentResults.Find(x => x.AssignmentResultID == AssignmentResults.AssignmentResultID);

                        if (currAssignmentResults != null)
                        {
                            currAssignmentResults.ProjectAssignment = AssignmentResults.ProjectAssignment;
                            currAssignmentResults.Maamar = AssignmentResults.Maamar;
                            currAssignmentResults.Note = AssignmentResults.Note;
                            currAssignmentResults.Status = AssignmentResults.Status;
                        }
                        else
                        {
                            currAssignmentResults = new AssignmentResult
                            {
                                ProjectAssignment = AssignmentResults.ProjectAssignment,
                                Maamar = AssignmentResults.Maamar,
                                Note = AssignmentResults.Note,
                                Status = AssignmentResults.Status,
                            };
                            currProjectAssignment.AssignmentResults.Add(currAssignmentResults);
                        }
                    }
                }
                db.SaveChanges();
                response.Data = currProjectAssignment;
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }

        internal static ProviderResponse DeleteProjectAssignment(int id)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();

                ProjectAssignment currProjectAssignment = db.ProjectAssignments.Find(id);

                if (currProjectAssignment == null)
                {
                    response.Messages.Add("נושא לא חוקי");
                }
                else
                {
                    currProjectAssignment.Status = ProjectAssignmentsStatus.Deleted;
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

        private static List<string> ValidateProjectAssignment(ProjectAssignment currProjectAssignment, ProjectAssignment projectAssignment, bool update, AppDBContext db)
        {
            List<string> errors = new List<string>();

            if (projectAssignment == null)
            {
                errors.Add("הקצאת פרויקט לא חוקית");
            }
            else if (update && currProjectAssignment == null)
            {
                errors.Add("מזהה הקצאת פרויקט לא חוקי");
            }
            else
            {
                if (projectAssignment.ProjectUser != null)
                {
                    projectAssignment.ProjectUser = db.ProjectUsers.Find(projectAssignment.ProjectUser.ProjectUserID);
                    if (projectAssignment.ProjectUser == null)
                    {
                        errors.Add("הפניה לא חוקית למשתמש בפרויקט");
                    }
                }
                else
                {
                    foreach (AssignmentResult AssignmentResult in projectAssignment.AssignmentResults)
                    {
                        if (AssignmentResult.Maamar != null)
                        {
                            AssignmentResult.Maamar = db.Maamarim.Find(AssignmentResult.Maamar.MaamarID);
                            if (AssignmentResult.Maamar == null)
                            {
                                errors.Add("הפניה לא חוקית של מאמר");
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(projectAssignment.Description))
                {
                    errors.Add("תיאור חסר");
                }
            }
            return errors;
        }
    }
}
