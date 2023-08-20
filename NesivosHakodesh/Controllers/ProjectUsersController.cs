using Microsoft.AspNetCore.Mvc;
using NesivosHakodesh.Comman;
using NesivosHakodesh.Domain.Entities;
using NesivosHakodesh.Providers.Project;
using NesivosHakodesh.Providers.Utils.Api;

namespace NesivosHakodesh.Controllers
{
    [ApiController]
    [Route("api/Projects/ProjectUsers")]
    public class ProjectUsersController : BaseApiController
    {
        [HttpGet]
        public ActionResult<ProviderResponse> GetAllProjectUsers([FromBody] SearchCriteria search)
        {
            return ReturnResponse(ProjectUsersProvider.GetAllProjectUsers(search));
        }
        [HttpGet, Route("{id}")]
        public ActionResult<ProviderResponse> GetProjectUser(int id)
        {
            return ReturnResponse(ProjectUsersProvider.GetProjectUser(id));
        }

        [HttpPost]
        public ActionResult<ProviderResponse> AddProjectUser([FromBody] ProjectUser sefurim)
        {
            return ReturnResponse(ProjectUsersProvider.AddProjectUser(sefurim));
        }

        [HttpPut, Route("{id}")]
        public ActionResult<ProviderResponse> UpdateProjectUser(int id, [FromBody] ProjectUser projectUser)
        {
            return ReturnResponse(ProjectUsersProvider.UpdateProjectUser(id, projectUser));
        }

        [HttpDelete, Route("{id}")]
        public ActionResult<ProviderResponse> DeleteProjectUser(int id)
        {
            return ReturnResponse(ProjectUsersProvider.DeleteProjectUser(id));
        }
    }
}
