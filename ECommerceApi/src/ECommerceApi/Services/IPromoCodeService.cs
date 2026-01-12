namespace ECommerceApi.Services
{
    public interface IPromoCodeService
    {
        bool TryGetRate(string code, out decimal rate);
    }
}
