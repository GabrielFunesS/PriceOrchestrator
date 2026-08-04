using FluentValidation;
using PriceOrchestrator.Api.DTOs;

namespace PriceOrchestrator.Api.Validators
{
    public class ProductStockDtoValidator : AbstractValidator<ProductStockDto>
    {
        public ProductStockDtoValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Quantity).GreaterThan(0);
            RuleFor(x => x.Warehouse).NotEmpty().MaximumLength(200);
        }
    }
}
