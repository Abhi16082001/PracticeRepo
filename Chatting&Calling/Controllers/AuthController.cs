using Chatting_Calling.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http;

namespace Chatting_Calling.Controllers
{
    public class AuthController : ApiController
    {

        [HttpPost]
        [Route("api/Auth/Login")]
        public IHttpActionResult Login([FromBody] LoginRequest request)
        {
            using (var db = new ApplicationDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.UserName == request.Username && u.PasswordHash == request.Password);
                if (user == null) return Unauthorized();

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("ThisIsASuperLongSecretKey1234567890!@#"); // keep safe
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName)
                }),
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new { token = tokenHandler.WriteToken(token) });
            }
        }

    }
}
