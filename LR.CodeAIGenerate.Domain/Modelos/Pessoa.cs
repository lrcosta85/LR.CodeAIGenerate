namespace LR.CodeAIGenerate.Domain.Modelos;

/// <summary>
/// Representa uma pessoa no domínio.
/// </summary>
public class Pessoa
{
    private string _nome = string.Empty;
    private DateTime _dataNascimento;
    private string _cpf = string.Empty;
    private DateTime _dataCadastro;

    /// <summary>
    /// Identificador único da pessoa.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nome completo da pessoa. Deve ter no mínimo 5 caracteres e conter ao menos nome e sobrenome.
    /// </summary>
    public string Nome
    {
        get => _nome;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("O nome não pode ser nulo ou vazio.", nameof(Nome));

            if (value.Trim().Length < 5)
                throw new ArgumentException("O nome deve ter no mínimo 5 caracteres.", nameof(Nome));

            var partes = value.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (partes.Length < 2)
                throw new ArgumentException("O nome deve conter ao menos nome e sobrenome.", nameof(Nome));

            _nome = value.Trim();
        }
    }

    /// <summary>
    /// Data de nascimento da pessoa. Não pode ser maior que a data atual.
    /// </summary>
    public DateTime DataNascimento
    {
        get => _dataNascimento;
        set
        {
            var hoje = DateTime.Today;
            if (value.Date > hoje)
                throw new ArgumentOutOfRangeException(nameof(DataNascimento),
                    "A data de nascimento não pode ser maior que a data de hoje.");

            _dataNascimento = value;
        }
    }

    /// <summary>
    /// CPF da pessoa. Deve possuir exatamente 11 caracteres.
    /// </summary>
    public string CPF
    {
        get => _cpf;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("O CPF não pode ser nulo ou vazio.", nameof(CPF));

            var cpf = value.Trim();
            if (cpf.Length != 11)
                throw new ArgumentException("O CPF deve possuir exatamente 11 caracteres.", nameof(CPF));

            _cpf = cpf;
        }
    }

    /// <summary>
    /// Data de cadastro da pessoa. Preenchida automaticamente na criação e imutável.
    /// </summary>
    public DateTime DataCadastro => _dataCadastro;

    /// <summary>
    /// Endereço associado (0 ou 1).
    /// </summary>
    public Endereco? Endereco { get; set; }

    /// <summary>
    /// Cria uma nova instância de Pessoa.
    /// </summary>
    public Pessoa()
    {
        Id = Guid.NewGuid();
        _dataCadastro = DateTime.Now;
    }

    /// <summary>
    /// Cria uma nova instância de Pessoa com os dados informados.
    /// </summary>
    /// <param name="nome">Nome completo (mínimo 5 caracteres, nome e sobrenome).</param>
    /// <param name="dataNascimento">Data de nascimento (não pode ser futura).</param>
    /// <param name="id">Identificador. Se não informado, será gerado um novo Guid.</param>
    public Pessoa(string nome, DateTime dataNascimento, Guid? id = null, string? cpf = null)
    {
        Id = id ?? Guid.NewGuid();
        Nome = nome;
        DataNascimento = dataNascimento;
        _dataCadastro = DateTime.Now;

        if (!string.IsNullOrWhiteSpace(cpf))
            CPF = cpf!;
    }
}
