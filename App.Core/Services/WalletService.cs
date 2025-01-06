using App.Core.Interfaces;
using App.Dtos.ResponseModel;
using App.Dtos.ResquestModel;
using App.Models;
using System;
using System.Threading.Tasks;

namespace App.Core.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly INotificationService _notificationService;

        public WalletService(IWalletRepository walletRepository, INotificationService notificationService)
        {
            _walletRepository = walletRepository;
            _notificationService = notificationService;
        }


        public async Task<ModelResponse<Wallet>> DebitWallet(string userId, decimal amount)
        {
            try
            {
                if (amount <= 0)
                {
                    return new ModelResponse<Wallet>() { Message = "Invalid wmount" };
                }

                var wallet = await _walletRepository.GetWalletByUserId(userId);
                if (wallet == null)
                {
                    await _notificationService.SendEmail(new EmailDto { Message = "Wallet balance is low" });
                    return new ModelResponse<Wallet>() { Message = "Wallet not found" };
                }

                if (amount > wallet.Balance)
                {

                    return new ModelResponse<Wallet> { Message = "Invalid amount" };
                }

                wallet.Balance -= amount;

                var walletResponse = await _walletRepository.UpdateWallet(wallet);
                return new ModelResponse<Wallet>() { Data = wallet, Message = "Wallet debited", Status = true };

            }
            catch (Exception e)
            {

                return new ModelResponse<Wallet> { Message = e.Message };
            }

        }


        public async Task<ModelResponse<decimal>> CreditWallet(string userId, decimal amount)
        {
            try
            {
                if (amount <= 0)
                {
                    return new ModelResponse<decimal>() { Message = "Invalid wmount" };
                }

                var wallet = await _walletRepository.GetWalletByUserId(userId);
                if (wallet == null)
                {
                    return new ModelResponse<decimal>() { Message = "Wallet not found" };
                }


                wallet.Balance += amount;

                var walletResponse = await _walletRepository.UpdateWallet(wallet);
                return new ModelResponse<decimal>() { Data = amount, Message = "Wallet credied", Status = true };
            }
            catch (Exception e)
            {

                return new ModelResponse<decimal> { Message = e.Message };
            }

        }

    }
}
