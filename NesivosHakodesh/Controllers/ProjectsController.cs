using Microsoft.AspNetCore.Mvc;
using NesivosHakodesh.Comman;
using NesivosHakodesh.Domain.Entities;
using NesivosHakodesh.Providers.Projects;
using NesivosHakodesh.Providers.Utils.Api;

namespace NesivosHakodesh.Controllers
{
    [ApiController]
    [Route("api/Projects")]
    public class ProjectsController : BaseApiController
    {
        [HttpGet]
        public ActionResult<ProviderResponse> GetAllProjects([FromBody] SearchCriteria search)
        {
            return ReturnResponse(ProjectsProvider.GetAllProjects(search));
        }
        [HttpGet, Route("{id}")]
        public ActionResult<ProviderResponse> GetProject(int id)
        {
            return ReturnResponse(ProjectsProvider.GetProject(id));
        }

        [HttpPost]
        public ActionResult<ProviderResponse> AddProject([FromBody] Project Project)
        {
            return ReturnResponse(ProjectsProvider.AddProject(Project));
        }

        [HttpPut, Route("{id}")]
        public ActionResult<ProviderResponse> UpdateProject(int id, [FromBody] Project Project)
        {
            return ReturnResponse(ProjectsProvider.UpdateProject(id, Project));
        }

        [HttpDelete, Route("{id}")]
        public ActionResult<ProviderResponse> DeleteProject(int id)
        {
            return ReturnResponse(ProjectsProvider.DeleteProject(id));
        }
    }
}
