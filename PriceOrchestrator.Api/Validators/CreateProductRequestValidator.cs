using FluentValidation;
using PriceOrchestrator.Api.DTOs;

namespace PriceOrchestrator.Api.Validators
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(x => x.ExternalId).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Description).MaximumLength(2000).When(x => x.Description != null);
            RuleFor(x => x.Brand).MaximumLength(200).When(x => x.Brand != null);
            RuleFor(x => x.Category).MaximumLength(200).When(x => x.Category != null);
        }
    }
}
