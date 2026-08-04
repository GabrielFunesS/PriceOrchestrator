using FluentValidation;
using PriceOrchestrator.Api.DTOs;

namespace PriceOrchestrator.Api.Validators
{
    public class CreatePromotionDtoValidator : AbstractValidator<CreatePromotionDto>
    {
        public CreatePromotionDtoValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Value).GreaterThanOrEqualTo(0);
            RuleFor(x => x.StartsAtUtc).NotEmpty();
            RuleFor(x => x.EndsAtUtc).NotEmpty();
            RuleFor(x => x.EndsAtUtc).GreaterThan(x => x.StartsAtUtc).When(x => x.StartsAtUtc != default);
        }
    }
}
