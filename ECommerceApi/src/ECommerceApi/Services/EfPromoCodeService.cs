using System;
using System.Linq;
using ECommerceApi.Data;

namespace ECommerceApi.Services
{
    public class EfPromoCodeService : IPromoCodeService
    {
        private readonly AppDbContext _db;

        public EfPromoCodeService(AppDbContext db)
        {
            _db = db;
        }

        public bool TryGetRate(string code, out decimal rate)
        {
            rate = 0m;
            if (string.IsNullOrWhiteSpace(code))
            {
                return false;
            }

            var promo = _db.PromoCodes.FirstOrDefault(p => p.Code == code.Trim());
            if (promo == null)
            {
                return false;
            }

            rate = promo.Rate;
            return true;
        }
    }
}
