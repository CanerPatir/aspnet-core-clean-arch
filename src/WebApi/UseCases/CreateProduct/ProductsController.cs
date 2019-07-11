using System.Threading;
using System.Threading.Tasks;
using Application.UseCases.CreateProduct;
using Infrastructure.Messaging.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.UseCases.CreateProduct
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IBus _bus;

        public ProductsController(IBus bus) => _bus = bus;

        // POST api/products
        [HttpPost]
        public async Task Post([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
        {
            await _bus.SendAsync(command, cancellationToken);
        }
    }
}