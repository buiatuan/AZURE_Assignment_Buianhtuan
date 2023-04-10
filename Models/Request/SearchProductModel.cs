

namespace BuianhtuanAssignment.Models.Request
{
    public class SearchProductModel
    {

        public string Name { get; set; } = "";

        public decimal Price { get; set; } = 0;

        public int Amount { get; set; } = 0;

        public string? Description { get; set; } = ""; 
    }
}
