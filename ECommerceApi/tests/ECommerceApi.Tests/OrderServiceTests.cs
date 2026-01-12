using System.Collections.Generic;
using System.Linq;
using ECommerceApi.Models;
using ECommerceApi.Services;
using Xunit;

namespace ECommerceApi.Tests
{
    public class OrderServiceTests
    {
        [Fact]
        public void ProcessOrder_ValidOrder_UpdatesStockAndCalculatesDiscounts()
        {
            var productService = new ProductService();
            var promoService = new InMemoryPromoCodeService();
            var orderService = new OrderService(productService, promoService);
            var request = new OrderRequest
            {
                Products = new List<OrderProductRequest>
                {
                    new OrderProductRequest { Id = 1, Quantity = 3 },
                    new OrderProductRequest { Id = 3, Quantity = 7 }
                },
                PromoCode = "DISCOUNT20"
            };

            var result = orderService.ProcessOrder(request);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Response);

            var response = result.Response;
            Assert.Equal(2, response.Products.Count);

            var productA = response.Products.Single(product => product.Id == 1);
            Assert.Equal("Produit A", productA.Name);
            Assert.Equal(3, productA.Quantity);
            Assert.Equal(10m, productA.PricePerUnit);
            Assert.Equal(30m, productA.Total);

            var productC = response.Products.Single(product => product.Id == 3);
            Assert.Equal("Produit C", productC.Name);
            Assert.Equal(7, productC.Quantity);
            Assert.Equal(20m, productC.PricePerUnit);
            Assert.Equal(126m, productC.Total);

            Assert.Contains(response.Discounts, discount => discount.Type == "order" && discount.Value == 7.8m);
            Assert.Contains(response.Discounts, discount => discount.Type == "promo" && discount.Value == 31.2m);
            Assert.Equal(117m, response.Total);

            Assert.Equal(17, productService.GetById(1).Stock);
            Assert.Equal(3, productService.GetById(3).Stock);
        }

        [Fact]
        public void ProcessOrder_InsufficientStock_ReturnsErrorAndDoesNotUpdateStock()
        {
            var productService = new ProductService();
            var promoService = new InMemoryPromoCodeService();
            var orderService = new OrderService(productService, promoService);
            var request = new OrderRequest
            {
                Products = new List<OrderProductRequest>
                {
                    new OrderProductRequest { Id = 2, Quantity = 7 }
                }
            };

            var result = orderService.ProcessOrder(request);

            Assert.False(result.IsSuccess);
            Assert.Contains("il ne reste que 5 exemplaire pour le produit Produit B", result.Errors);
            Assert.Equal(5, productService.GetById(2).Stock);
        }

        [Fact]
        public void ProcessOrder_ProductDoesNotExist_ReturnsError()
        {
            var productService = new ProductService();
            var promoService = new InMemoryPromoCodeService();
            var orderService = new OrderService(productService, promoService);
            var request = new OrderRequest
            {
                Products = new List<OrderProductRequest>
                {
                    new OrderProductRequest { Id = 1111, Quantity = 1 }
                }
            };

            var result = orderService.ProcessOrder(request);

            Assert.False(result.IsSuccess);
            Assert.Contains("Le produit avec l'identifiant 1111 n'existe pas", result.Errors);
        }

        [Fact]
        public void ProcessOrder_InvalidPromoCode_ReturnsError()
        {
            var productService = new ProductService();
            var promoService = new InMemoryPromoCodeService();
            var orderService = new OrderService(productService, promoService);
            var request = new OrderRequest
            {
                Products = new List<OrderProductRequest>
                {
                    new OrderProductRequest { Id = 1, Quantity = 6 }
                },
                PromoCode = "BADCODE"
            };

            var result = orderService.ProcessOrder(request);

            Assert.False(result.IsSuccess);
            Assert.Contains("le code promo est invalide", result.Errors);
        }

        [Fact]
        public void ProcessOrder_PromoBelowThreshold_ReturnsError()
        {
            var productService = new ProductService();
            var promoService = new InMemoryPromoCodeService();
            var orderService = new OrderService(productService, promoService);
            var request = new OrderRequest
            {
                Products = new List<OrderProductRequest>
                {
                    new OrderProductRequest { Id = 1, Quantity = 4 }
                },
                PromoCode = "DISCOUNT10"
            };

            var result = orderService.ProcessOrder(request);

            Assert.False(result.IsSuccess);
            Assert.Contains("Les codes promos ne sont valables qu'à partir de 50€ d'achat", result.Errors);
        }

        [Fact]
        public void ProcessOrder_MultipleErrors_ReturnsAll()
        {
            var productService = new ProductService();
            var promoService = new InMemoryPromoCodeService();
            var orderService = new OrderService(productService, promoService);
            var request = new OrderRequest
            {
                Products = new List<OrderProductRequest>
                {
                    new OrderProductRequest { Id = 1111, Quantity = 1 },
                    new OrderProductRequest { Id = 2, Quantity = 7 }
                },
                PromoCode = "BADCODE"
            };

            var result = orderService.ProcessOrder(request);

            Assert.False(result.IsSuccess);
            Assert.Contains("Le produit avec l'identifiant 1111 n'existe pas", result.Errors);
            Assert.Contains("il ne reste que 5 exemplaire pour le produit Produit B", result.Errors);
            Assert.Contains("le code promo est invalide", result.Errors);
        }
    }
}
