using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using MoneyTrackerApp.DTO;
using MoneyTrackerApp.Models;

namespace MoneyTrackerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configure;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configure)
        {
            this.userManager = userManager;
            this.configure = configure;
        }


        [HttpPost("Register")]
        public async Task<ActionResult<GeneralResponse>> Register(RegisterDTO registerFromReq)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser UserEmailFromDb = await userManager.FindByEmailAsync(registerFromReq.Email);
                if (UserEmailFromDb != null)
                {
                    return new GeneralResponse()
                    {
                        IsPass = false,
                        Data = "Email address is already taken"
                    };
                }
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = registerFromReq.UserName,
                    Email = registerFromReq.Email,
                    PhoneNumber = registerFromReq.PhoneNumber
                };
                IdentityResult result = await userManager.CreateAsync(user, registerFromReq.Password);
                if (result.Succeeded)
                {
                    return new GeneralResponse()
                    {
                        IsPass = true,
                        Data = "Account Create Success"
                    };
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("newErrors", item.Description);

                    }
                    return new GeneralResponse()
                    {
                        IsPass = false,
                        Data = ModelState["newErrors"].Errors
                    };
                }
            }
            return new GeneralResponse()
            {
                IsPass = false,
                Data = ModelState["errors"]
            };
        }



        [HttpPost("login")]
        public async Task<ActionResult<GeneralResponse>> Login(LoginDTO loginFromRequest)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(loginFromRequest.UserName) && string.IsNullOrEmpty(loginFromRequest.Email))
                {
                    return new GeneralResponse()
                    {
                        IsPass = false,
                        Data = "The Username/Email field is required"
                    };
                }
                ApplicationUser user = await userManager.FindByNameAsync(loginFromRequest.UserName ?? "");
                if (user == null)
                {
                    user = await userManager.FindByEmailAsync(loginFromRequest.Email);
                }
                if (user != null)
                {
                    bool found = await userManager.CheckPasswordAsync(user, loginFromRequest.Password);
                    if (found)
                    {
                        var userRoles = await userManager.GetRolesAsync(user);
                        List<Claim> claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        if (userRoles != null)
                        {
                            foreach (var role in userRoles)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, role));
                            }
                        }
                        SymmetricSecurityKey signinkey = new(Encoding.UTF8.GetBytes(configure["JWT:Key"]));

                        SigningCredentials signingCredentials =
                            new SigningCredentials(signinkey, SecurityAlgorithms.HmacSha256);
                        JwtSecurityToken token = new JwtSecurityToken(
                            issuer: configure["JWT:Iss"],
                            audience: configure["JWT:Aud"],
                            expires: DateTime.Now.AddDays(15),
                            claims: claims,
                            signingCredentials: signingCredentials
                            );
                        return new GeneralResponse()
                        {
                            IsPass = true,
                            Data = new
                            {
                                expired = token.ValidTo,
                                token = new JwtSecurityTokenHandler().WriteToken(token)
                            }
                        };
                    }

                }
                ModelState.AddModelError("newError", "Invalid Account");
                return new GeneralResponse()
                {
                    IsPass = false,
                    Data = ModelState["newError"].Errors
                };
            }
            return new GeneralResponse()
            {
                IsPass = false,
                Data = ModelState["errors"]
            };

        }



        [HttpGet("U/{UserName:alpha}")]
        [Authorize]
        public async Task<ActionResult<GeneralResponse>> GetUserInfo(string UserName)
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null || currentUser.UserName != UserName)
            {
                return new GeneralResponse()
                {
                    IsPass = false,
                    Data = "Unauthorized access or user mismatch"
                };
            }

            ApplicationUser user = await userManager.FindByNameAsync(UserName ?? "");

            if (user != null)
            {
                return new GeneralResponse()
                {
                    IsPass = true,
                    Data = new
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,

                    }
                };
            }
            return new GeneralResponse()
            {
                IsPass = false,
                Data = "No user found"
            };

        }




    }
}

