using System.Collections.Generic;
using System.Linq;
using ECommerceApi.Models;

namespace ECommerceApi.Services
{
    public class ProductService : IProductService
    {
        private readonly List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Produit A", Price = 10, Stock = 20 },
            new Product { Id = 2, Name = "Produit B", Price = 15, Stock = 5 },
            new Product { Id = 3, Name = "Produit C", Price = 20, Stock = 10 },
            new Product { Id = 4, Name = "Produit D", Price = 25, Stock = 0 },
            new Product { Id = 5, Name = "Produit E", Price = 30, Stock = 15 }
        };

        public IEnumerable<Product> GetAllProducts()
        {
            return _products;
        }

        public Product GetById(int id)
        {
            return _products.FirstOrDefault(product => product.Id == id);
        }

        public void SaveChanges()
        {
        }
    }
}
