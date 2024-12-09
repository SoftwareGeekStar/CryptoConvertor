namespace CryptoConvertor.Services.Interfaces
{
    public interface ICryptoService
    {
        Task<Dictionary<string, decimal>> GetCryptoQuotesAsync(string cryptoSymbol);
    }
}
