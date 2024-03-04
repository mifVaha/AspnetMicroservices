using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOredersListQuery : IRequest<List<OrderVm>>
    {
        public string UserName { get; set; }

        public GetOredersListQuery(string userName)
        {
            UserName = userName;
        }
    }
}
