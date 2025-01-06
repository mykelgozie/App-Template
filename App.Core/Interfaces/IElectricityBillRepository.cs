using App.Dtos.ResponseModel;
using App.Models;
using System.Threading.Tasks;

namespace App.Core.Interfaces
{
    public interface IElectricityBillRepository
    {
        Task<ElectricityBill> CreateElectricityBill(ElectricityBill electricityBill);
        Task<ElectricityBill> GetElectricityBillByVerificationRef(string verificationRef);
        Task<ElectricityBill> UpdateElectricityBill(ElectricityBill electricityBill);
    }
}
