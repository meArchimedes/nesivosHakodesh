using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NesivosHakodesh.Comman;
using NesivosHakodesh.Core;
using NesivosHakodesh.Domain.Entities;
using NesivosHakodesh.Providers.Users;
using NesivosHakodesh.Providers.Utils.Api;

namespace NesivosHakodesh.Controllers
{
    [ApiController]
    [Route("api/Users")]
    public class UserController : BaseApiController
    {
        [AllowAnonymous]
        [HttpPost, Route("login")]
        public async Task<ActionResult<ProviderResponse>> Login([FromBody] User user)
        {
            return await ReturnResponseAsync(UsersProvider.LoginUserAsync(user));
        }

        [HttpGet, Route("self")]
        public ActionResult<ProviderResponse> CurrentUser()
        {
            return ReturnResponse(new ProviderResponse
            {
                Data = AppProvider.GetCurrentUser()
            });
        }

        [HttpGet, Route("openUsers")]
        public ActionResult<ProviderResponse> GetAllUsersOpen()
        {
            return ReturnResponse(UsersProvider.GetAllUsersOpen());
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost, Route("search")]
        public ActionResult<ProviderResponse> GetAllUsers([FromBody] SearchCriteria search)
        {
            return ReturnResponse(UsersProvider.GetAllUsers(search));
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet, Route("{id}")]
        public ActionResult<ProviderResponse> GetUser(int id)
        {
            return ReturnResponse(UsersProvider.GetUser(id));
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<ActionResult<ProviderResponse>> AddUserAsync([FromBody] User user)
        {
            return await ReturnResponseAsync(UsersProvider.AddUserAsync(user));
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut]
        public async Task<ActionResult<ProviderResponse>> UpdateUserAsync([FromBody] User user)
        {
            return await ReturnResponseAsync(UsersProvider.UpdateUserAsync(user));
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete, Route("{id}")]
        public ActionResult<ProviderResponse> DeleteUser(int id)
        {
            return ReturnResponse(UsersProvider.DeleteUser(id));
        }
    }
}
