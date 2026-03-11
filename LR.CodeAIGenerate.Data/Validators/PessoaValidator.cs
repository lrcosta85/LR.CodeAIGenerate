using FluentValidation;
using LR.CodeAIGenerate.Domain.Modelos;

namespace LR.CodeAIGenerate.Data.Validators;

/// <summary>
/// Validador FluentValidation para a entidade Pessoa.
/// </summary>
public class PessoaValidator : AbstractValidator<Pessoa>
{
    public PessoaValidator()
    {
        RuleFor(p => p.Nome)
            .NotEmpty()
            .WithMessage("O nome não pode ser nulo ou vazio.")
            .MinimumLength(5)
            .WithMessage("O nome deve ter no mínimo 5 caracteres.")
            .Must(NomeDeveTerSobrenome)
            .WithMessage("O nome deve conter ao menos nome e sobrenome.");

        RuleFor(p => p.DataNascimento)
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage("A data de nascimento não pode ser maior que a data de hoje.");
    }

    private static bool NomeDeveTerSobrenome(string? nome)
    {
        if (string.IsNullOrWhiteSpace(nome)) return false;
        var partes = nome.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return partes.Length >= 2;
    }
}
