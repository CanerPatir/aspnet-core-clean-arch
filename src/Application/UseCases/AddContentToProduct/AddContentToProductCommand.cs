using System;

namespace Application.UseCases.AddContentToProduct
{
    public class AddContentToProductCommand
    {
        public AddContentToProductCommand(Guid productId, string title, string description,
            int attributeId, int attributeValueId)
        {
            ProductId = productId;
            Title = title;
            Description = description;
            AttributeId = attributeId;
            AttributeValueId = attributeValueId;
        }

        public Guid ProductId { get; }
        public string Title { get; }
        public string Description { get; }
        public int AttributeId { get; }
        public int AttributeValueId { get; }
    }
}