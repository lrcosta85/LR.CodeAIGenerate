using FluentValidation;
using LR.CodeAIGenerate.Business.Interfaces;
using LR.CodeAIGenerate.Domain.Modelos;

namespace LR.CodeAIGenerate.Data;

/// <summary>
/// Implementação do repositório de Pessoa com validação FluentValidation antes de persistir.
/// </summary>
public class RepositorioPessoa : IRepositorioPessoa
{
    private readonly Repositorio<Pessoa, Guid> _repositorio;
    private readonly IValidator<Pessoa> _validator;

    public RepositorioPessoa(AppDbContext context, IValidator<Pessoa> validator)
    {
        _repositorio = new Repositorio<Pessoa, Guid>(context);
        _validator = validator;
    }

    /// <inheritdoc />
    public Task<Pessoa?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _repositorio.ObterPorIdAsync(id, cancellationToken);

    /// <inheritdoc />
    public Task<IReadOnlyList<Pessoa>> ObterTodosAsync(CancellationToken cancellationToken = default) =>
        _repositorio.ObterTodosAsync(cancellationToken);

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
