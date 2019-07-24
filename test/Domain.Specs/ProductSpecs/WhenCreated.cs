using System;
using Domain.ProductContext;
using Domain.ProductContext.Events;
using TestBase;
using Xunit;

namespace Domain.Specs.ProductSpecs
{
    public class WhenCreated : SpecBase
    {
        [Fact]
        public void Aggregate_Should_Be_Created_And_Publish_ProductCreated_Event()
        {
            var productId = Random<Guid>();
            var brandId = Random<int>();
            var productCode = Random<string>();
            var categoryId = Random<int>();

            var productCreated = new ProductCreated(
                productId,
                productCode,
                brandId, categoryId);

            new ScenarioFor<Product>(
                    () => Product.Create(productId, categoryId, brandId, productCode)
                )
                .When(product => { })
                .Then(productCreated);
        }
    }
}