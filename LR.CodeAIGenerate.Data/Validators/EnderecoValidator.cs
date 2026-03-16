using FluentValidation;
using LR.CodeAIGenerate.Domain.Modelos;

namespace LR.CodeAIGenerate.Data.Validators;

public class EnderecoValidator : AbstractValidator<Endereco>
{
    public EnderecoValidator()
    {
        RuleFor(e => e.CEP)
            .NotEmpty()
            .WithMessage("O CEP não pode ser nulo ou vazio.")
            .Length(8)
            .WithMessage("O CEP deve possuir exatamente 8 caracteres.");

        RuleFor(e => e.Logradouro)
            .NotEmpty()
            .MinimumLength(5)
            .WithMessage("O logradouro deve ter no mínimo 5 caracteres.");

        RuleFor(e => e.Bairro)
            .NotEmpty()
            .MinimumLength(4)
            .WithMessage("O bairro deve ter no mínimo 4 caracteres.");

        RuleFor(e => e.Cidade)
            .NotEmpty()
            .MinimumLength(3)
            .WithMessage("A cidade deve ter no mínimo 3 caracteres.");

        RuleFor(e => e.UF)
            .NotEmpty()
            .Length(2)
            .WithMessage("A UF deve possuir exatamente 2 caracteres.");

        RuleFor(e => e.IdPessoa)
            .NotEqual(Guid.Empty)
            .WithMessage("IdPessoa inválido.");
    }
}
