using System.Net.Mime;
using LR.CodeAIGenerate.Business.Interfaces;
using LR.CodeAIGenerate.Domain.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LR.CodeAIGenerate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class PessoaController : ControllerBase
{
    private readonly IRepositorioPessoa _repositorioPessoa;

    public PessoaController(IRepositorioPessoa repositorioPessoa)
    {
        _repositorioPessoa = repositorioPessoa;
    }

    /// <summary>
    /// Obtém todas as pessoas.
    /// </summary>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<Pessoa>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Pessoa>>> GetAsync(CancellationToken cancellationToken)
    {
        var pessoas = await _repositorioPessoa.ObterTodosAsync(cancellationToken).ConfigureAwait(false);
        return Ok(pessoas);
    }

    /// <summary>
    /// Obtém uma pessoa pelo identificador.
    /// </summary>
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(Pessoa), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Pessoa>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var pessoa = await _repositorioPessoa.ObterPorIdAsync(id, cancellationToken).ConfigureAwait(false);
        if (pessoa is null)
            return NotFound();

        return Ok(pessoa);
    }

    /// <summary>
    /// Cria uma nova pessoa.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "add-pessoa")]
    [ProducesResponseType(typeof(Pessoa), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pessoa>> PostAsync([FromBody] Pessoa pessoa, CancellationToken cancellationToken)
    {
        var criada = await _repositorioPessoa.IncluirAsync(pessoa, cancellationToken).ConfigureAwait(false);
        //return CreatedAtAction(nameof(GetByIdAsync), new { id = criada.Id }, criada);
        return Ok(pessoa);
    }

    /// <summary>
    /// Atualiza uma pessoa existente.
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutAsync(Guid id, [FromBody] Pessoa pessoa, CancellationToken cancellationToken)
    {
        if (id != pessoa.Id)
            return BadRequest("O id da rota é diferente do id do corpo da requisição.");

        var existente = await _repositorioPessoa.ObterPorIdAsync(id, cancellationToken).ConfigureAwait(false);
        if (existente is null)
            return NotFound();

        existente.Nome = pessoa.Nome;
        existente.DataNascimento = pessoa.DataNascimento;

        await _repositorioPessoa.AtualizarAsync(existente, cancellationToken).ConfigureAwait(false);

        return Ok(pessoa);
    }

    /// <summary>
    /// Remove uma pessoa.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _repositorioPessoa.ExcluirAsync(id, cancellationToken).ConfigureAwait(false);
        return Ok();
    }
}

