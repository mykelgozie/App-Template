using App.Core.Interfaces;
using App.Core.Repository;
using App.Dtos.ResponseModel;
using App.Dtos.ResquestModel;
using App.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace App.Core.Services
{
    public class ElectricityBillService : IElectricityBillService
    {
        private readonly IElectricityBillRepository _electricityBillRepository;
        private readonly IProducerService _producerService;
        private readonly IWalletRepository _walletRepository;
        private readonly IWalletService _walletService;

        public ElectricityBillService(IElectricityBillRepository electricityBillRepository, IProducerService producerService, IWalletRepository walletRepository, IWalletService walletService)
        {
            _electricityBillRepository = electricityBillRepository;
            _producerService = producerService;
            _walletRepository = walletRepository;
            _walletService = walletService;
        }

        public async Task<ModelResponse<ElectricityBill>> GetPendingBillByVerificationRef(string verificationRef)
        {
            var bill = await _electricityBillRepository.GetElectricityBillByVerificationRef(verificationRef);
            if (bill == null)
            {
                return new ModelResponse<ElectricityBill> { Message = "no bill found" };
            }

            if (bill.Status != PaymentStatus.Pending)
            {
                return new ModelResponse<ElectricityBill> { Message = "Payment has been made" };
            }

            return new ModelResponse<ElectricityBill> { Data = bill, Message = "Bill successfully retrieved", Status = true };
        }

        public async Task<ModelResponse<ElectricityBill>> CreateBill(BillDto billDto)
        {
            try
            {
                var electricBill = new ElectricityBill()
                {
                    Amount = billDto.Amount,
                    Status = PaymentStatus.Pending,
                    VerificationRef = GenerateRandomChar(),
                    UserId = billDto.UserId,
                };

                await _producerService.PublishBillAsync(electricBill);
                return new ModelResponse<ElectricityBill> { Data = electricBill, Status = true, Message = "New bill created " };
            }
            catch (Exception e)
            {

                return new ModelResponse<ElectricityBill> { Status = false, Message = "Error cannot process bill" };
            }

        }

        public string GenerateRandomChar(int length = 10)
        {
            Random random = new Random();
            return new string(Enumerable.Range(0, length).Select(_ => "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"[random.Next(62)]).ToArray());

        }

        public async Task<ModelResponse<ElectricityBill>> VendBill(string verificationRef)
        {
            var billResponse = await GetPendingBillByVerificationRef(verificationRef);
            if (!billResponse.Status)
                return billResponse;


            var pendingBill = billResponse.Data;

            var wallet = await _walletRepository.GetWalletByUserId(pendingBill.UserId);
            if (wallet == null)
                return new ModelResponse<ElectricityBill> { Message = "wallet not found" };

              
            await _producerService.PublishVendBillAsync(pendingBill);
            return new ModelResponse<ElectricityBill> { Message = "Processing payment token would be sent", Status = true };
        }

      
    }
}
