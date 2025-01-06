using App.Core.Interfaces;
using App.Data;
using App.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace App.Core.Repositories
{
    public class ElectricityBillRepository : IElectricityBillRepository
    {
        private AppDbContext _appDbContext;

        public ElectricityBillRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ElectricityBill> CreateElectricityBill(ElectricityBill electricityBill)
        {
            await _appDbContext.ElectricityBills.AddAsync(electricityBill);
            return await _appDbContext.SaveChangesAsync() > 0 ? electricityBill : null;
        }

        public async Task<ElectricityBill> GetElectricityBillByVerificationRef(string verificationRef)
        {

            var bill = await _appDbContext.ElectricityBills.FirstOrDefaultAsync(x => x.VerificationRef == verificationRef);
            return bill;
        }

        public async Task<ElectricityBill> UpdateElectricityBill(ElectricityBill electricityBill)
        {
            _appDbContext.ElectricityBills.Update(electricityBill);
            var result = await _appDbContext.SaveChangesAsync();
            return result > 0 ? electricityBill : null;
        }

    }
}
