using Application.UseCases.AddContentToProduct;
using FluentValidation;

namespace WebApi.UseCases.AddContentToProduct
{
    public class AddContentToProductCommandValidator: AbstractValidator<AddContentToProductCommand> {
        public AddContentToProductCommandValidator() {
            RuleFor(x => x.Description).NotNull();
            RuleFor(x => x.Title).NotNull();
            RuleFor(x => x.AttributeId).NotNull();
            RuleFor(x => x.ProductId).NotNull(); 
            RuleFor(x => x.AttributeValueId).NotNull(); 
        }
    }
}