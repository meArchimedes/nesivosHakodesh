using Microsoft.AspNetCore.Mvc;
using NesivosHakodesh.Comman;
using NesivosHakodesh.Domain.Entities;
using NesivosHakodesh.Providers.Project;
using NesivosHakodesh.Providers.Utils.Api;

namespace NesivosHakodesh.Controllers
{
    [ApiController]
    [Route("api/Projects/ProjectAssignment")]
    public class ProjectAssignmentController : BaseApiController
    {
        [HttpGet]
        public ActionResult<ProviderResponse> GetAllProjectAssignment([FromBody] SearchCriteria search)
        {
            return ReturnResponse(ProjectAssignmentProvider.GetAllProjectAssignments(search));
        }

        [HttpGet, Route("{id}")]
        public ActionResult<ProviderResponse> GetProjectAssignment(int id)
        {
            return ReturnResponse(ProjectAssignmentProvider.GetProjectAssignment(id));
        }

        [HttpPost]
        public ActionResult<ProviderResponse> AddProjectAssignment([FromBody] ProjectAssignment projectAssignment)
        {
            return ReturnResponse(ProjectAssignmentProvider.AddProjectAssignment(projectAssignment));
        }

        [HttpPut, Route("{id}")]
        public ActionResult<ProviderResponse> UpdateProjectAssignment(int id, [FromBody] ProjectAssignment projectAssignment)
        {
            return ReturnResponse(ProjectAssignmentProvider.UpdateProjectAssignment(id, projectAssignment));
        }

        [HttpDelete, Route("{id}")]
        public ActionResult<ProviderResponse> DeleteProjectAssignment(int id)
        {
            return ReturnResponse(ProjectAssignmentProvider.DeleteProjectAssignment(id));
        }
    }
}
