

namespace BuianhtuanAssignment.Models.Request
{
    public class SearchCustomerModel
    {
        public string Name { get; set; } = "";

        public int? Age { get; set; } = 0!;

        public string? Gender { get; set; } ="";

        public string? Address { get; set; } = "";
    }
}
