using Microsoft.AspNetCore.Mvc;
using NesivosHakodesh.Comman;
using NesivosHakodesh.Domain.Entities;
using NesivosHakodesh.Providers.Torahs;
using NesivosHakodesh.Providers.Utils.Api;

namespace NesivosHakodesh.Controllers
{
    [ApiController]
    [Route("api/Maamarim")]
    public class MaamarimController : BaseApiController
    {
        [HttpPost, Route("search")]
        public ActionResult<ProviderResponse> GetAllMaamarim([FromBody] SearchCriteria search)
        {
            return ReturnResponse(MaamarimProvider.GetAllMaamarim(search));
        }
        [HttpGet, Route("{id}")]
        public ActionResult<ProviderResponse> GetMaamar(int id)
        {
            return ReturnResponse(MaamarimProvider.GetMaamar(id));
        }

        [HttpPost]
        public ActionResult<ProviderResponse> AddMaamar([FromBody] Maamar maamarim)
        {
            return ReturnResponse(MaamarimProvider.AddMaamar(maamarim));
        }

        [HttpPut]
        public ActionResult<ProviderResponse> UpdateMaamar( [FromBody] Maamar maamarim)
        {
            return ReturnResponse(MaamarimProvider.UpdateMaamar(maamarim));
        }

        [HttpDelete, Route("{id}")]
        public ActionResult<ProviderResponse> DeleteMaamar(int id)
        {
            return ReturnResponse(MaamarimProvider.DeleteMaamar(id));
        }

        //[HttpDelete, Route("Paragraph")]
        //public ActionResult<ProviderResponse> DeleteMaamarParagraph(int id)
        //{
        //    return ReturnResponse(MaamarimProvider.DeleteMaamar(id));
        //}

        [HttpGet, Route("folders")]
        public ActionResult<ProviderResponse> GetFolders()
        {
            return ReturnResponse(MaamarimFoldersProvider.GetFileStructure());
        }

        [HttpGet, Route("types")]
        public ActionResult<ProviderResponse> GetMaamarimTypes()
        {
            return ReturnResponse(MaamarimProvider.GetMaamarTypes());
        }

        [HttpGet, Route("status")]
        public ActionResult<ProviderResponse> GetMaamarimStatusOptions()
        {
            return ReturnResponse(MaamarimProvider.GetMaamarStatusOptions());
        }
    }
}
