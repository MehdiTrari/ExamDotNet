using System.Collections.Generic;
using System.Linq;
using ECommerceApi.Models;

namespace ECommerceApi.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
            if (!db.Products.Any())
            {
                db.Products.AddRange(new List<Product>
                {
                    new Product { Id = 1, Name = "Produit A", Price = 10, Stock = 20 },
                    new Product { Id = 2, Name = "Produit B", Price = 15, Stock = 5 },
                    new Product { Id = 3, Name = "Produit C", Price = 20, Stock = 10 },
                    new Product { Id = 4, Name = "Produit D", Price = 25, Stock = 0 },
                    new Product { Id = 5, Name = "Produit E", Price = 30, Stock = 15 }
                });
            }

            if (!db.PromoCodes.Any())
            {
                db.PromoCodes.AddRange(new List<PromoCode>
                {
                    new PromoCode { Code = "DISCOUNT20", Rate = 0.20m },
                    new PromoCode { Code = "DISCOUNT10", Rate = 0.10m }
                });
            }

            db.SaveChanges();
        }
    }
}
