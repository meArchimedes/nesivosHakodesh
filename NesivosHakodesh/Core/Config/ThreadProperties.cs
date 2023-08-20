using System.Threading;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NesivosHakodesh.Core.DB;
using NesivosHakodesh.Domain.Entities;

namespace NesivosHakodesh.Core.Config
{
    public class ThreadProperties
    {
        private static AsyncLocal<AppDBContext> _dbContext = new AsyncLocal<AppDBContext>();
        private static AsyncLocal<IConfiguration> _configuration = new AsyncLocal<IConfiguration>();
        private static AsyncLocal<UserManager<User>> _userManager = new AsyncLocal<UserManager<User>>();
        private static AsyncLocal<User> _currentUser = new AsyncLocal<User>();

        public static void SetConfiguration(IConfiguration configuration)
        {
            _configuration.Value = configuration;
        }
        public static IConfiguration GetConfiguration()
        {
            return _configuration.Value;
        }

        public static void SetDbContext(AppDBContext dBContext)
        {
            _dbContext.Value = dBContext;
        }
        public static AppDBContext GetDbContext()
        {
            return _dbContext.Value;
        }

        public static void SetUserManager(UserManager<User> manager)
        {
            _userManager.Value = manager;
        }
        public static UserManager<User> GetUserManager()
        {
            return _userManager.Value;
        }

        public static void SetCurrentUser(User appUser)
        {
            _currentUser.Value = appUser;
        }
        public static User GetCurrentUser()
        {
            return _currentUser.Value;
        }
    }
}
