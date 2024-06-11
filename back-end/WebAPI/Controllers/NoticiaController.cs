using Aplicacao.Interfaces;
using Entidades.Entidades;
using Entidades.Notificacoes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NoticiaController : ControllerBase
{
    private readonly IAplicacaoNoticia _aplicacaoNoticia;
    private readonly IAplicacaoUsuario _IAplicacaoUsuario;

    public NoticiaController(IAplicacaoNoticia aplicacaoNoticia, IAplicacaoUsuario iAplicacaoUsuario)
    {
        _aplicacaoNoticia = aplicacaoNoticia;
        _IAplicacaoUsuario = iAplicacaoUsuario;
    }

    [Authorize]
    [Produces("application/json")]
    [HttpPost("/Noticia/List")]
    public async Task<List<Noticia>> ListarNoticias()
    {
        return await _aplicacaoNoticia.ListarNoticiasAtivas();
    }

    [Authorize]
    [Produces("application/json")]
    [HttpPost("/Noticia/Create")]
    public async Task<List<Notifica>> Create([FromBody] NoticiaModel noticiaModel)
    {
        var novaNoticia = new Noticia
        {
            Titulo = noticiaModel.Titulo,
            Informacao = noticiaModel.Informacao
        };

        var email = User.Claims.FirstOrDefault().Subject.Name;
        var idUsuario = await _IAplicacaoUsuario.RetornaIdUsuario(email);
        novaNoticia.UsuarioId = idUsuario;

        await _aplicacaoNoticia.AdicionarNoticia(novaNoticia);

        return novaNoticia.Notificacoes;
    }

    [Authorize]
    [Produces("application/json")]
    [HttpPost("/Update")]
    public async Task<List<Notifica>> Update([FromBody] NoticiaModel noticiaModel)
    {
        var novaNoticia = await _aplicacaoNoticia.BuscarPorId(noticiaModel.idNoticia);
        novaNoticia.Titulo = noticiaModel.Titulo;
        novaNoticia.Informacao = noticiaModel.Informacao;
        var email = User.Claims.FirstOrDefault().Subject.Name;
        var idUsuario = await _IAplicacaoUsuario.RetornaIdUsuario(email);
        novaNoticia.UsuarioId = idUsuario;
        await _aplicacaoNoticia.AtualizaNoticia(novaNoticia);
        return novaNoticia.Notificacoes;
    }

    [Authorize]
    [Produces("application/json")]
    [HttpPost("/Delete")]
    public async Task<List<Notifica>> Delete([FromBody] NoticiaModel noticiaModel)
    {
        var novaNoticia = await _aplicacaoNoticia.BuscarPorId(noticiaModel.idNoticia);
        await _aplicacaoNoticia.Excluir(novaNoticia);
        return novaNoticia.Notificacoes;
    }

}
