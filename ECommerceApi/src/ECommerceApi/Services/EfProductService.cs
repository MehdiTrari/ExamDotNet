using System.Collections.Generic;
using System.Linq;
using ECommerceApi.Data;
using ECommerceApi.Models;

namespace ECommerceApi.Services
{
    public class EfProductService : IProductService
    {
        private readonly AppDbContext _db;

        public EfProductService(AppDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _db.Products.ToList();
        }

        public Product GetById(int id)
        {
            return _db.Products.FirstOrDefault(product => product.Id == id);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}
