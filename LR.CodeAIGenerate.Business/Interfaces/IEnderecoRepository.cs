using System;
using System.Threading;
using System.Threading.Tasks;
using LR.CodeAIGenerate.Domain.Modelos;

namespace LR.CodeAIGenerate.Business.Interfaces;

public interface IEnderecoRepository : IRepositorio<Endereco, Guid>
{
    Task<Endereco?> ObterPorIdPessoaAsync(Guid idPessoa, CancellationToken cancellationToken = default);
}
