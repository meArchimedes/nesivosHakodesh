using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NesivosHakodesh.Comman;
using NesivosHakodesh.Providers.Torahs;
using NesivosHakodesh.Providers.Utils.Api;

namespace NesivosHakodesh.Controllers
{
    [ApiController]
    [Route("api/Torahs")]
    public class TorahsController : BaseApiController
    {
        [HttpPost, Route("search")]
        public ActionResult<ProviderResponse> GetAllTorahs([FromBody] SearchCriteria search)
        {
            return ReturnResponse(TorahsProvider.GetAllTorahs(search));
        }

        /*[Authorize(Roles = "TORHAS_VIEW")]
        [HttpGet, Route("{id}")]
        public ActionResult<ProviderResponse> GetTorah(int id)
        {
            return ReturnResponse(TorahsProvider.GetTorah(id));
        }*/

        /*[Authorize(Roles = "TORHAS_EDIT")]
        [HttpPost]
        public ActionResult<ProviderResponse> AddTorah([FromBody] Torah Torah)
        {
            return ReturnResponse(TorahsProvider.AddTorah(Torah));
        }*/

        /*[Authorize(Roles = "TORHAS_EDIT")]
        [HttpPut]
        public ActionResult<ProviderResponse> UpdateTorah(Torah torahs)
        {
            return ReturnResponse(TorahsProvider.UpdateTorah( torahs));
        }*/

        [Authorize(Roles = "TORHAS_EDIT")]
        [HttpDelete, Route("{id}")]
        public ActionResult<ProviderResponse> DeleteTorah(int id)
        {
            return ReturnResponse(TorahsProvider.DeleteTorah(id));
        }

    }
}
