using Api.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly IRolRepository _rolRepository;

        public RolController(IRolRepository rolRepository)
        {
            this._rolRepository = rolRepository;
        }
        [HttpGet("token")]
        [AllowAnonymous]
        public IActionResult GetToken()
        {
            var token = GenerateJwtToken();
            return Ok(new { token });
        }
        
        [HttpGet("Roles")]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var result = await _rolRepository.GetRol();
            return Ok(result);
        }
        private string GenerateJwtToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("this is my custom Secret key for authenticatio");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("userId", "1234") }),
                Expires = DateTime.UtcNow.AddSeconds(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
