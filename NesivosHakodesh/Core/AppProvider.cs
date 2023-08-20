using NesivosHakodesh.Core.Config;
using NesivosHakodesh.Core.DB;
using NesivosHakodesh.Domain.Entities;

namespace NesivosHakodesh.Core
{
    public class AppProvider
    {
        public static User GetCurrentUser()
        {
            User appUser = ThreadProperties.GetCurrentUser();
            return appUser;
        }

        public static User GetCurrentUserFromDB()
        {
            
            return GetDBContext().Users.Find(GetCurrentUser().Id);
        }

        public static AppDBContext GetDBContext()
        {
            return ThreadProperties.GetDbContext();
        }
    }
}
