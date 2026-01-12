using System.Collections.Generic;
using ECommerceApi.Models;

namespace ECommerceApi.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        Product GetById(int id);
        void SaveChanges();
    }
}
