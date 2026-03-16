using System;
using FluentAssertions;
using LR.CodeAIGenerate.Domain.Modelos;
using Xunit;

namespace LR.CodeAIGenerate.Domain.Tests.Modelos;

public class EnderecoTests
{
    private static readonly DateTime DataValida = new(1990, 5, 15);

    [Fact]
    public void CriacaoValida_DeveAceitarPropriedades()
    {
        var pessoa = new Pessoa("Carlos Silva", DataValida);
        var endereco = new Endereco
        {
            CEP = "12345678",
            Logradouro = "Rua das Flores",
            Complemento = "Apto 10",
            Bairro = "Centro",
            Cidade = "São Paulo",
            UF = "SP",
            IdPessoa = pessoa.Id
        };

        pessoa.Endereco = endereco;

        endereco.CEP.Should().Be("12345678");
        endereco.Logradouro.Should().Be("Rua das Flores");
        endereco.Complemento.Should().Be("Apto 10");
        endereco.Bairro.Should().Be("Centro");
        endereco.Cidade.Should().Be("São Paulo");
        endereco.UF.Should().Be("SP");
        endereco.IdPessoa.Should().Be(pessoa.Id);
        pessoa.Endereco.Should().BeSameAs(endereco);
    }

    [Fact]
    public void CEP_Invalido_DeveLancarArgumentException()
    {
        var endereco = new Endereco();

        Action act = () => endereco.CEP = "123";

        act.Should().Throw<ArgumentException>()
            .WithMessage("*CEP deve possuir exatamente 8 caracteres*");
    }

    [Fact]
    public void Logradouro_Invalido_DeveLancarArgumentException()
    {
        var endereco = new Endereco();

        Action act = () => endereco.Logradouro = "Rua";

        act.Should().Throw<ArgumentException>()
            .WithMessage("*logradouro deve ter no mínimo 5 caracteres*");
    }

    [Fact]
    public void UF_Invalida_DeveLancarArgumentException()
    {
        var endereco = new Endereco();

        Action act = () => endereco.UF = "S";

        act.Should().Throw<ArgumentException>()
            .WithMessage("*UF deve possuir exatamente 2 caracteres*");
    }

    [Fact]
    public void Relacionamento_ComPessoa_DeveVincularEndereco()
    {
        var pessoa = new Pessoa("Ana Maria", DataValida);
        var endereco = new Endereco
        {
            CEP = "87654321",
            Logradouro = "Avenida Brasil",
            Bairro = "Bela Vista",
            Cidade = "Rio",
            UF = "RJ",
            IdPessoa = pessoa.Id
        };

        pessoa.Endereco = endereco;

        pessoa.Endereco.Should().NotBeNull();
        pessoa.Endereco!.IdPessoa.Should().Be(pessoa.Id);
        pessoa.Endereco.Should().BeSameAs(endereco);
    }
}
