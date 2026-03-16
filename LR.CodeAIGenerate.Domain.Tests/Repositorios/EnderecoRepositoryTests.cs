using FluentAssertions;
using LR.CodeAIGenerate.Data;
using LR.CodeAIGenerate.Data.Validators;
using LR.CodeAIGenerate.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LR.CodeAIGenerate.Domain.Tests.Repositorios;

public class EnderecoRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly EnderecoRepository _repository;

    public EnderecoRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _repository = new EnderecoRepository(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task Incluir_ObterPorId_DeveFuncionar()
    {
        var pessoa = new Pessoa("Teste Repo", new DateTime(1990,1,1));
        await _context.Pessoas.AddAsync(pessoa);
        await _context.SaveChangesAsync();

        var endereco = new Endereco
        {
            CEP = "00000000",
            Logradouro = "Rua Teste",
            Bairro = "Bairro",
            Cidade = "Cidade",
            UF = "SP",
            IdPessoa = pessoa.Id
        };

        var criado = await _repository.IncluirAsync(endereco);

        var obtido = await _repository.ObterPorIdAsync(criado.Id);

        obtido.Should().NotBeNull();
        obtido!.CEP.Should().Be("00000000");
    }

    [Fact]
    public async Task ObterPorIdPessoa_DeveRetornarEndereco()
    {
        var pessoa = new Pessoa("Teste Repo 2", new DateTime(1992,2,2));
        await _context.Pessoas.AddAsync(pessoa);
        await _context.SaveChangesAsync();

        var endereco = new Endereco
        {
            CEP = "11111111",
            Logradouro = "Rua Teste 2",
            Bairro = "Bairro 2",
            Cidade = "Cidade 2",
            UF = "RJ",
            IdPessoa = pessoa.Id
        };

        await _repository.IncluirAsync(endereco);

        var obtido = await _repository.ObterPorIdPessoaAsync(pessoa.Id);

        obtido.Should().NotBeNull();
        obtido!.IdPessoa.Should().Be(pessoa.Id);
    }

    [Fact]
    public async Task Atualizar_DeveAlterarDados()
    {
        var pessoa = new Pessoa("Teste Repo 3", new DateTime(1993,3,3));
        await _context.Pessoas.AddAsync(pessoa);
        await _context.SaveChangesAsync();

        var endereco = new Endereco
        {
            CEP = "22222222",
            Logradouro = "Rua Teste 3",
            Bairro = "Bairro 3",
            Cidade = "Cidade 3",
            UF = "MG",
            IdPessoa = pessoa.Id
        };

        var criado = await _repository.IncluirAsync(endereco);

        criado.Logradouro = "Rua Teste 3 Alterada";

        await _repository.AtualizarAsync(criado);

        var obtido = await _repository.ObterPorIdAsync(criado.Id);

        obtido!.Logradouro.Should().Be("Rua Teste 3 Alterada");
    }

    [Fact]
    public async Task Excluir_DeveRemover()
    {
        var pessoa = new Pessoa("Teste Repo 4", new DateTime(1994,4,4));
        await _context.Pessoas.AddAsync(pessoa);
        await _context.SaveChangesAsync();

        var endereco = new Endereco
        {
            CEP = "33333333",
            Logradouro = "Rua Teste 4",
            Bairro = "Bairro 4",
            Cidade = "Cidade 4",
            UF = "BA",
            IdPessoa = pessoa.Id
        };

        var criado = await _repository.IncluirAsync(endereco);

        await _repository.ExcluirAsync(criado.Id);

        var obtido = await _repository.ObterPorIdAsync(criado.Id);

        obtido.Should().BeNull();
    }
}
