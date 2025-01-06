using App.Models;
using System.Threading.Tasks;

namespace App.Core.Interfaces
{
    public interface IWalletRepository
    {
        Task<Wallet> GetWalletByUserId(string userId);
        Task<Wallet> UpdateWallet(Wallet wallet);
    }
}
