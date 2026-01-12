using System;
using System.Collections.Generic;
using System.Linq;
using ECommerceApi.Models;

namespace ECommerceApi.Services
{
    public class OrderService
    {
        private readonly IProductService _productService;
        private readonly IPromoCodeService _promoCodeService;

        public OrderService(IProductService productService, IPromoCodeService promoCodeService)
        {
            _productService = productService;
            _promoCodeService = promoCodeService;
        }

        public OrderResult ProcessOrder(OrderRequest request)
        {
            var result = new OrderResult();
            if (request == null)
            {
                result.Errors.Add("La requete est invalide");
                return result;
            }

            var requestedProducts = request.Products ?? new List<OrderProductRequest>();
            var lines = new List<OrderLine>();

            foreach (var item in requestedProducts)
            {
                var product = _productService.GetById(item.Id);
                if (product == null)
                {
                    result.Errors.Add($"Le produit avec l'identifiant {item.Id} n'existe pas");
                    continue;
                }

                if (item.Quantity > product.Stock)
                {
                    result.Errors.Add($"il ne reste que {product.Stock} exemplaire pour le produit {product.Name}");
                }

                var lineBaseTotal = product.Price * item.Quantity;
                var lineTotal = item.Quantity > 5 ? lineBaseTotal * 0.90m : lineBaseTotal;

                lines.Add(new OrderLine
                {
                    Product = product,
                    Quantity = item.Quantity,
                    BaseTotal = lineBaseTotal,
                    Total = lineTotal
                });
            }

            var baseTotal = lines.Sum(line => line.BaseTotal);
            var subtotal = lines.Sum(line => line.Total);

            var promoCode = request.PromoCode?.Trim();
            var promoRate = 0m;
            if (!string.IsNullOrEmpty(promoCode))
            {
                if (!_promoCodeService.TryGetRate(promoCode, out promoRate))
                {
                    result.Errors.Add("le code promo est invalide");
                }

                if (baseTotal < 50m)
                {
                    result.Errors.Add("Les codes promos ne sont valables qu'à partir de 50€ d'achat");
                }
            }

            if (result.Errors.Count > 0)
            {
                return result;
            }

            var orderDiscount = subtotal > 100m ? subtotal * 0.05m : 0m;
            var promoDiscount = promoRate > 0m ? subtotal * promoRate : 0m;

            var response = new OrderResponse();

            foreach (var line in lines)
            {
                response.Products.Add(new OrderProductLine
                {
                    Id = line.Product.Id,
                    Name = line.Product.Name,
                    Quantity = line.Quantity,
                    PricePerUnit = RoundMoney(line.Product.Price),
                    Total = RoundMoney(line.Total)
                });
            }

            if (orderDiscount > 0m)
            {
                response.Discounts.Add(new Discount { Type = "order", Value = RoundMoney(orderDiscount) });
            }

            if (promoDiscount > 0m)
            {
                response.Discounts.Add(new Discount { Type = "promo", Value = RoundMoney(promoDiscount) });
            }

            response.Total = RoundMoney(subtotal - orderDiscount - promoDiscount);

            foreach (var line in lines)
            {
                line.Product.Stock -= line.Quantity;
            }

            _productService.SaveChanges();

            result.Response = response;
            return result;
        }

        private static decimal RoundMoney(decimal value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        private sealed class OrderLine
        {
            public Product Product { get; set; }
            public int Quantity { get; set; }
            public decimal BaseTotal { get; set; }
            public decimal Total { get; set; }
        }
    }

    public class OrderResult
    {
        public OrderResponse Response { get; set; }
        public List<string> Errors { get; } = new List<string>();
        public bool IsSuccess => Errors.Count == 0;
    }
}
