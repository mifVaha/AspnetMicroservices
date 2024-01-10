﻿using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Discount.API.Controller
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class DiscountController : ControllerBase
	{
		private readonly IDiscountRepository _repository;

		public DiscountController(IDiscountRepository repository)
		{
			_repository = repository;
		}

		[HttpGet("{productName}", Name = "GetDiscount")]
		[ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<Coupon>> GetDiscount(string productName)
		{ 
			var coupon = await _repository.GetDiscount(productName);

			return Ok(coupon);
		}

		[HttpPost]
		[ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<Coupon>> CreateDiscount([FromBody] Coupon coupon)
		{	await _repository.CreateDiscount(coupon);
			return CreatedAtRoute("GetDiscount", new { coupon.ProductName }, coupon);
		}
		[HttpPut]
		[ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<bool>> UpdateDiscount([FromBody] Coupon coupon)
		{
			return (Ok(await _repository.UpdateDiscount(coupon)));
		}

		[HttpDelete("{productName}", Name = "DeleteDiscount")]
		[ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<bool>> DeleteDiscount(string productName)
		{
			return Ok(await _repository.DeleteDiscount(productName));
		}
	}
}
