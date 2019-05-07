using System;
using Domain.Abstraction;

namespace Domain.ProductContext.Repository
{
    public interface IProductRepository : IRepository<Product, Guid>
    {
    }
}