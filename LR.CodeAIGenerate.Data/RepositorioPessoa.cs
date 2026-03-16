using FluentValidation;
using LR.CodeAIGenerate.Business.Interfaces;
using LR.CodeAIGenerate.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LR.CodeAIGenerate.Data;

/// <summary>
/// Implementação do repositório de Pessoa com validação FluentValidation antes de persistir.
/// </summary>
public class RepositorioPessoa : IRepositorioPessoa
{
    private readonly Repositorio<Pessoa, Guid> _repositorio;
    private readonly IValidator<Pessoa> _validator;
    private readonly AppDbContext _context;

    public RepositorioPessoa(AppDbContext context, IValidator<Pessoa> validator)
    {
        _repositorio = new Repositorio<Pessoa, Guid>(context);
        _validator = validator;
        _context = context;
    }

    /// <inheritdoc />
    public async Task<Pessoa?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Pessoas
            .Include(p => p.Endereco)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Pessoa>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Pessoas
            .Include(p => p.Endereco)
            .AsNoTracking()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<Pessoa> IncluirAsync(Pessoa entidade, CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(entidade, cancellationToken).ConfigureAwait(false);
        return await _repositorio.IncluirAsync(entidade, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task AtualizarAsync(Pessoa entidade, CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(entidade, cancellationToken).ConfigureAwait(false);
        await _repositorio.AtualizarAsync(entidade, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public Task ExcluirAsync(Guid id, CancellationToken cancellationToken = default) =>
        _repositorio.ExcluirAsync(id, cancellationToken);
}
