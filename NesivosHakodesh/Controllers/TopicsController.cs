using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NesivosHakodesh.Comman;
using NesivosHakodesh.Domain.Entities;
using NesivosHakodesh.Providers.Topics;
using NesivosHakodesh.Providers.Utils.Api;

namespace NesivosHakodesh.Controllers
{
    [ApiController]
    [Route("api/Topics")]
    public class TopicsController : BaseApiController
    {
        [HttpPost, Route("search")]
        public ActionResult<ProviderResponse> GetAllTopics([FromBody] SearchCriteria search)
        {
            return ReturnResponse(TopicsProvider.GetAllTopics(search));
        }

        [Authorize(Roles = "TOPICS_VIEW")]
        [HttpGet, Route("{id}")]
        public ActionResult<ProviderResponse> GetTopics(int id)
        {
            return ReturnResponse(TopicsProvider.GetTopic(id));
        }

        [Authorize(Roles = "TOPICS_EDIT")]
        [HttpPost]
        public ActionResult<ProviderResponse> AddTopic([FromBody] Topic topic)
        {
            return ReturnResponse(TopicsProvider.AddTopic(topic));
        }

        [Authorize(Roles = "TOPICS_EDIT")]
        [HttpPut]
        public ActionResult<ProviderResponse> UpdateTopics( [FromBody] Topic topic)
        {
            return ReturnResponse(TopicsProvider.UpdateTopic( topic));
        }

        [Authorize(Roles = "TOPICS_EDIT")]
        [HttpDelete, Route("{id}")]
        public ActionResult<ProviderResponse> DeleteTopics(int id)
        {
            return ReturnResponse(TopicsProvider.DeleteTopic(id));
        }

        [Authorize(Roles = "TOPICS_VIEW")]
        [HttpGet, Route("Seforim")]
        public ActionResult<ProviderResponse> GetSeforimTopics()
        {
            return ReturnResponse(TopicsProvider.GetSeforimTopics());
        }

        [Authorize(Roles = "TOPICS_VIEW")]
        [HttpGet, Route("List")]
        public ActionResult<ProviderResponse> GetTopicsForDropDown()
        {
            return ReturnResponse(TopicsProvider.GetTopicsForDropDown());
        }

    }
}
