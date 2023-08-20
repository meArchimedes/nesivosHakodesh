using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NesivosHakodesh.Comman;
using NesivosHakodesh.Core;
using NesivosHakodesh.Core.DB;
using NesivosHakodesh.Domain.Entities;
using NesivosHakodesh.Providers.Identity;
using NesivosHakodesh.Providers.Utils;
using NesivosHakodesh.Providers.Utils.Api;

namespace NesivosHakodesh.Providers.Users
{
    public class UsersProvider
    {
        internal static ProviderResponse GetAllUsers(SearchCriteria search)
        {
            ProviderResponse response = new ProviderResponse();
            ListResData<User> listRes = new ListResData<User>(search);
            response.Data = listRes;

            try
            {
                var UserSearch = AppProvider.GetDBContext().Users
                    .Where(x => string.IsNullOrEmpty(search.SearchTerm) ||
                      x.FirstName.Contains(search.SearchTerm)  ||
                      x.LastName.Contains(search.SearchTerm) ||
                      x.Email.Contains(search.SearchTerm)  ||
                      x.PhoneNumber.Contains(search.SearchTerm)  ||
                      x.Cell.Contains(search.SearchTerm));



                listRes.TotalCount = UserSearch.Count();

                if (search.SortDirection == SortDirection.Ascending)
                {

                    UserSearch = UserSearch.OrderBy(x => EF.Property<object>(x, search.SortBy));  //.ThenBy(x => x.HebrewFirstName)

                }
                else
                {

                    UserSearch = UserSearch.OrderByDescending(x => EF.Property<object>(x, search.SortBy));  //.ThenBy(x => x.HebrewFirstName)

                }
                UserSearch = UserSearch.Skip(search.PageStartIndex).Take(search.ItemsPerPage);

                listRes.List = UserSearch.Select(x => new User { 
                    Id = x.Id,
                    UserName = x.UserName,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhoneNumber = x.PhoneNumber,
                    Status = x.Status,
                    Email = x.Email,
                    Cell = x.Cell
                }).ToList();
            
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
         }

        internal static ProviderResponse GetAllUsersOpen()
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                response.Data = AppProvider.GetDBContext().Users.Select(x => new User
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName
                }).ToList();

            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }

        internal static ProviderResponse GetUser(int id)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                User user = AppProvider.GetDBContext().Users
                                                .Where(x => x.Id == id)
                                                .Include(x => x.UserRoles)
                                                    .ThenInclude(x => x.Role)
                                                .FirstOrDefault();

                if (user != null)
                {
                    response.Data = user;
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
        }


        internal static async Task<ProviderResponse> AddUserAsync(User user)
        {
            ProviderResponse response = new ProviderResponse();
          
            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                response.Messages.AddRange(ValidateUser(null, user, false, db));

                if (response.Success)
                {
                    response = await IdentityService.RegisterAsync(user);
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }

        internal static async Task<ProviderResponse> UpdateUserAsync( User user)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                User currUser = db.Users.Where(x => x.Id == user.Id)
                                        .Include(x => x.UserRoles)
                                            .ThenInclude(x => x.Role)
                                        .FirstOrDefault();

                List<Role> allRoles = db.Roles.ToList();

                response.Messages.AddRange(ValidateUser(currUser, user, true, db));

                if (response.Success)
                {

                    currUser.UserName = user.UserName;
                    currUser.NormalizedUserName = user.UserName;
                    currUser.Email = user.Email ;
                    currUser.NormalizedEmail = user.Email;
                    currUser.FirstName = user.FirstName;
                    currUser.LastName = user.LastName;
                    currUser.PhoneNumber = user.PhoneNumber;
                    currUser.Cell = user.Cell;
                    currUser.Status = user.Status;

                    //remove deleted roles
                    foreach (UserRole currRole in currUser.UserRoles.ToList())
                    {
                        //if not in new list
                        if(user.UserRoles.Find(x => x.Role.Name == currRole.Role.Name) == null)
                        {
                            db.UserRoles.Remove(currRole);
                        }
                    }

                    //add new roles
                    foreach (UserRole newRole in user.UserRoles)
                    {
                        //if not in current list
                        if (currUser.UserRoles.Find(x => x.Role.Name == newRole.Role.Name) == null)
                        {
                            //only if role exists
                            Role role = allRoles.Find(x => x.Name == newRole.Role.Name);

                            if(role != null)
                            {
                                currUser.UserRoles.Add(new UserRole {
                                    Role = role
                                });
                            }
                        }
                    }


                    string sectionRoleName = $"{Sections.MAAMARIM}_{PermissionType.VIEW}";
                    //if has any maamarim permission, then make sure has section permissaion
                    if (PermissionProvider.GetMaamarimTypesForUser(currUser).Count > 0)
                    {
                        if(!PermissionProvider.HasViewAccess(currUser, Sections.MAAMARIM))
                        {
                            currUser.UserRoles.Add(new UserRole
                            {
                                Role = allRoles.Find(x => x.Name == sectionRoleName)
                            });
                        }
                    }
                    //if does not have, then remove section permission
                    else
                    {
                        if (PermissionProvider.HasViewAccess(currUser, Sections.MAAMARIM))
                        {
                            db.UserRoles.Remove(currUser.UserRoles.Find(x => x.Role.Name == sectionRoleName));
                        }
                    }

                    db.SaveChanges();

                    if(!string.IsNullOrEmpty(user.NewPassword))
                    {
                        response = await IdentityService.ChangePasswordForUser(currUser.Id, user.NewPassword);
                    }

                    response.Data = currUser;
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }

        internal static ProviderResponse DeleteUser(int id)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                User currUser = db.Users.Find(id);

                if (currUser == null)
                {
                    response.Messages.Add("לא חוקי");
                }
                else
                {
                    currUser.Status = UserStatus.Deleted;
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

        private static List<string> ValidateUser(User currUser, User user, bool update, AppDBContext db)
        {
            List<string> errors = new List<string>();

            if (user == null)
            {
                errors.Add("משתמש שגוי");
            }
            else if (update && currUser == null)
            {
                errors.Add("מזהה משתמש לא חוקי");
            }
            else
            {
                if (string.IsNullOrEmpty(user.FirstName))
                {
                    errors.Add("חסר שם פרטי");
                }
                if (string.IsNullOrEmpty(user.LastName))
                {
                    errors.Add("חסר שם משפחה");
                }
                if (string.IsNullOrEmpty(user.Email))
                {
                    errors.Add("חסר אימייל");
                }
                if (string.IsNullOrEmpty(user.Cell))
                {
                    //errors.Add("Missing Call Number");
                }
                if (string.IsNullOrEmpty(user.UserName))
                {
                    errors.Add("חסר שם משתמש");
                }
                
                if(!update && string.IsNullOrEmpty(user.NewPassword))
                {
                    errors.Add("חסר סיסמה");
                }

                /*if (!update || currUser.FirstName != user.FirstName || currUser.LastName != user.LastName || currUser.Cell != user.Cell)
                {
                    var Chack = db.Users.Where(x => x.FirstName == user.FirstName && x.LastName == user.LastName && x.Cell == user.Cell).ToList();

                    if (Chack.Count != 0)
                    {
                        errors.Add("User with This Name And Call number exists already");
                    }

                }*/

            }

            return errors;
        }

        public static async Task<ProviderResponse> LoginUserAsync(User login)
        {
            ProviderResponse res = new ProviderResponse();

            try
            {
                //validate user
                if (login == null || string.IsNullOrEmpty(login.UserName))
                {
                    res.Messages.Add("שם משתמש לא חוקי");
                    return res;
                }
                if (string.IsNullOrEmpty(login.NewPassword))
                {
                    res.Messages.Add("סיסמה שגויה");
                }
                if (!res.Success)
                {
                    return res;
                }

                res = await IdentityService.LoginUserAsync(login);
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                res.Messages.Add("התרחשה שגיאה");
            }

            return res;
        }
    }
}
