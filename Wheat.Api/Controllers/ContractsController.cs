using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Wheat.Api.Service;
using Wheat.DataLayer.Repositories.Interfaces;
using Wheat.Models.Requests;
using Wheat.Models.Responses;

namespace Wheat.Api.Controllers
{
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContractsController : ControllerExt
    {
        private readonly IContractRepository _contracts;
        public ContractsController(IContractRepository contracts) : base()
        {
            _contracts = contracts;
        }


        [HttpGet]
        [SwaggerResponse(200, "", typeof(ContractsResult))]
        public async Task<IActionResult> Get()
        {
            var result = await _contracts.GetMyContractsAsync(UserId);
            return Json(result);
        }

        [HttpGet("sell")]
        [SwaggerResponse(200, "", typeof(Contracts))]
        public async Task<IActionResult> GetSell()
        {
            var result = await _contracts.GetMySellContracts(UserId);
            return Json(result);
        }

        [HttpGet("buy")]
        [SwaggerResponse(200, "", typeof(Contracts))]
        public async Task<IActionResult> GetBuy()
        {
            var result = await _contracts.GetMyBuyContracts(UserId);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostContractsRequest request)
        {
            await _contracts.CreateContractsAsync(UserId, request.Count, request.Price, request.Sell);
            return Ok();
        }

        [HttpPost("deal")]
        public async Task<IActionResult> Post([FromBody] DealRequest request)
        {
            await _contracts.DealAsync(UserId, request.Code, request.Count, request.Price, request.Buy);
            return Ok();
        }

        [HttpPost("resell")]
        public async Task<IActionResult> Post([FromBody] ReSellRequest request)
        {
            await _contracts.ReSellContractsAsync(UserId, request.Code, request.Count, request.OldPrice,
                request.NewPrice);
            return Ok();
        }

        [HttpPost("redeal")]
        public async Task<IActionResult> Post([FromBody] ReDealRequest request)
        {
            await _contracts.DealAsync(UserId, request.BuyerCode, request.SellerCode, request.Count, request.Price);
            return Ok();
        }

        [HttpGet("balance")]
        [SwaggerResponse(200,"",typeof(BalanceResults))]
        public async Task<IActionResult> GetBalance()
        {
            var result = await  _contracts.GetTotalBalanceAsync(UserId);
            return Json(result);
        }

        [HttpGet("publicsell")]
        [SwaggerResponse(200,"",typeof(List<ContractPrice>))]
        public async Task<IActionResult> GetPublicSell()
        {
            var result = await _contracts.GetPublicSellContracts(UserId);
            return Json(new {data = result});
        }

        [HttpGet("publicbuy")]
        [SwaggerResponse(200, "", typeof(List<ContractPrice>))]
        public async Task<IActionResult> GetPublicBuy()
        {
            var result = await _contracts.GetPublicBuyContracts(UserId);
            return Json(new { data = result });
        }
    }
}
