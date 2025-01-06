using App.Core.Interfaces;
using App.Dtos.ResquestModel;
using App.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace App.Core.Services
{
    public class ProducerService : IProducerService
    {
        private IElectricityBillRepository _electricityBillRepository;
        private readonly IWalletService _walletService;
        private readonly IWalletRepository _walletRepository;
        private readonly ILogger<ProducerService> _logger;
        private readonly INotificationService _notificationService;

        public ProducerService(IElectricityBillRepository electricityBillRepository, IWalletService walletService, IWalletRepository walletRepository, ILogger<ProducerService> logger, INotificationService notificationService)
        {
            _electricityBillRepository = electricityBillRepository;
            _walletService = walletService;
            _walletRepository = walletRepository;
            _logger = logger;
            _notificationService = notificationService;
        }

        public async Task PublishBillAsync(ElectricityBill electricityBill)
        {
            var message = JsonSerializer.Serialize(electricityBill);

            // Replace with SNS topic ARN or mock URL
            await _electricityBillRepository.CreateElectricityBill(electricityBill);
            var email = new EmailDto { FromEmail = "sendermail", ToMail = "receivermail", Message = "New Bill Has been created " };
            await PublishEmailAsync(email);
        }


        public async Task PublishVendBillAsync(ElectricityBill electricityBill)
        {
            try
            {
                var message = JsonSerializer.Serialize(electricityBill);

                // Replace with SNS topic ARN or mock URL
                await MockPublishVendBill(message);
            }
            catch (Exception e)
            {

                _logger.LogError($"{nameof(PublishVendBillAsync)}  Error processing {JsonSerializer.Serialize(electricityBill)}", e);
            }

        }




        public async Task MockPublishVendBill(string verificationRef)
        {
            var pendingBill = await _electricityBillRepository.GetElectricityBillByVerificationRef(verificationRef);

            var wallet = await _walletRepository.GetWalletByUserId(pendingBill.UserId);
            if (wallet == null)
                return;


            var walletResponse = await _walletService.DebitWallet(pendingBill.UserId, pendingBill.Amount);

            if (walletResponse.Status)
            {
                await _notificationService.SendEmail(new EmailDto { Message = walletResponse.Message });
                return;
            }
          


            pendingBill.Status = PaymentStatus.Paid;
            pendingBill.ModifiedAt = DateTime.UtcNow;
            await _electricityBillRepository.UpdateElectricityBill(pendingBill);
            var email = new EmailDto { FromEmail = "sendermail", ToMail = "receivermail", Message = "Payment sucessful here is your token DNJFHDk" };
            await PublishEmailAsync(email);
        }



        public async Task PublishWalletAsync(WalletDto walletDto)
        {
            try
            {
                var message = JsonSerializer.Serialize(walletDto);

                // Replace with SNS topic ARN or mock URL
                await _walletService.CreditWallet(walletDto.userId, walletDto.Amount);

            }
            catch (Exception e)
            {

                _logger.LogError($"{nameof(PublishWalletAsync)}  Error processing {JsonSerializer.Serialize(walletDto)}", e);
            }

        }


        public async Task PublishEmailAsync(EmailDto emailDto)
        {
            try
            {
                var message = JsonSerializer.Serialize(emailDto);

                // Replace with SNS topic ARN or mock URL
                await _notificationService.SendEmail(emailDto);


            }
            catch (Exception e)
            {

                _logger.LogError($"{nameof(PublishEmailAsync)}  Error processing ", e);
            }

        }


    }
}
