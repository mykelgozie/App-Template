using App.Dtos.ResquestModel;
using App.Models;
using System.Threading.Tasks;

namespace App.Core.Interfaces
{
    public interface IProducerService
    {
        Task PublishBillAsync(ElectricityBill electricityBill);
        Task PublishEmailAsync(EmailDto emailDto);
        Task PublishVendBillAsync(ElectricityBill electricityBill);
        Task PublishWalletAsync(WalletDto walletDto);
    }
}
