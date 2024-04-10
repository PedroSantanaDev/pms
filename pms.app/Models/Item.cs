using System.ComponentModel.DataAnnotations;
namespace pms.app.Models
{
    public class Item
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Item SKU is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
        public int SKU { get; set; }
        [Required(ErrorMessage = "Item Name is required.")]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Item Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Item Price must be a positive number.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Item Status is required.")]
        public string Status { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; }
        public int? CategoryId { get; set; }

        public Category? Category { get; set; }

    }
}
