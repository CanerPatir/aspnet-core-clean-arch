using System.Threading;
using System.Threading.Tasks;
using Application.UseCases.CreateProduct;
using Domain.ProductContext;
using Xunit;

namespace Application.Tests
{
    public class CreateProductCommandHandlerTests : CommandHandlerTestBase
    {
        private readonly CreateProductCommandHandler _sut;
        private readonly IProductRepository _repository;

        public CreateProductCommandHandlerTests()
        {
            _repository = FakeAndRegister<IProductRepository>();
            _sut = new CreateProductCommandHandler(_repository);
        }

        [Fact]
        public async Task Should_Persist_Created_Product()
        {
            // Given
            var expectedCategoryId = Random<int>();
            var expectedBrandId = Random<int>();
            var expectedProductCode = Random<string>();

            // When
            await _sut.Handle(new CreateProductCommand(expectedCategoryId, expectedBrandId, expectedProductCode),
                CancellationToken.None);

            // Then
            Verify(() => _repository.Save(Matches<Product>(c => c.Category.Id == expectedCategoryId
                                                                && c.Brand.Id == expectedBrandId
                                                                && c.Code == expectedProductCode),
                Ignore<CancellationToken>()));
        }
    }
}