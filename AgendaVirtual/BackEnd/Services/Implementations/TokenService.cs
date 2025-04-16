using BackEnd.DTO;
using BackEnd.Services.Interfaces;
using DAL.Implementations;
using DAL.Interfaces;
using Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackEnd.Services.Implementations
{
    public class TokenService : ITokenService
    {

        IConfiguration _configuration;
        IUsuarioDAL _usuarioDAL;

        public TokenService(IConfiguration configuration, IUsuarioDAL usuarioDAL)
        {
            _configuration = configuration;
            _usuarioDAL = usuarioDAL;
        }

        public TokenDTO GenerateToken(IdentityUser user, List<string> roles)
        {
            int idUsuario = GetIdUser(user.Id);
            var authClaims = new List<Claim>
             {
                 new Claim(ClaimTypes.Name, user.UserName),
                 new Claim("id_usuario", idUsuario.ToString()),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             };

            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new TokenDTO
            {
                Token = tokenString,
                Expiration = token.ValidTo
            };
        }
        public int GetIdUser(string id)
        {
            try
            {
                return _usuarioDAL.GetIdUser(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al obtener el ID del usuario: {ex.Message}");
                throw;
            }
        }
    }
}