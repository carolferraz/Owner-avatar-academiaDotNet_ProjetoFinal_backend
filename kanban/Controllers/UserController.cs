using Castle.Core.Internal;
using kanban.DataModels;
using kanban.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kanbanBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IJWTAuthenticationManager _jwt;
        private readonly kanbanContext _context;

        public UserController(IJWTAuthenticationManager jwt, kanbanContext context)
        {
            _jwt = jwt;
            _context = context; 
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthenticationDTO userCredentials)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userCredentials.Email);

                if (user == null)
                {
                    return NotFound("Usuário não encontrado");
                }

                bool validPassword = HashUtils.ValidaSenha(userCredentials.Password, user.Password);

                if (validPassword)
                {
                    string token = _jwt.Authenticate(user.Name);
                    return Ok(new { Token = token });
                }
                else
                {
                    return Unauthorized("Credenciais inválidas");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.AsNoTracking().ToListAsync();
            return users.Count == 0 ? NoContent() : Ok(users);
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                // Verificar se o email já existe na base de dados para evitar duplicados
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    return BadRequest("O e-mail fornecido já está em uso.");
                }

                string senhaHash = HashUtils.GeraSenhaHash(user.Password);
                user.Password = senhaHash;

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return Created("api/users/" + user.Id, user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] int id)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound("Usuário não encontrado");
            }

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserById([FromRoute] int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound("Usuário não encontrado");
            }

            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserById([FromRoute] int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (existingUser == null)
            {
                return NotFound("Usuário não encontrado");
            }

            try
            {
                existingUser.Name = user.Name;
                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}


