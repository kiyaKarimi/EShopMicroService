using BuildingBlocks.Exceptions;

namespace Ordering.Application.Extensions
{
    public class OrderNotFoundException : NotFoundException
    {
        public OrderNotFoundException(Guid id) : base("Order", id)
        {
        }
    }
}
