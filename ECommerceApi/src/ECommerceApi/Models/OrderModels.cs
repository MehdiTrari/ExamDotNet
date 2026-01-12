using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ECommerceApi.Models
{
    public class OrderRequest
    {
        public List<OrderProductRequest> Products { get; set; } = new List<OrderProductRequest>();

        [JsonPropertyName("promo_code")]
        public string PromoCode { get; set; }
    }

    public class OrderProductRequest
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderResponse
    {
        public List<OrderProductLine> Products { get; set; } = new List<OrderProductLine>();
        public List<Discount> Discounts { get; set; } = new List<Discount>();
        public decimal Total { get; set; }
    }

    public class OrderProductLine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }

        [JsonPropertyName("price_per_unit")]
        public decimal PricePerUnit { get; set; }

        public decimal Total { get; set; }
    }

    public class Discount
    {
        public string Type { get; set; }
        public decimal Value { get; set; }
    }

    public class ErrorResponse
    {
        public List<string> Errors { get; set; } = new List<string>();
    }
}
