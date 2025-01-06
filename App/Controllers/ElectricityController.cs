using App.Core.Interfaces;
using App.Core.Services;
using App.Dtos.ResquestModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElectricityController : ControllerBase
    {
        private IElectricityBillService _electricityBillService;

        public ElectricityController(IElectricityBillService electricityBillService)
        {
            _electricityBillService = electricityBillService;
        }

        [HttpPost("verify")]
        public async Task<IActionResult> PayBill([FromBody] BillDto billDto)
        {
            var billesponse = await _electricityBillService.CreateBill(billDto);
            return Ok(billesponse);
        }


        [HttpPost("Vend/{validationRef}/pay")]
        public async Task<IActionResult> VerifyElectricityBill(string validationRef)
        {
            var vendingResponse = await _electricityBillService.VendBill(validationRef);
            return Ok(vendingResponse);
        }
    }
}
