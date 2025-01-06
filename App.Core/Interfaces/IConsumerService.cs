using System.Threading.Tasks;

namespace App.Core.Interfaces
{
    public interface IConsumerService
    {
        void BillConsumer();
        void EmailConsumer();
        void VendBillConsumer();
        void WalletConsumer();
    }
}
