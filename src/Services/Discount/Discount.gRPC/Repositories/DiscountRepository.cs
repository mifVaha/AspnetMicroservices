using Dapper;
using Discount.gRPC.Entities;
using Npgsql;

namespace Discount.gRPC.Repositories
{
	public class DiscountRepository : IDiscountRepository
	{
		private readonly IConfiguration _configuration;

		public DiscountRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<bool> CreateDiscount(Coupon coupon)
		{
			using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var res = await connection.ExecuteAsync("insert into Coupon (ProductName, Description, Amount) Values (@ProductName, @Description, @Amount)", 
				new { coupon.ProductName, coupon.Description, coupon.Amount } );
			
			if(res == 0)
				return false;

			return true;
		}

		public async Task<bool> DeleteDiscount(string productName)
		{
			using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var res = await connection.ExecuteAsync("delete from Coupon where ProductName = @ProductName",
				new {  ProductName = productName });

			if (res == 0)
				return false;

			return true;
		}

		public async Task<Coupon> GetDiscount(string productName)
		{
			using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("Select * from Coupon where ProductName = @ProductName", new { ProductName = productName });
			
			if(coupon == null)
				return new Coupon { ProductName = "No Discount", Description = "No Discount Desc", Amount = 0 };

			return coupon;
		}

		public async Task<bool> UpdateDiscount(Coupon coupon)
		{
			using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var res = await connection.ExecuteAsync("update Coupon set ProductName = @ProductName, Description = @Description, Amount = @Amount Where Id = @Id",
				new { coupon.ProductName, coupon.Description, coupon.Amount, coupon.Id });

			if (res == 0)
				return false;

			return true;
		}
	}
}
