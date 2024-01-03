using kanban.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace kanbanBoard
{
    public interface IJWTAuthenticationManager
    {
        string Authenticate(string username);
    }

    public class JWTAuthenticationManager : IJWTAuthenticationManager
    {
        //Conexão com o DataBase
        kanbanContext context = new kanbanContext();


        private readonly string tokenKey;

        public JWTAuthenticationManager(string tokenKey)
        {
            //This se refere ao fiedl da própria classe e o tokenkey que recebe é o que vem como parâmetro token no program.cs
            this.tokenKey = tokenKey;
            this.context = new kanbanContext();
        }
        public string Authenticate(string username)
        {
            var user = context.Users.FirstOrDefault(u => u.Name == username);

            if (user == null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
