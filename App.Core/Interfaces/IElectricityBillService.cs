using App.Dtos.ResponseModel;
using App.Dtos.ResquestModel;
using App.Models;
using System.Threading.Tasks;

namespace App.Core.Interfaces
{
    public interface IElectricityBillService
    {
        Task<ModelResponse<ElectricityBill>> CreateBill(BillDto billDto);
        Task<ModelResponse<ElectricityBill>> VendBill(string verificationRef);
    }
}
