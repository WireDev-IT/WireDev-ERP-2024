using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WireDev.Erp.V1.Models.Storage;
using WireDev.Erp.V1.Api.Context;

namespace WireDev.Erp.V1.Api.Controllers
{
    [ApiController]
    //[Authorize("PURCHASE:RO")]
    [Route("api/[controller]")]
    public class PurchaseController : Controller
    {
        private readonly PurchaseDbContext _context;
        private readonly ILogger<PurchaseController> _logger;

        public PurchaseController(PurchaseDbContext context, ILogger<PurchaseController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetPurchases()
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurchase(Guid id)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        //[Authorize("PURCHASE_BUY:RW")]
        [HttpPost("buy")]
        public async Task<IActionResult> BuyPurchase([FromBody] Purchase puchase)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        //[Authorize("PURCHASE_SELL:RW")]
        [HttpPost("sell")]
        public async Task<IActionResult> SellPurchase([FromBody] Purchase puchase)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        //[Authorize("PURCHASE_CANCEL:RW")]
        [HttpPost("cancel")]
        public async Task<IActionResult> CancelPurchase([FromBody] Purchase puchase)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        //[Authorize("PURCHASE_WITHDRAW:RW")]
        [HttpPost("withdraw")]
        public async Task<IActionResult> WithdrawPurchase([FromBody] Purchase puchase)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }
}

