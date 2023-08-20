using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NesivosHakodesh.Core;
using NesivosHakodesh.Core.Config;
using NesivosHakodesh.Domain.Entities;
using NesivosHakodesh.Providers.Utils.Api;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NesivosHakodesh.Providers.Identity
{
    public class IdentityService
    {
        public static async Task<ProviderResponse> LoginUserAsync(User login)
        {
            ProviderResponse res = new ProviderResponse();

            UserManager<User> userManager = ThreadProperties.GetUserManager();

            var user = await userManager.FindByNameAsync(login.UserName);

            if (user == null)
            {
                res.Messages.Add("שם המשתמש או הסיסמה אינם חוקיים");
            }
            else
            {
                if (await userManager.CheckPasswordAsync(user, login.NewPassword))
                {
                    IdentitySettings identitySettings = AppSettingsProvider.GetIdentitySettings();

                    var key = Encoding.ASCII.GetBytes(identitySettings.TokenSecret);
                    var secretKey = new SymmetricSecurityKey(key);
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                    
                    //Setup Cliams
                    List<Claim> claimsList = new List<Claim> {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName ?? ""),
                        new Claim(ClaimTypes.Email, user.Email ?? ""),
                        new Claim("FirstName", user.FirstName ?? ""),
                        new Claim("LastName", user.LastName ?? ""),
                        new Claim("PhoneNumber", user.PhoneNumber ?? ""),
                        new Claim("Cell", user.Cell  ?? ""),
                        new Claim("Status", ((int)user.Status).ToString())
                    };

                    //add Roles
                    var roles = AppProvider.GetDBContext().UserRoles
                                                        .Where(x => x.UserId == user.Id)
                                                        .Include(x => x.Role)
                                                        .ToList();
                    foreach (var role in roles)
                    {
                        claimsList.Add(new Claim(ClaimTypes.Role, role.Role.Name));
                    }
                    
                    var tokeOptions = new JwtSecurityToken(
                        issuer: identitySettings.TokenIssuer,
                        audience: identitySettings.TokenAudience,
                        claims: claimsList,
                        expires: DateTime.Now.AddDays(30),
                        signingCredentials: signinCredentials
                    );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                    res.Data = tokenString;
                }
                else
                {
                    res.Messages.Add("שם המשתמש או הסיסמה אינם חוקיים");
                }
            }

            return res;
        }

        public static async Task<ProviderResponse> RegisterAsync(User user)
        {
            ProviderResponse res = new ProviderResponse();

            UserManager<User> userManager = ThreadProperties.GetUserManager();

            User newUser = new User { 
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Cell = user.Cell,
                Status = UserStatus.Active,
                NormalizedEmail = user.UserName,
                NormalizedUserName = user.UserName,
                PhoneNumber = user.PhoneNumber
            };



            IdentityResult result = await userManager.CreateAsync(newUser, user.NewPassword);

            if (result.Succeeded)
            {
                res.Data = user;
            }
            else
            {
                PopulateErrorMessages(result, res);
            }

            return res;
        }




        public static async Task<ProviderResponse> ChangePasswordForUser(int userId, string newPassword)
        {
            ProviderResponse res = new ProviderResponse();

            UserManager<User> userManager = ThreadProperties.GetUserManager();

            User user = await userManager.FindByIdAsync(userId.ToString());

            if(user == null)
            {
                res.Messages.Add("שגיאת שינוי סיסמה, משתמש לא חוקי");
            }
            else
            {
                string token = await userManager.GeneratePasswordResetTokenAsync(user);

                var result = await userManager.ResetPasswordAsync(user, token, newPassword);

                PopulateErrorMessages(result, res);
            }

            return res;
        }

        private static void PopulateErrorMessages(IdentityResult identityResult, ProviderResponse res)
        {
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError err in identityResult.Errors)
                {
                    res.Messages.Add(err.Description);
                }
            }
        }
    }
}
