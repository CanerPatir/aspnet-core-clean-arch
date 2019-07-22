using System.Threading;
using System.Threading.Tasks;
using Domain.ProductContext;

namespace Application.UseCases.AddContentToProduct
{
    public class AddContentToProductCommandHandler : ICommandHandler<AddContentToProductCommand>
    {
        private readonly IProductRepository _productRepository;

        public AddContentToProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Handle(AddContentToProductCommand command, CancellationToken cancellationToken)
        {
            var aggregate = await _productRepository.Load(command.ProductId, cancellationToken);

            aggregate.AddContent(command.Title, command.Description,
                new AttributeRef(command.AttributeId, command.AttributeValueId));

            await _productRepository.Save(aggregate, cancellationToken);
        }
    }
}