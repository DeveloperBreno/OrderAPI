using Aplicacao.Interfaces;
using Dominio.Interfaces;
using Dominio.Interfaces.Filas;
using Entidades.Entidades;
using Entidades.Enums;
using Insfraestrutura.Filas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using WebAPI.Models;
using WebAPI.Token;

namespace WebAPI.Controllers.v1;

[ApiController]
[Route("[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IAplicacaoUsuario _IAplicacaoUsuario;
    private readonly IInsereNaFila _IInsereNaFila;

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger _logger;

    public UsuarioController(IAplicacaoUsuario IAplicacaoUsuario, SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager, ILogger logger, IInsereNaFila insereNaFila)
    {
        _IAplicacaoUsuario = IAplicacaoUsuario;
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _IInsereNaFila = insereNaFila;
    }


    [AllowAnonymous]
    [Produces("application/json")]
    [HttpPost("/v1/User/Create")]
    public async Task<IActionResult> AdicionaUsuarioIdentity([FromBody] Login login)
    {

        var errorMessage = "";
             // Verifica se a senha é nula ou vazia
        if (string.IsNullOrWhiteSpace(login.senha))
        {
            errorMessage = "A senha não pode ser nula ou vazia.";
        }

        // Verifica o comprimento mínimo da senha
        if (login.senha.Length < 6)
        {
            errorMessage = "A senha deve ter pelo menos 6 caracteres.";
        }

        // Verifica se a senha contém pelo menos uma letra maiúscula
        if (!login.senha.Any(char.IsUpper))
        {
            errorMessage = "A senha deve conter pelo menos uma letra maiúscula.";
        }

        // Verifica se a senha contém pelo menos uma letra minúscula
        if (!login.senha.Any(char.IsLower))
        {
            errorMessage = "A senha deve conter pelo menos uma letra minúscula.";
        }

        // Verifica se a senha contém pelo menos um caractere especial
        if (!login.senha.Any(ch => !char.IsLetterOrDigit(ch)))
        {
            errorMessage = "A senha deve conter pelo menos um caractere especial.";
        }

        if (errorMessage.Length > 0)
        {
            return BadRequest(errorMessage);
        }

        var user = new ApplicationUser
        {
            UserName = login.userName,
            Email = login.email,
            Celular = login.celular,
            Tipo = TipoUsuario.Comum,
            NormalizedUserName = login.userName,
            PasswordHash = login.senha,
            DataDeNascimento = login.nascimento
        };

        _IInsereNaFila.Inserir(user, "InsertApplicationUser");

        return Ok("Usuário será inserido em breve");

    }


    [AllowAnonymous]
    [Produces("application/json")]
    [HttpPost("/v1/User/Token")]
    public async Task<IActionResult> CriarTokenIdentity([FromBody] Login login)
    {
        if (string.IsNullOrWhiteSpace(login.email) || string.IsNullOrWhiteSpace(login.senha))
            return Unauthorized();

        string userName = _IAplicacaoUsuario.RetornaUserName(login.email).Result;

        var resultado = await
            _signInManager.PasswordSignInAsync(userName, login.senha, false, lockoutOnFailure: false);

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
             .AddExpiry(172800000)
             .Builder();

            return Ok(token.value);
        }
        else
        {
            return Unauthorized();
        }

    }

    [Authorize]
    [Produces("application/json")]
    [HttpGet("/v1/User/Info")]
    public async Task<IActionResult> GetInfoAboutUser()
    {
        var email = User.Claims.FirstOrDefault().Subject.Name;
        var idUsuario = await _IAplicacaoUsuario.RetornaIdUsuario(email);

        var nomeDoUsuario = await _IAplicacaoUsuario.RetornaONomeDoUsuarioPorId(idUsuario);

        return Ok(new { name = nomeDoUsuario });
    }
}
