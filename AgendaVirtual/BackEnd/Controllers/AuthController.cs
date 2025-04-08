using Azure;
using BackEnd.DTO;
using BackEnd.Services.Implementations;
using BackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {


        private readonly UserManager<IdentityUser> userManager;
        private ITokenService TokenService;

        public AuthController(UserManager<IdentityUser> userManager,
                                ITokenService tokenService)
        {
            this.userManager = userManager;
            this.TokenService = tokenService;

        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {


            IdentityUser user = await userManager.FindByNameAsync(model.Username);
            LoginDTO Usuario = new LoginDTO();
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                int IdUsuario = TokenService.GetIdUser(user.Id);
                var userRoles = await userManager.GetRolesAsync(user);

                var jwtToken = TokenService.GenerateToken(user, userRoles.ToList());

                Usuario.Token = jwtToken;
                Usuario.Roles = userRoles.ToList();
                Usuario.Username = user.UserName;
                Usuario.IdUsuario = IdUsuario;
                return Ok(Usuario);
            }
            

            return Unauthorized();

        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {

            var userExists = await userManager.FindByNameAsync(model.Username);

            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            IdentityUser user = new IdentityUser
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);

            }

            return Ok();

        }

    }
}