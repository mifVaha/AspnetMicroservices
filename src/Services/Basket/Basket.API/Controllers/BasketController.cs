using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{

	[ApiController]
	[Route("api/v1/[controller]")]
	public class BasketController : ControllerBase
	{
		private readonly IBasketRepository _basketRepository;
		private readonly DiscountGrpcService _discountGrpcService;

		public BasketController(IBasketRepository basketRepository, DiscountGrpcService discountGrpcService)
		{
			_basketRepository = basketRepository;
			_discountGrpcService = discountGrpcService;
		}

		[HttpGet("{userName}", Name = "GetBasket")]
		[ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<ShoppingCart>> GetBasket(string userName) 
		{
			var basket = await _basketRepository.GetBasket(userName);
			return Ok(basket ?? new ShoppingCart(userName));
		}
		[HttpPost]
		[ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
		{
			foreach (var item in basket.Items)
			{
				var coupon = await _discountGrpcService.GetDiscount(item.ProductName);

				if(coupon.Amount < item.Price && coupon.Amount > 0)
					item.Price -= coupon.Amount;
			}
			return Ok(await (_basketRepository.UpdateBasket(basket)));
		}
		[HttpPost("{userName}", Name = "DeleteBasket")]
		[ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
		public async Task DeleteBasket(string userName)
		{
			await _basketRepository.DeleteBasket(userName);
		}
	}
}
