

namespace BuianhtuanAssignment.Models.Request
{
    public class EditCustomerModel
    {
        public string Name { get; set; } = null!;

        public int? Age { get; set; }

        public string? Gender { get; set; }

        public string? Address { get; set; }

        public int Status { get; set; }

        public string? Description { get; set; }
    }
}
