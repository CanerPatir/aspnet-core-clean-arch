using System;
using System.Threading.Tasks;
using Domain.ProductContext;

namespace Application.UseCases.CreateProduct
{
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task HandleAsync(CreateProductCommand command)
        {
            var newProduct = Product.Create(Guid.NewGuid(), command.CategoryId, command.BrandId, command.ProductCode);
            
            await _productRepository.Save(newProduct);
        }
    }
}