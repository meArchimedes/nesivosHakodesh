using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NesivosHakodesh.Comman;
using NesivosHakodesh.Domain.Entities;
using NesivosHakodesh.Providers.Torahs;
using NesivosHakodesh.Providers.Utils.Api;

namespace NesivosHakodesh.Controllers
{
    [ApiController]
    [Route("api/Sefurim")]
    public class SefurimController : BaseApiController
    {
        [HttpPost, Route("search")]
        public ActionResult<ProviderResponse> GetAllSefurim([FromBody] SearchCriteria search)
        {
            return ReturnResponse(SefurimProvider.GetAllSefurim(search));
        }

        [HttpGet, Route("list")]
        public ActionResult<ProviderResponse> GetSefurimList()
        {
            var response = SefurimProvider.GetSefurimList();
            return ReturnResponse(response);
        }

        [Authorize(Roles = "TORHAS_VIEW")]
        [HttpGet, Route("{id}")]
        public ActionResult<ProviderResponse> GetSefer(int id)
        {
            return ReturnResponse(SefurimProvider.GetSefer(id));
        }

        [Authorize(Roles = "TORHAS_EDIT")]
        [HttpPost]
        public ActionResult<ProviderResponse> AddSefer([FromBody] Sefer sefurim)
        {
            return ReturnResponse(SefurimProvider.AddSefer(sefurim));
        }

        [Authorize(Roles = "TORHAS_EDIT")]
        [HttpPut]
        public ActionResult<ProviderResponse> UpdateSefer([FromBody] Sefer sefurim)
        {
            return ReturnResponse(SefurimProvider.UpdateSefer(sefurim));
        }

        [Authorize(Roles = "TORHAS_EDIT")]
        [HttpDelete, Route("{id}")]
        public ActionResult<ProviderResponse> DeleteSefer(int id)
        {
            return ReturnResponse(SefurimProvider.DeleteSefer(id));
        }
    }
}
