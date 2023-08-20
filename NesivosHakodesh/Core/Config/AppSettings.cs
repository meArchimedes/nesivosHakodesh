using NesivosHakodesh.Providers.Identity;
using NesivosHakodesh.Providers.Utils.Emails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NesivosHakodesh.Core.Config
{
    public class AppSettings
    {
        public EmailSettings EmailSettings { get; set; }
        public IdentitySettings IdentitySettings { get; set; }
        public ClientSettings ClientSettings { get; set; }
        public OtherSettings OtherSettings { get; set; }
    }

    public class ClientSettings
    {
        public string BaseUrl { get; set; }
        public string ResetPasswordPage { get; set; }
        public string SignInPage { get; set; }
    }

    public class OtherSettings
    {
        public string SolrBaseUrl { get; set; }
        public string FilesBaseDir { get; set; }
        public string LogToFile { get; set; }
    }
}
