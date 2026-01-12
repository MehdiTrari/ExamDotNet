using System.Collections.Generic;

namespace ECommerceApi.Services
{
    public class InMemoryPromoCodeService : IPromoCodeService
    {
        private static readonly Dictionary<string, decimal> PromoRates = new Dictionary<string, decimal>
        {
            { "DISCOUNT20", 0.20m },
            { "DISCOUNT10", 0.10m }
        };

        public bool TryGetRate(string code, out decimal rate)
        {
            rate = 0m;
            if (string.IsNullOrWhiteSpace(code))
            {
                return false;
            }

            return PromoRates.TryGetValue(code.Trim(), out rate);
        }
    }
}
