using Microsoft.Extensions.Configuration;
using NesivosHakodesh.Providers.Identity;
using NesivosHakodesh.Providers.Utils.Emails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NesivosHakodesh.Core.Config
{
    public class AppSettingsProvider
    {
        public static AppSettings GetAppSettings()
        {
            var appSettingsSection = ThreadProperties.GetConfiguration().GetSection("AppSettings");
            return appSettingsSection.Get<AppSettings>();
        }

        public static EmailSettings GetEmailSettings()
        {
            return GetAppSettings().EmailSettings;
        }

        public static IdentitySettings GetIdentitySettings()
        {
            return GetAppSettings().IdentitySettings;
        }

        public static ClientSettings GetClientSettings()
        {
            return GetAppSettings().ClientSettings;
        }

        public static OtherSettings GetOtherSettings()
        {
            return GetAppSettings().OtherSettings;
        }
    }
}
