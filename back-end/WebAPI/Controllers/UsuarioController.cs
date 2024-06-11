using Aplicacao.Interfaces;
using Entidades.Entidades;
using Entidades.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using WebAPI.Models;
using WebAPI.Token;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsuarioController : ControllerBase
{
    // pode ser utilizado para novas funcionalidades, como inativar usuario e etc.
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IAplicacaoUsuario _IAplicacaoUsuario;


    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public UsuarioController(IAplicacaoUsuario IAplicacaoUsuario, SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager, ILogger<WeatherForecastController> logger)
    {
        _IAplicacaoUsuario = IAplicacaoUsuario;
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }
    
    [AllowAnonymous]
    [Produces("application/json")]
    [HttpPost("/User/Create")]
    public async Task<IActionResult> AdicionaUsuarioIdentity([FromBody] Login login)
    {
        if (string.IsNullOrWhiteSpace(login.email) || string.IsNullOrWhiteSpace(login.senha))
            return Ok("Falta alguns dados");

        var user = new ApplicationUser
        {
            UserName = login.email,
            Email = login.email,
            Celular = login.celular,
            Tipo = TipoUsuario.Comum,
        };
        var resultado = await _userManager.CreateAsync(user, login.senha);

        if (resultado.Errors.Any())
        {
            return BadRequest(resultado.Errors);
        }

        // Geração de Confirmação caso precise
        var userId = await _userManager.GetUserIdAsync(user);
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        // retorno email 
        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var resultado2 = await _userManager.ConfirmEmailAsync(user, code);

        if (resultado2.Succeeded)
            return Ok("Usuário Adicionado com Sucesso");
        else
            return Ok("Erro ao confirmar usuários");
    }

    [AllowAnonymous]
    [Produces("application/json")]
    [HttpPost("/User/Token")]
    public async Task<IActionResult> CriarTokenIdentity([FromBody] Login login)
    {
        if (string.IsNullOrWhiteSpace(login.email) || string.IsNullOrWhiteSpace(login.senha))
            return Unauthorized();

        var resultado = await
            _signInManager.PasswordSignInAsync(login.email, login.senha, false, lockoutOnFailure: false);

        if (resultado.Succeeded)
        {
            // muda no program.cs tambem
            var key = "Secret_Key-12345678_Secret_Key-12345678";

            var idUsuario = await _IAplicacaoUsuario.RetornaIdUsuario(login.email);

            var token = new TokenJWTBuilder()
                 .AddSecurityKey(JwtSecurityKey.Create(key))
             .AddSubject("Empresa - Canal Dev Net Core")
             .AddIssuer("Teste.Securiry.Bearer")
             .AddAudience("Teste.Securiry.Bearer")
             .AddClaim("idUsuario", idUsuario)
             .AddExpiry(5)
             .Builder();

            return Ok(token.value);
        }
        else
        {
            return Unauthorized();
        }

    }
}
