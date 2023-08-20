using System.Collections.Generic;
using System.Linq;
using NesivosHakodesh.Comman;
using NesivosHakodesh.Core;
using NesivosHakodesh.Domain.Entities;

namespace NesivosHakodesh.Providers.Identity
{
    public enum PermissionType
    {
        VIEW,
        EDIT,
        PRINT,
        DOWNLOAD,
    }

    public enum Sections
    {
        MAAMARIM,
        TORHAS,
        TOPICS,
        SOURCES,
        SOURCES_PRS,
    }

    public class PermissionProvider
    {
        private static readonly string ADMIN = "ADMIN";

        public static void PopulateMaamarimSearchBasedOnUserRoles(SearchCriteria search)
        {
            User currentUser = AppProvider.GetCurrentUser();

            //Type
            PopulateTypesForFilter(search, currentUser, PermissionType.VIEW);

            //CreatedUserId
            if (!IsAdmin(currentUser))
            {
                search.CreatedUserId = currentUser.Id;
            }
        }

        public static bool HasPermissionToMaamar(Maamar maamar, PermissionType permissionType)
        {
            User currentUser = AppProvider.GetCurrentUser();

            if (IsAdmin(currentUser))
            {
                return true;
            }

            //Type
            if (!GetAllowedTypes(currentUser, permissionType).Contains(maamar.Type.Value))
            {
                return false;
            }

            //Locked
            if (maamar.Locked && maamar.CreatedUser.Id != currentUser.Id)
            {
                return false;
            }

            return true;
        }

        public static void PopulateSourcesSearchBasedOnUserRoles(SearchCriteria search)
        {
            User currentUser = AppProvider.GetCurrentUser();

            //Type
            if(HasViewAccess(currentUser, Sections.SOURCES_PRS) || IsAdmin(currentUser))
            {
                search.IncludePersonalSources = true;
            }
        }

        public static bool HasPermissionToSource(Source source, PermissionType permissionType)
        {
            User currentUser = AppProvider.GetCurrentUser();

            if (IsAdmin(currentUser))
            {
                return true;
            }

            if(source.SourceType == SourceTypes.Maamarim)
            {
                return currentUser?.UserRoles?.Find(x => x.Role.Name == $"{Sections.SOURCES}_{permissionType}") != null;
            }
            else
            {
                return currentUser?.UserRoles?.Find(x => x.Role.Name == $"{Sections.SOURCES_PRS}_{permissionType}") != null;
            }
        }

        public static string Error()
        {
            return "אין לך הרשאה לבצע פעולה זו";
        }

        public static List<MaamarType> GetMaamarimTypesForUser(User currentUser)
        {
            return GetAllowedTypes(currentUser, PermissionType.VIEW);
        }

        public static List<MaamarType> GetMaamarimTypes()
        {
            User currentUser = AppProvider.GetCurrentUser();
            return GetMaamarimTypesForUser(currentUser);
        }

        public static bool HasViewAccess(User currentUser, Sections section)
        {
            return currentUser?.UserRoles?.Find(x => x.Role.Name == $"{section}_{PermissionType.VIEW}") != null;
        }

        public static bool IsAdmin(User currentUser)
        {
            return currentUser?.UserRoles?.Find(x => x.Role.Name == ADMIN) != null;
        }

        public static List<Role> GetAllRoles()
        {
            return new List<Role> {
                new Role { Id = 1, Name = $"{ADMIN}" },
                new Role { Id = 2, Name = $"{Sections.TOPICS}_{PermissionType.VIEW}" },
                new Role { Id = 3, Name = $"{Sections.TOPICS}_{PermissionType.EDIT}" },
                new Role { Id = 4, Name = $"{Sections.SOURCES}_{PermissionType.VIEW}" },
                new Role { Id = 5, Name = $"{Sections.SOURCES}_{PermissionType.EDIT}" },
                new Role { Id = 6, Name = $"{Sections.SOURCES_PRS}_{PermissionType.VIEW}" },
                new Role { Id = 7, Name = $"{Sections.SOURCES_PRS}_{PermissionType.EDIT}" },
                new Role { Id = 8, Name = $"{Sections.TORHAS}_{PermissionType.VIEW}" },
                new Role { Id = 9, Name = $"{Sections.TORHAS}_{PermissionType.EDIT}" },
                new Role { Id = 10, Name = $"{Sections.MAAMARIM}_0_{PermissionType.VIEW}" },
                new Role { Id = 11, Name = $"{Sections.MAAMARIM}_0_{PermissionType.EDIT}" },
                new Role { Id = 12, Name = $"{Sections.MAAMARIM}_0_{PermissionType.PRINT}" },
                new Role { Id = 13, Name = $"{Sections.MAAMARIM}_0_{PermissionType.DOWNLOAD}" },
                new Role { Id = 14, Name = $"{Sections.MAAMARIM}_1_{PermissionType.VIEW}" },
                new Role { Id = 15, Name = $"{Sections.MAAMARIM}_1_{PermissionType.EDIT}" },
                new Role { Id = 16, Name = $"{Sections.MAAMARIM}_1_{PermissionType.PRINT}" },
                new Role { Id = 17, Name = $"{Sections.MAAMARIM}_1_{PermissionType.DOWNLOAD}" },
                new Role { Id = 18, Name = $"{Sections.MAAMARIM}_2_{PermissionType.VIEW}" },
                new Role { Id = 19, Name = $"{Sections.MAAMARIM}_2_{PermissionType.EDIT}" },
                new Role { Id = 20, Name = $"{Sections.MAAMARIM}_2_{PermissionType.PRINT}" },
                new Role { Id = 21, Name = $"{Sections.MAAMARIM}_2_{PermissionType.DOWNLOAD}" },
                new Role { Id = 22, Name = $"{Sections.MAAMARIM}_3_{PermissionType.VIEW}" },
                new Role { Id = 23, Name = $"{Sections.MAAMARIM}_3_{PermissionType.EDIT}" },
                new Role { Id = 24, Name = $"{Sections.MAAMARIM}_3_{PermissionType.PRINT}" },
                new Role { Id = 25, Name = $"{Sections.MAAMARIM}_3_{PermissionType.DOWNLOAD}" },
                new Role { Id = 26, Name = $"{Sections.MAAMARIM}_{PermissionType.VIEW}" },
            };
        }



        private static void PopulateTypesForFilter(SearchCriteria search, User currentUser, PermissionType permissionType)
        {
            //get
            List<MaamarType> allowedTypes = GetAllowedTypes(currentUser, permissionType);

            List<MaamarType> filteredTypes = new List<MaamarType>();

            //if searched for any, then only allow to search for allowed types
            if (search.Type != null && search.Type.Any())
            {
                foreach (var type in search.Type)
                {
                    if (allowedTypes.Contains(type))
                    {
                        filteredTypes.Add(type);
                    }
                }

                //if or searched but not is allowd
                if (filteredTypes.Count == 0)
                {
                    filteredTypes.Add(MaamarType.None);
                }
            }

            //if not searched for any, then all all allowed
            if (search.Type == null || search.Type.Count == 0)
            {
                filteredTypes = allowedTypes;

                //if he does not have permission to any
                if (filteredTypes.Count == 0)
                {
                    filteredTypes.Add(MaamarType.None);
                }
            }

            search.Type = filteredTypes;
        }

        private static List<MaamarType> GetAllowedTypes(User currentUser, PermissionType permissionType)
        {
            List<MaamarType> types = new List<MaamarType>();

            if(IsAdmin(currentUser) || currentUser.UserRoles.Find(x => x.Role.Name == $"{Sections.MAAMARIM}_{(int)MaamarType.PisguminKadishin}_{permissionType}") != null)
            {
                types.Add(MaamarType.PisguminKadishin);
            }
            if (IsAdmin(currentUser) || currentUser.UserRoles.Find(x => x.Role.Name == $"{Sections.MAAMARIM}_{(int)MaamarType.HadrachosYesharos}_{permissionType}") != null)
            {
                types.Add(MaamarType.HadrachosYesharos);
            }
            if (IsAdmin(currentUser) || currentUser.UserRoles.Find(x => x.Role.Name == $"{Sections.MAAMARIM}_{(int)MaamarType.BechatzarHakodesh}_{permissionType}") != null)
            {
                //types.Add(Models.Type.BechatzarHakodesh);
                types.Add(MaamarType.BH_BerchesKodesh);
                types.Add(MaamarType.BH_MaameremProtem);
                types.Add(MaamarType.BH_NoamSheikh);
                types.Add(MaamarType.BH_SeperiKodesh);
                types.Add(MaamarType.BH_SichesKodesh);
            }
            if (IsAdmin(currentUser) || currentUser.UserRoles.Find(x => x.Role.Name == $"{Sections.MAAMARIM}_{(int)MaamarType.Personals}_{permissionType}") != null)
            {
                types.Add(MaamarType.Personals);
            }

            return types;
        }

        
    }
}
