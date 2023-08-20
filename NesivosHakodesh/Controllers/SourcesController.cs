using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NesivosHakodesh.Comman;
using NesivosHakodesh.Domain.Entities;
using NesivosHakodesh.Providers.Torahs;
using NesivosHakodesh.Providers.Utils.Api;

namespace NesivosHakodesh.Controllers
{
    [ApiController]
    [Route("api/Sources")]
    public class SourcesController : BaseApiController
    {
        [HttpPost, Route("search")]
        public ActionResult<ProviderResponse> GetAllSources([FromBody] SearchCriteria search)
        {
            return ReturnResponse(SourcesProvider.GetAllSources(search));
        }

        [Authorize(Roles = "SOURCES_VIEW")]
        [HttpGet, Route("{id}")]
        public ActionResult<ProviderResponse> GetSource(int id)
        {
            return ReturnResponse(SourcesProvider.GetSource(id));
        }

        [Authorize(Roles = "SOURCES_EDIT")]
        [HttpPost]
        public ActionResult<ProviderResponse> AddSource([FromBody] Source sources)
        {
            return ReturnResponse(SourcesProvider.AddSource(sources));
        }

        [Authorize(Roles = "SOURCES_EDIT")]
        [HttpPut]
        public ActionResult<ProviderResponse> UpdateSource( [FromBody] Source sources)
        {
            return ReturnResponse(SourcesProvider.UpdateSource( sources));
        }

        [Authorize(Roles = "SOURCES_EDIT")]
        [HttpDelete, Route("{id}")]
        public ActionResult<ProviderResponse> DeleteSource(int id)
        {
            return ReturnResponse(SourcesProvider.DeleteSource(id));
        }
    }
}
