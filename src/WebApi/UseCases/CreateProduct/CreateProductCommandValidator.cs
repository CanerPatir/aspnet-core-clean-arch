using Application.UseCases.CreateProduct;
using FluentValidation;

namespace WebApi.UseCases.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.BrandId).NotNull();
            RuleFor(x => x.CategoryId).NotNull();
            RuleFor(x => x.ProductCode).NotNull();
        }
    }
}