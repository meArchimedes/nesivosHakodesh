using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NesivosHakodesh.Providers.Utils.Emails
{
    public class EmailSettings
    {
        public bool SendEmail { get; set; }
        public string DevEmail { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }

        public string SendGridApiKey { get; set; }
    }
}
