using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NesivosHakodesh.Comman;
using NesivosHakodesh.Providers.Library;
using NesivosHakodesh.Providers.Utils.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NesivosHakodesh.Controllers
{
    [ApiController]

    [Route("api/Library")]
    public class LibraryController : BaseApiController
    {

        [HttpPost, Route("search")]
        public ActionResult<ProviderResponse> GetAllLibrary([FromBody] SearchCriteria search)
        {
            return ReturnResponse(LibraryProvider.GetAllLibrary(search));
        }

        [HttpPost, Route("Scrolldetails")]
        public ActionResult<ProviderResponse> ScrollLibrarydetails([FromBody] SearchCriteria search)
        {
            return ReturnResponse(LibraryProvider.ScrollLibrarydetails(search));
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult<ProviderResponse> AddToLibrary([FromBody] LibraryObject LibraryObject)
        {
            return ReturnResponse(LibraryProvider.AddToLibraryTalmud(LibraryObject));
        }


        [HttpGet, Route("Categoryfilters")]
        public ActionResult<ProviderResponse> GetCategoryOptions()
        {
            return ReturnResponse(LibraryProvider.GetCategoryOptions());
        }

        [HttpPost, Route("Typefilters")]
        public ActionResult<ProviderResponse> GetTypeOptions([FromBody] List<string> Category)
        {
            return ReturnResponse(LibraryProvider.GetTypeOptions(Category));
        }
      
        [HttpPost, Route("Sectionfilters")]
        public ActionResult<ProviderResponse> GetSectionOptions([FromBody] SearchCriteria Type)
        {
            return ReturnResponse(LibraryProvider.GetSectionOptions(Type));
        }
        
      [HttpPost, Route("Chopterfilters")]
      public ActionResult<ProviderResponse> GetChopterOptions([FromBody] SearchCriteria Section)
      {
          return ReturnResponse(LibraryProvider.GetChopterOptions(Section));
      }
       [HttpPost, Route("details")]
      public ActionResult<ProviderResponse> GetLibrarydetails([FromBody] SearchCriteria Section)
       {
            return ReturnResponse(LibraryProvider.GetLibrarydetails(Section));
       }


        [HttpGet, Route("{LibraryId}")]
        public ActionResult<ProviderResponse> GetMaamarLinks(int LibraryId)
        {
            return ReturnResponse(LibraryProvider.GetMaamarLinks(LibraryId));
        }

        [HttpDelete, Route("{id}")]
        public ActionResult<ProviderResponse> DeleteMaamarLink(int id)
        {
            return ReturnResponse(LibraryProvider.DeleteMaamarLink(id));
        }
    }
}
