using AutoMapper;
using MassTransit;
using MediatR;
using Store.Order.Models.Dto;
using Store.Order.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Store.Order.Application.Commands
{
    public class CreateOrderHandler : AsyncRequestHandler<OrderRequest>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        public CreateOrderHandler(IMapper mapper, IOrderRepository orderRepository, IPublishEndpoint publishEndpoint)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _publishEndpoint = publishEndpoint;
        }

        protected override async Task Handle(OrderRequest request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Models.Orders>(request);

            await _orderRepository.Insert(entity);

            await _publishEndpoint.Publish<CreatedOrder>(new CreatedOrder { CustomerId = entity.CustomerId, OrderId = entity.OrderId }, cancellationToken);
        }
    }
}
