using App.Dtos.ResponseModel;
using App.Models;
using System.Threading.Tasks;

namespace App.Core.Interfaces
{
    public interface IWalletService
    {
        Task<ModelResponse<decimal>> CreditWallet(string userId, decimal amount);
        Task<ModelResponse<Wallet>> DebitWallet(string userId, decimal amount);
    }
}
