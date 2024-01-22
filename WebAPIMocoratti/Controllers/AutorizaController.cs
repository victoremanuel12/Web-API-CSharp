using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPIMocoratti.DTOs;

namespace WebAPIMocoratti.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizaController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _singInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        public AutorizaController(
            SignInManager<IdentityUser> singInManager,
            UserManager<IdentityUser> userManager,
            IConfiguration configuration)
        {
            _singInManager = singInManager;
            _userManager = userManager;
            _configuration = configuration;
        }
        [HttpGet]
        public ActionResult<string> Autoriza()
        {
            return $"AutorizaController :: Acessando em : {DateTime.Now.ToLongDateString()}";
        }
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] usuarioDTO usuario)
        {
            var user = new IdentityUser
            {
                UserName = usuario.Email,
                Email = usuario.Email,
                EmailConfirmed = true,
            };
            var result = await _userManager.CreateAsync(user, usuario.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            await _singInManager.SignInAsync(user, false);
            return Ok(GerarToken(usuario));
        }
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] usuarioDTO usuario)
        {

            var result = await _singInManager.PasswordSignInAsync(usuario.Email, usuario.Password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Login Invalido");
                return BadRequest(ModelState);
            }

            return Ok(GerarToken(usuario));
        }
        private UserTokenDTO GerarToken(usuarioDTO usuario)
        {
            //definir declarações do usuário.
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName,usuario.Email),
                new Claim("meuPet","Docinho"),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            // Gerar uma chave com base em um algoritmo simétrico
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
            //Gera  uma assinatura  Digital do token usando o algoritmo Hmac e a chave privada
            var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //Tempo de expiração do token
            var expiracao = _configuration["TokenConfiguration:ExpireHours"];
            var expiration = DateTime.UtcNow.AddHours(double.Parse(expiracao));
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["TokenConfiguration:Issuer"],
                audience: _configuration["TokenConfiguration:Audience"],
                expires: expiration,
                signingCredentials: credenciais
            );
            return new UserTokenDTO()
            {
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Message = "Token_access OK",


            };
        }
    }
}
