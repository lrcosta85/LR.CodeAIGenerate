namespace LR.CodeAIGenerate.Business.Interfaces;

/// <summary>
/// Interface genérica para repositório com operações CRUD.
/// </summary>
/// <typeparam name="T">Tipo da entidade.</typeparam>
/// <typeparam name="TKey">Tipo da chave da entidade.</typeparam>
public interface IRepositorio<T, TKey> where T : class
{
    /// <summary>
    /// Obtém uma entidade pelo identificador.
    /// </summary>
    Task<T?> ObterPorIdAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém todas as entidades.
    /// </summary>
    Task<IReadOnlyList<T>> ObterTodosAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Adiciona uma nova entidade.
    /// </summary>
    Task<T> IncluirAsync(T entidade, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza uma entidade existente.
    /// </summary>
    Task AtualizarAsync(T entidade, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove uma entidade pelo identificador.
    /// </summary>
    Task ExcluirAsync(TKey id, CancellationToken cancellationToken = default);
}
