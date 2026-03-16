using System.Net.Mime;
using LR.CodeAIGenerate.Business.Interfaces;
using LR.CodeAIGenerate.Domain.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LR.CodeAIGenerate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class EnderecoController : ControllerBase
{
    private readonly IEnderecoRepository _repositorio;

    public EnderecoController(IEnderecoRepository repositorio)
    {
        _repositorio = repositorio;
    }

    [HttpPost]
    [Authorize(Policy = "endereco")]
    public async Task<ActionResult<Endereco>> PostAsync([FromBody] Endereco endereco, CancellationToken cancellationToken)
    {
        var criada = await _repositorio.IncluirAsync(endereco, cancellationToken).ConfigureAwait(false);
        return Ok(criada);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = "endereco")]
    public async Task<ActionResult<Endereco>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var e = await _repositorio.ObterPorIdAsync(id, cancellationToken).ConfigureAwait(false);
        if (e is null) return NotFound();
        return Ok(e);
    }

    [HttpGet("pessoa/{idPessoa:guid}")]
    [Authorize(Policy = "endereco")]
    public async Task<ActionResult<Endereco>> GetByPessoaAsync(Guid idPessoa, CancellationToken cancellationToken)
    {
        var e = await _repositorio.ObterPorIdPessoaAsync(idPessoa, cancellationToken).ConfigureAwait(false);
        if (e is null) return NotFound();
        return Ok(e);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "endereco")]
    public async Task<IActionResult> PutAsync(Guid id, [FromBody] Endereco endereco, CancellationToken cancellationToken)
    {
        if (id != endereco.Id) return BadRequest("O id da rota é diferente do id do corpo da requisição.");

        var existente = await _repositorio.ObterPorIdAsync(id, cancellationToken).ConfigureAwait(false);
        if (existente is null) return NotFound();

        existente.CEP = endereco.CEP;
        existente.Logradouro = endereco.Logradouro;
        existente.Complemento = endereco.Complemento;
        existente.Bairro = endereco.Bairro;
        existente.Cidade = endereco.Cidade;
        existente.UF = endereco.UF;

        await _repositorio.AtualizarAsync(existente, cancellationToken).ConfigureAwait(false);

        return Ok(existente);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "endereco")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _repositorio.ExcluirAsync(id, cancellationToken).ConfigureAwait(false);
        return Ok();
    }
}
