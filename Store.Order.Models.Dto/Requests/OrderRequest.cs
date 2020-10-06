using MediatR;
using Store.Order.Models.Dto.Mapper;

namespace Store.Order.Models.Dto
{
    public class OrderRequest : IRequest, IMapTo<Models.Orders>
    {
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
        public Enums.OrderStatuses StatusId { get; set; }
        public int CustomerId { get; set; }
    }
}
