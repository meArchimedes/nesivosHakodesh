using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NesivosHakodesh.Core.Config;
using NesivosHakodesh.Core.DB;
using NesivosHakodesh.Domain.Entities;
using NesivosHakodesh.Providers.Utils;
using NesivosHakodesh.Providers.Utils.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NesivosHakodesh.Controllers
{
   
        [Authorize]
        [Route("api")]
        [ServiceFilter(typeof(WebApiFilter))]
        [ApiController]
        public class BaseApiController : ControllerBase
        {
            [Route("xxxxxx1")]
            [ApiExplorerSettings(IgnoreApi = true)]
            public ActionResult ReturnResponse(ProviderResponse providerResponse)
            {
                try
                {
                    if (providerResponse.Success)
                    {
                        return Ok(providerResponse);
                    }
                    else
                    {
                        return BadRequest(providerResponse);
                    }

                }
                catch (Exception e)
                {
                    Logger.Log(e.ToString());

                    ProviderResponse errorRes = new ProviderResponse
                    {
                        Messages = new List<string> { "התרחשה שגיאה" }
                    };

                    return BadRequest(errorRes);
                }
            }

            [Route("xxxxxx2")]
            [ApiExplorerSettings(IgnoreApi = true)]
            public async Task<ActionResult> ReturnResponseAsync(Task<ProviderResponse> providerResponse)
            {
                try
                {
                    ProviderResponse res = await providerResponse;

                    if (res.Success)
                    {
                        return Ok(res);
                    }
                    else
                    {
                        return BadRequest(res);
                    }
                }
                catch (Exception e)
                {
                    Logger.Log(e.ToString());

                    ProviderResponse errorRes = new ProviderResponse
                    {
                        Messages = new List<string> { "התרחשה שגיאה" }
                    };

                    return BadRequest(errorRes);
                }
            }

            public class WebApiFilter : ActionFilterAttribute
            {
                public IConfiguration _configuration { get; }
                private UserManager<User> _userManger;
                private AppDBContext _dbContext;

                public WebApiFilter(IConfiguration configuration, UserManager<User> userManager, AppDBContext dbContext)
                {
                    _configuration = configuration;
                    _userManger = userManager;
                    _dbContext = dbContext;
                }

                public override void OnActionExecuting(ActionExecutingContext context)
                {
                    ThreadProperties.SetUserManager(_userManger);
                    ThreadProperties.SetConfiguration(_configuration);
                    ThreadProperties.SetDbContext(_dbContext);

                    if (context.HttpContext.User.Identity.IsAuthenticated)
                    {
                        var identity = context.HttpContext.User;


                        User currentUser = new User
                        {
                            Id = int.Parse(identity.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value),
                            UserName = identity.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value,
                            Email = identity.Claims.Where(x => x.Type == ClaimTypes.Email).FirstOrDefault()?.Value,
                            FirstName = identity.Claims.Where(x => x.Type == "FirstName").FirstOrDefault()?.Value,
                            LastName = identity.Claims.Where(x => x.Type == "LastName").FirstOrDefault()?.Value,
                            PhoneNumber = identity.Claims.Where(x => x.Type == "PhoneNumber").FirstOrDefault()?.Value,
                            Cell = identity.Claims.Where(x => x.Type == "Cell").FirstOrDefault()?.Value,
                            Status = (UserStatus)int.Parse(identity.Claims.Where(x => x.Type == "Status").FirstOrDefault()?.Value),
                            UserRoles = identity.Claims.Where(x => x.Type == ClaimTypes.Role).ToList().ConvertAll(x => new UserRole
                            {
                                Role = new Role
                                {
                                    Name = x.Value
                                }
                            }),
                        };
                        ThreadProperties.SetCurrentUser(currentUser);

                    /*User appUser = _userManger.GetUserAsync(context.HttpContext.User).Result;

                    if (appUser != null)
                    {
                        ThreadProperties.SetCurrentUser(appUser);
                    }*/
                }

                    base.OnActionExecuting(context);
                }
            }
        }
    
}
