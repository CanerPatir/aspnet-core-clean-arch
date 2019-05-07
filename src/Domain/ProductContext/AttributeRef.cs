namespace Domain.ProductContext
{
    public class AttributeRef : ValueObject<AttributeRef>
    {
        public AttributeRef(int attributeId, int attributeValueId)
        {
            AttributeId = attributeId;
            AttributeValueId = attributeValueId;
        }

        /// <summary>
        /// Represents attribute for example: color, size, material etc.
        /// </summary>
        public int AttributeId { get; }

        /// <summary>
        /// Represents value of attribute for example: Red, Small, Cotton etc. 
        /// </summary>
        public int AttributeValueId { get; }

    }
}