using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NesivosHakodesh.Comman;
using NesivosHakodesh.Domain.Entities;
using NesivosHakodesh.Providers.Topics;
using NesivosHakodesh.Providers.Utils.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NesivosHakodesh.Controllers
{
    [ApiController]
    [Route("api/Categories")]
    public class CategoriesController : BaseApiController
    {
        [HttpPost, Route("search")]
        public ActionResult<ProviderResponse> GetAllCategories([FromBody] SearchCriteria search)
        {
            return ReturnResponse(CategoriesProvider.GetAllCategories(search));
        }

      /*  [Authorize(Roles = "TOPICS_VIEW")]
        [HttpGet, Route("{id}")]
        public ActionResult<ProviderResponse> GetTopics(int id)
        {
            return ReturnResponse(CategoriesProvider.GetTopic(id));
        }*/

        [Authorize(Roles = "TOPICS_EDIT")]
        [HttpPost]
        public ActionResult<ProviderResponse> AddCategories([FromBody] Category topic)
        {
            return ReturnResponse(CategoriesProvider.AddCategories(topic));
        }

        [Authorize(Roles = "TOPICS_EDIT")]
        [HttpPut]
        public ActionResult<ProviderResponse> UpdateCategories([FromBody] Category topic)
        {
            return ReturnResponse(CategoriesProvider.UpdateCategories(topic));
        }

        [Authorize(Roles = "TOPICS_EDIT")]
        [HttpDelete, Route("{id}")]
        public ActionResult<ProviderResponse> DeleteCategories(int id)
        {
            return ReturnResponse(CategoriesProvider.DeleteCategories(id));
        }
    }
}
