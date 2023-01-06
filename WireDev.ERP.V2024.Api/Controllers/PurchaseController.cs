using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Api.Controllers
{
    [ApiController]
    //[Authorize("SELLS:RW")]
    [Route("api/[controller]")]
    public class PurchaseController : Controller
    {
        private readonly ILogger<PurchaseController> _logger;

        public PurchaseController(ILogger<PurchaseController> logger)
        {
            _logger = logger;
        }

        //[Authorize("PURCHASE_SELL:RW")]
        [HttpPost("sell")]
        public async Task<IActionResult> DoPurchase([FromBody] Product[] products)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        //[Authorize("PURCHASE_CANCEL:RW")]
        [HttpPost("cancel")]
        public async Task<IActionResult> CancelPurchase([FromBody] Product[] products)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        //[Authorize("PURCHASE_WITHDRAW:RW")]
        [HttpPost("withdraw")]
        public async Task<IActionResult> WithdrawPurchase([FromBody] Product[] products)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }
}

