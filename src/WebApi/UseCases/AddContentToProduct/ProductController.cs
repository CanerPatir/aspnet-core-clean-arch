using System.Threading;
using System.Threading.Tasks;
using Application.UseCases.AddContentToProduct;
using Infrastructure.Messaging.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.UseCases.AddContentToProduct
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator) => _mediator = mediator;

        // POST api/products
        [HttpPost]
        public async Task Post([FromBody] AddContentToProductCommand command, CancellationToken cancellationToken)
        {
            await _mediator.SendAsync(command, cancellationToken);
        }
    }
}