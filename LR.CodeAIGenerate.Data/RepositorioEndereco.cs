using LR.CodeAIGenerate.Business.Interfaces;
using LR.CodeAIGenerate.Domain.Modelos;
using Microsoft.EntityFrameworkCore;

namespace LR.CodeAIGenerate.Data;

public class EnderecoRepository : IEnderecoRepository
{
    private readonly Repositorio<Endereco, Guid> _repositorio;
    private readonly AppDbContext _context;

    public EnderecoRepository(AppDbContext context)
    {
        _context = context;
        _repositorio = new Repositorio<Endereco, Guid>(context);
    }

    public Task<Endereco?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _repositorio.ObterPorIdAsync(id, cancellationToken);

    public Task<IReadOnlyList<Endereco>> ObterTodosAsync(CancellationToken cancellationToken = default) =>
        _repositorio.ObterTodosAsync(cancellationToken);

    public async Task<Endereco> IncluirAsync(Endereco entidade, CancellationToken cancellationToken = default)
    {
        await _context.Set<Endereco>().AddAsync(entidade, cancellationToken).ConfigureAwait(false);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return entidade;
    }

    public async Task AtualizarAsync(Endereco entidade, CancellationToken cancellationToken = default)
    {
        _context.Set<Endereco>().Update(entidade);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public Task ExcluirAsync(Guid id, CancellationToken cancellationToken = default) =>
        _repositorio.ExcluirAsync(id, cancellationToken);

    public async Task<Endereco?> ObterPorIdPessoaAsync(Guid idPessoa, CancellationToken cancellationToken = default)
    {
        return await _context.Enderecos.AsNoTracking().FirstOrDefaultAsync(e => e.IdPessoa == idPessoa, cancellationToken).ConfigureAwait(false);
    }
}
