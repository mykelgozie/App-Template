using App.Core.Interfaces;
using App.Data;
using App.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace App.Core.Repository
{
    public class WalletRepository : IWalletRepository
    {
        private readonly AppDbContext _appDbContext;

        public WalletRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public async Task<Wallet> GetWalletByUserId(string userId)
        {
            return await _appDbContext.Wallets.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<Wallet> UpdateWallet(Wallet wallet)
        {
            await _appDbContext.Wallets.AddAsync(wallet);
            var result = await _appDbContext.SaveChangesAsync();
            return result > 0 ? wallet : null;
        }
    }
}
