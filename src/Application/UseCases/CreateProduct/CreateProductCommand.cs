namespace Application.UseCases.CreateProduct
{
    public class CreateProductCommand
    {
        public CreateProductCommand(int categoryId, int brandId, string productCode)
        {
            CategoryId = categoryId;
            BrandId = brandId;
            ProductCode = productCode;
        }

        public int CategoryId { get; }
        public int BrandId { get; }
        public string ProductCode { get; }
        
    }
}