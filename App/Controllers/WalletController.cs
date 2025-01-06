using App.Core.Interfaces;
using App.Dtos.ResquestModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IProducerService _producerService;

        public WalletController(IProducerService producerService)
        {
            _producerService = producerService;
        }



        [HttpPost("{id}/add-funds")]
        public async Task<IActionResult> AddFunds(string id, [FromBody] WalletDto walletDto)
        {
            walletDto.WalletId = id;
            await _producerService.PublishWalletAsync(walletDto);
            return Ok(new { message = "Toping up wallet" });
        }
    }
}
