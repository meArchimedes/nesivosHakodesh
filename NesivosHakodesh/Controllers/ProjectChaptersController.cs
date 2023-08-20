using Microsoft.AspNetCore.Mvc;
using NesivosHakodesh.Comman;
using NesivosHakodesh.Domain.Entities;
using NesivosHakodesh.Providers.Project;
using NesivosHakodesh.Providers.Utils.Api;

namespace NesivosHakodesh.Controllers
{
    [ApiController]
    [Route("api/Projects/ProjectChapters")]
    public class ProjectChaptersController : BaseApiController
    {
        [HttpGet]
        public ActionResult<ProviderResponse> GetAllProjectChapters([FromBody] SearchCriteria search)
        {
            return ReturnResponse(ProjectChaptersProvider.GetAllProjectChapters(search));
        }
        [HttpGet, Route("{id}")]
        public ActionResult<ProviderResponse> GetProjectChapter(int id)
        {
            return ReturnResponse(ProjectChaptersProvider.GetProjectChapter(id));
        }

        [HttpPost]
        public ActionResult<ProviderResponse> AddProjectChapter([FromBody] ProjectChapter projectChapter)
        {
            return ReturnResponse(ProjectChaptersProvider.AddProjectChapter(projectChapter));
        }

        [HttpPut, Route("{id}")]
        public ActionResult<ProviderResponse> UpdateProjectChapter(int id, [FromBody] ProjectChapter projectChapter)
        {
            return ReturnResponse(ProjectChaptersProvider.UpdateProjectChapter(id, projectChapter));
        }

        [HttpDelete, Route("{id}")]
        public ActionResult<ProviderResponse> DeleteProjectChapter(int id)
        {
            return ReturnResponse(ProjectChaptersProvider.DeleteProjectChapter(id));
        }
    }
}
