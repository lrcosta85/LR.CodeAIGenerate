using System;

namespace LR.CodeAIGenerate.Domain.Modelos;

/// <summary>
/// Representa um endereço associado a uma Pessoa.
/// </summary>
public class Endereco
{
    private string _cep = string.Empty;
    private string _logradouro = string.Empty;
    private string? _complemento;
    private string _bairro = string.Empty;
    private string _cidade = string.Empty;
    private string _uf = string.Empty;
    private Guid _idPessoa;

    public Guid Id { get; set; }

    public string CEP
    {
        get => _cep;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("O CEP não pode ser nulo ou vazio.", nameof(CEP));

            var cep = value.Trim();
            if (cep.Length != 8)
                throw new ArgumentException("O CEP deve possuir exatamente 8 caracteres.", nameof(CEP));

            _cep = cep;
        }
    }

    public string Logradouro
    {
        get => _logradouro;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("O logradouro não pode ser nulo ou vazio.", nameof(Logradouro));

            var txt = value.Trim();
            if (txt.Length < 5)
                throw new ArgumentException("O logradouro deve ter no mínimo 5 caracteres.", nameof(Logradouro));

            _logradouro = txt;
        }
    }

    public string? Complemento
    {
        get => _complemento;
        set => _complemento = string.IsNullOrWhiteSpace(value) ? null : value?.Trim();
    }

    public string Bairro
    {
        get => _bairro;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("O bairro não pode ser nulo ou vazio.", nameof(Bairro));

            var txt = value.Trim();
            if (txt.Length < 4)
                throw new ArgumentException("O bairro deve ter no mínimo 4 caracteres.", nameof(Bairro));

            _bairro = txt;
        }
    }

    public string Cidade
    {
        get => _cidade;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("A cidade não pode ser nula ou vazia.", nameof(Cidade));

            var txt = value.Trim();
            if (txt.Length < 3)
                throw new ArgumentException("A cidade deve ter no mínimo 3 caracteres.", nameof(Cidade));

            _cidade = txt;
        }
    }

    public string UF
    {
        get => _uf;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("A UF não pode ser nula ou vazia.", nameof(UF));

            var txt = value.Trim();
            if (txt.Length != 2)
                throw new ArgumentException("A UF deve possuir exatamente 2 caracteres.", nameof(UF));

            _uf = txt.ToUpperInvariant();
        }
    }

    public Guid IdPessoa
    {
        get => _idPessoa;
        set
        {
            if (value == Guid.Empty)
                throw new ArgumentException("IdPessoa inválido.", nameof(IdPessoa));

            _idPessoa = value;
        }
    }

    /// <summary>
    /// Navegação para a pessoa proprietária.
    /// </summary>
    public Pessoa? Pessoa { get; set; }

    public Endereco()
    {
        Id = Guid.NewGuid();
    }
}
