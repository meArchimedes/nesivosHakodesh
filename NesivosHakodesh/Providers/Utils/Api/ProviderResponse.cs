using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NesivosHakodesh.Providers.Utils.Api
{
    public class ProviderResponse
    {
        public object Data;
        public List<string> Messages = new List<string>();

        private bool _success = true;
        public bool Success
        {
            get
            {
                if (Messages.Any())
                {
                    return false;
                }

                return _success;
            }
            set
            {
                _success = value;
            }
        }
    }
}

