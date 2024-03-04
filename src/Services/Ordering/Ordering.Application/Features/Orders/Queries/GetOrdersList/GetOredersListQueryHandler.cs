using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOredersListQueryHandler : IRequestHandler<GetOredersListQuery, List<OrderVm>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOredersListQueryHandler(IMapper mapper, IOrderRepository orderRepository)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
        }

        public async Task<List<OrderVm>> Handle(GetOredersListQuery request, CancellationToken cancellationToken)
        {
            var orderList = await _orderRepository.GetOrdersByUserName(request.UserName);

            var res  = _mapper.Map<List<OrderVm>>(orderList);

            return res;
        }
    }
}
