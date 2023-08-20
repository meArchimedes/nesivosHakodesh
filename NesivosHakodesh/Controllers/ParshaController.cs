using Microsoft.AspNetCore.Mvc;
using NesivosHakodesh.Providers.Parsha;
using NesivosHakodesh.Providers.Utils.Api;

namespace NesivosHakodesh.Controllers
{
    [ApiController]
    [Route("api/Parsha")]
    public class ParshaController : BaseApiController
    {
        [HttpGet]
        public ActionResult<ProviderResponse> GetAllParshas()
        {
            return ReturnResponse(ParshaProvider.GetAllParshas());
        }

        [HttpGet, Route("withBooks")]
        public ActionResult<ProviderResponse> GetAllParshas2()
        {
            return ReturnResponse(ParshaProvider.GetAllParshas2());
        }

        [HttpGet, Route("Years")]
        public ActionResult<ProviderResponse> GetAllYears()
        {
            return ReturnResponse(ParshaProvider.GetAllYears());
        }
    }
}
