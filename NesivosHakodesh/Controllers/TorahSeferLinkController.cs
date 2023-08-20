using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NesivosHakodesh.Domain.Entities;
using NesivosHakodesh.Providers.Torahs;
using NesivosHakodesh.Providers.Utils.Api;

namespace NesivosHakodesh.Controllers
{
    [ApiController]
    [Route("api/TorahSeferLink")]
    public class TorahSeferLinkController : BaseApiController
    {
        //TODO need to add validation check for maamar permission
        [Authorize(Roles = "TORHAS_EDIT")]
        [HttpPost]
        public ActionResult<ProviderResponse> AddTorahSeferLink([FromBody] List<TorahSeferLink> torahSefer)
        {
            var response = TorahSeferLinkProvider.AddTorahSeferLink(torahSefer);
            return ReturnResponse(response);
        }

        [Authorize(Roles = "TORHAS_EDIT")]
        [HttpGet, Route("{id}")]
        public ActionResult<ProviderResponse> GetTorahSeferLink (int seferId)
        {
            var response = TorahSeferLinkProvider.GetTorahSeferLink(seferId);
            return ReturnResponse(response);
        }

        [Authorize(Roles = "TORHAS_EDIT")]
        [HttpDelete, Route("{id}")]
        public ActionResult<ProviderResponse> DeleteTorahSeferLink(int torahSeferId)
        {
            var response = TorahSeferLinkProvider.DeleteTorahSeferLink(torahSeferId);
            return ReturnResponse(response);
        }
    }
}
