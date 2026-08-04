using FluentValidation;
using PriceOrchestrator.Api.DTOs;

namespace PriceOrchestrator.Api.Validators
{
    public class CreatePriceChangeDtoValidator : AbstractValidator<CreatePriceChangeDto>
    {
        public CreatePriceChangeDtoValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.OldPrice).GreaterThanOrEqualTo(0);
            RuleFor(x => x.NewPrice).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Currency).NotEmpty().Length(3);
            RuleFor(x => x.EffectiveFromUtc).NotEmpty();
            RuleFor(x => x.RequestedBy).NotEmpty().MaximumLength(100);
        }
    }
}
