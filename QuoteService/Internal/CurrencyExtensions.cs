using System.Globalization;

namespace QuoteService.Internal
{
    public static class CurrencyExtensions
    {
        internal static readonly CultureInfo _culture = CultureInfo.GetCultureInfo("en-GB");
        public static string ToCurrency(this decimal amount)
        {
            return amount.ToString("C", _culture);
        }
    }
}
