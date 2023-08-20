using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NesivosHakodesh.Providers.Solr;
using NesivosHakodesh.Providers.Utils.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NesivosHakodesh.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet, Route("test")]
        public ActionResult<ProviderResponse> Test()
        {
            return ReturnResponse(new ProviderResponse
            {
                Data = "Hello World"
            });
        }

        [AllowAnonymous]
        [HttpGet, Route("solr")]
        public ActionResult<ProviderResponse> SolrTest()
        {
            return ReturnResponse(SolrSyncProvider.SyncSolr());
        }
    }
}
