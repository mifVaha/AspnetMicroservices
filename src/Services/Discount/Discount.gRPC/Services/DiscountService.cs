using AutoMapper;
using Discount.gRPC.Entities;
using Discount.gRPC.Protos;
using Discount.gRPC.Repositories;
using Grpc.Core;

namespace Discount.gRPC.Services
{
	public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
	{
		private readonly IDiscountRepository _repository;
		private readonly IMapper _mapper;
		private readonly ILogger<DiscountService> _logger;

		public DiscountService(IDiscountRepository repository, IMapper mapper, ILogger<DiscountService> logger)
		{
			_repository = repository;
			_mapper = mapper;
			_logger = logger;
		}

		public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
		{
			var coupon = await _repository.GetDiscount(request.ProductName);

			if (coupon == null)
				throw new RpcException(new Status(StatusCode.NotFound, $"Discount with productName{request.ProductName} not found"));
			_logger.LogInformation($"Discount is retrived for ProductName: {coupon.ProductName}, Amount: {coupon.Amount}, Descriprtion: {coupon.Description}");

			var couponModel = _mapper.Map<CouponModel>(coupon);

			return couponModel;
		}
		public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
		{
			var coupon = _mapper.Map<Coupon>(request.Coupon);
			await _repository.CreateDiscount(coupon);

			_logger.LogInformation("Discount was created ProductName: {ProductName}", coupon.ProductName);

			var couponModel = _mapper.Map<CouponModel>(coupon);

			return couponModel;
		}

		public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
		{
			var coupon = _mapper.Map<Coupon>(request.Coupon);
			await _repository.UpdateDiscount(coupon);

			_logger.LogInformation("Discount was updated ProductName: {ProductName}", coupon.ProductName);

			var couponModel = _mapper.Map<CouponModel>(coupon);

			return couponModel;
		}

		public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
		{
			var res = await _repository.DeleteDiscount(request.ProductName);

			if(res == true)
				_logger.LogInformation("Discount was removed ProductName: {ProductName}", request.ProductName);

			return new DeleteDiscountResponse { 
				Success = res
			};
		}
	}
}
