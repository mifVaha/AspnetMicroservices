using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;
        public CheckoutOrderCommandHandler(IMapper mapper, IOrderRepository orderRepository, IEmailService emailService, ILogger<CheckoutOrderCommandHandler> logger)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _emailService = emailService;
            _logger = logger;
        }
        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var mapperdOrder = _mapper.Map<Order>(request);
            var newOrder = await _orderRepository.AddAsync(mapperdOrder);

            _logger.LogInformation($"New order with id: {newOrder.Id} was succsessfully addded.");

            await SendEmail(newOrder);

            return newOrder.Id;
        }

        private async Task SendEmail(Order order)
        {
            var email = new Email() { To = "support@gmail.com", Body = $"Order was created.", Subject = "Order was created" };

            try
            {
                await _emailService.SendEmail(email);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Order {order.Id} failed due to an error with the mail service: {ex.Message}");
            }
        }
    }
}
