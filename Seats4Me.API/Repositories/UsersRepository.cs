using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Seats4Me.Data.Common;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Repositories
{
    public class UsersRepository : TheatreRepository
    {
        public UsersRepository(TheatreContext context) : base(context)
        {
        }

        public async Task<Seats4MeUser> GetAuthenticatedUserAsync(string email, string password)
        {
            return await _context.Seats4MeUsers.FirstOrDefaultAsync(u =>
                    u.Email.Equals(email.ToLower()) && u.Password.Equals(password));
        }

        public string GetToken(Seats4MeUser user, string jwtKey, string jwtIssuer)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            foreach (var role in user.Roles.Split(','))
            {
                claims.Add(new Claim(role.InitCap(), "True"));
            }

            var token = new JwtSecurityToken(jwtIssuer,
                jwtIssuer,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds,
                claims:claims
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
