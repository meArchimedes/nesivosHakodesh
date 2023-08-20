using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NesivosHakodesh.Domain.Entities;
using NesivosHakodesh.Providers.Torahs;
using NesivosHakodesh.Providers.Utils.Api;

namespace NesivosHakodesh.Controllers
{
    [ApiController]
    [Route("api/MaamarTorahLink")]
    public class MaamarTorahLinkController : BaseApiController
    {
        //TODO need to add validation check for maamar permission
        [Authorize(Roles = "TORHAS_EDIT")]
        [HttpPost]
        public ActionResult<ProviderResponse> AddMaamarTorahLink([FromBody] List<MaamarTorahLink> MaamarTorah)
        {
            return ReturnResponse(MaamarTorahLinkProvider.AddMaamarTorahLink(MaamarTorah));
        }

        [Authorize(Roles = "TORHAS_EDIT")]
        [HttpGet, Route("{id}")]
        public ActionResult<ProviderResponse> GetMaamarTorahLink (int id)
        {
            return ReturnResponse(MaamarTorahLinkProvider.GetMaamarTorahLink(id));
        }

        [Authorize(Roles = "TORHAS_EDIT")]
        [HttpDelete, Route("{id}")]
        public ActionResult<ProviderResponse> DeleteMaamarTorahLink(int id)
        {
            return ReturnResponse(MaamarTorahLinkProvider.DeleteMaamarTorahLink(id));
        }

    }
}
