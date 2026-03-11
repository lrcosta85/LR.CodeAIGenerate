using FluentAssertions;
using LR.CodeAIGenerate.Domain.Modelos;

namespace LR.CodeAIGenerate.Domain.Tests.Modelos;

public class PessoaTests
{
    private static readonly DateTime DataValida = new(1990, 5, 15);

    #region Construtor padrão

    [Fact]
    public void ConstrutorPadrao_DeveGerarIdNaoVazio()
    {
        var pessoa = new Pessoa();

        pessoa.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void ConstrutorPadrao_DevePermitirDefinirNomeEDataDepois()
    {
        var pessoa = new Pessoa
        {
            Nome = "Maria Silva",
            DataNascimento = DataValida
        };

        pessoa.Nome.Should().Be("Maria Silva");
        pessoa.DataNascimento.Should().Be(DataValida);
    }

    #endregion

    #region Construtor com parâmetros

    [Fact]
    public void ConstrutorComParametros_ComDadosValidos_DeveCriarPessoa()
    {
        var nome = "João da Silva";
        var dataNascimento = new DateTime(1985, 10, 20);

        var pessoa = new Pessoa(nome, dataNascimento);

        pessoa.Id.Should().NotBe(Guid.Empty);
        pessoa.Nome.Should().Be(nome);
        pessoa.DataNascimento.Should().Be(dataNascimento);
    }

    [Fact]
    public void ConstrutorComParametros_ComIdInformado_DeveUsarIdInformado()
    {
        var id = Guid.NewGuid();
        var pessoa = new Pessoa("Ana Costa", DataValida, id);

        pessoa.Id.Should().Be(id);
    }

    [Fact]
    public void ConstrutorComParametros_SemId_DeveGerarNovoId()
    {
        var pessoa = new Pessoa("Pedro Santos", DataValida, id: null);

        pessoa.Id.Should().NotBe(Guid.Empty);
    }

    #endregion

    #region Validação de Nome - nulo, vazio, apenas espaços

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Nome_NuloOuVazioOuApenasEspacos_DeveLancarArgumentException(string? nome)
    {
        var pessoa = new Pessoa();

        var act = () => pessoa.Nome = nome!;

        act.Should().Throw<ArgumentException>()
            .WithMessage("*O nome não pode ser nulo ou vazio*")
            .WithParameterName("Nome");
    }

    [Fact]
    public void ConstrutorComParametros_NomeNulo_DeveLancarArgumentException()
    {
        var act = () => new Pessoa(null!, DataValida);

        act.Should().Throw<ArgumentException>()
            .WithParameterName("Nome");
    }

    #endregion

    #region Validação de Nome - mínimo 5 caracteres

    [Theory]
    [InlineData("João")]
    [InlineData("Ana")]
    [InlineData("Lu")]
    public void Nome_ComMenosDeCincoCaracteres_DeveLancarArgumentException(string nome)
    {
        var pessoa = new Pessoa();

        var act = () => pessoa.Nome = nome;

        act.Should().Throw<ArgumentException>()
            .WithMessage("*O nome deve ter no mínimo 5 caracteres*")
            .WithParameterName("Nome");
    }

    [Fact]
    public void Nome_ComCincoCaracteresMasUmaPalavraSo_DeveLancarArgumentException()
    {
        var pessoa = new Pessoa();

        var act = () => pessoa.Nome = "Maria";

        act.Should().Throw<ArgumentException>()
            .WithMessage("*O nome deve conter ao menos nome e sobrenome*")
            .WithParameterName("Nome");
    }

    #endregion

    #region Validação de Nome - nome e sobrenome

    [Theory]
    [InlineData("Maria")]   // 5 caracteres, uma palavra
    [InlineData("Pedro")]   // 5 caracteres, uma palavra
    public void Nome_ApenasUmaPalavraComCincoOuMaisCaracteres_DeveLancarArgumentException(string nome)
    {
        var pessoa = new Pessoa();

        var act = () => pessoa.Nome = nome;

        act.Should().Throw<ArgumentException>()
            .WithMessage("*O nome deve conter ao menos nome e sobrenome*")
            .WithParameterName("Nome");
    }

    #endregion

    #region Nome válido

    [Theory]
    [InlineData("Maria Silva")]
    [InlineData("João da Silva")]
    [InlineData("Ana Paula Costa")]
    [InlineData("José  Maria")] // múltiplos espaços são tratados (Split RemoveEmptyEntries)
    public void Nome_ValidoComNomeESobrenome_DeveAceitar(string nome)
    {
        var pessoa = new Pessoa();

        pessoa.Nome = nome;

        pessoa.Nome.Should().Be(nome.Trim());
    }

    [Fact]
    public void Nome_ComEspacosExtras_DeveSerTrimado()
    {
        var pessoa = new Pessoa();

        pessoa.Nome = "  Maria Silva  ";

        pessoa.Nome.Should().Be("Maria Silva");
    }

    #endregion

    #region Validação de DataNascimento

    [Fact]
    public void DataNascimento_DataFutura_DeveLancarArgumentOutOfRangeException()
    {
        var pessoa = new Pessoa();
        var dataFutura = DateTime.Today.AddDays(1);

        var act = () => pessoa.DataNascimento = dataFutura;

        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("DataNascimento")
            .WithMessage("*data de nascimento não pode ser maior que a data de hoje*");
    }

    [Fact]
    public void DataNascimento_DataDeHoje_DeveAceitar()
    {
        var pessoa = new Pessoa { Nome = "Maria Silva" };
        var hoje = DateTime.Today;

        pessoa.DataNascimento = hoje;

        pessoa.DataNascimento.Should().Be(hoje);
    }

    [Fact]
    public void DataNascimento_DataNoPassado_DeveAceitar()
    {
        var pessoa = new Pessoa { Nome = "Maria Silva" };
        var dataPassado = DateTime.Today.AddYears(-30);

        pessoa.DataNascimento = dataPassado;

        pessoa.DataNascimento.Should().Be(dataPassado);
    }

    [Fact]
    public void ConstrutorComParametros_DataNascimentoFutura_DeveLancarArgumentOutOfRangeException()
    {
        var dataFutura = DateTime.Today.AddDays(1);

        var act = () => new Pessoa("Maria Silva", dataFutura);

        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("DataNascimento");
    }

    #endregion

    #region Cenários combinados

    [Fact]
    public void Pessoa_ComTodosDadosValidos_DeveManterValores()
    {
        var id = Guid.NewGuid();
        var nome = "Carlos Eduardo Souza";
        var dataNascimento = new DateTime(1980, 1, 10);

        var pessoa = new Pessoa(nome, dataNascimento, id);

        pessoa.Id.Should().Be(id);
        pessoa.Nome.Should().Be(nome);
        pessoa.DataNascimento.Should().Be(dataNascimento);
    }

    [Fact]
    public void Id_DeveSerAlteravel()
    {
        var pessoa = new Pessoa { Nome = "Maria Silva", DataNascimento = DataValida };
        var novoId = Guid.NewGuid();

        pessoa.Id = novoId;

        pessoa.Id.Should().Be(novoId);
    }

    #endregion
}
