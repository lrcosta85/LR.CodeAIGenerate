using LR.CodeAIGenerate.Business.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LR.CodeAIGenerate.Data;

/// <summary>
/// Implementação genérica do repositório usando Entity Framework Core.
/// </summary>
public class Repositorio<T, TKey> : IRepositorio<T, TKey> where T : class
{
    private readonly AppDbContext _context;

    public Repositorio(AppDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public virtual async Task<T?> ObterPorIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().FindAsync(new object[] { id! }, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public virtual async Task<IReadOnlyList<T>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public virtual async Task<T> IncluirAsync(T entidade, CancellationToken cancellationToken = default)
    {
        await _context.Set<T>().AddAsync(entidade, cancellationToken).ConfigureAwait(false);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return entidade;
    }

    /// <inheritdoc />
    public virtual async Task AtualizarAsync(T entidade, CancellationToken cancellationToken = default)
    {
        _context.Set<T>().Update(entidade);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public virtual async Task ExcluirAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entidade = await ObterPorIdAsync(id, cancellationToken).ConfigureAwait(false);
        if (entidade is not null)
        {
            _context.Set<T>().Remove(entidade);
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
