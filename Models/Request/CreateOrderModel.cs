

namespace BuianhtuanAssignment.Models.Request
{
    public class CreateOrderModel
    {
        public long ProductId { get; set; }

        public long CustomerId { get; set; }

        public int Amount { get; set; }

        public decimal Price { get; set; }
    }
}
