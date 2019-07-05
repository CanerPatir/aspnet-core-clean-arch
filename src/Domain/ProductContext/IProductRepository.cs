using System;
using Domain.Abstraction;

namespace Domain.ProductContext
{
    public interface IProductRepository : IRepository<Product, Guid>
    {
    }
}