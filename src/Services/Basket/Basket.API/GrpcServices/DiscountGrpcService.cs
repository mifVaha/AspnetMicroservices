using Discount.gRPC.Protos;

namespace Basket.API.GrpcServices
{
	public class DiscountGrpcService
	{
		private readonly DiscountProtoService.DiscountProtoServiceClient _serviceGrpcServiceClient;

		public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient serviceGrpcServiceClient)
		{
			_serviceGrpcServiceClient = serviceGrpcServiceClient;
		}

		public async Task<CouponModel> GetDiscount(string productName) {
			var request = new GetDiscountRequest { ProductName = productName };

			var res = await _serviceGrpcServiceClient.GetDiscountAsync(request);
			
			return res;
		}
	}
}
