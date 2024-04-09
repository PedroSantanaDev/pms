using System.ComponentModel.DataAnnotations;
namespace pms.app.Models
{
    public class Item
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Item SKU is required.")]
        public int SKU { get; set; }
        [Required(ErrorMessage = "Item Name is required.")]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Item Price is required.")]
        public decimal Price { get; set; }
        public string Status { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; }
        // Foreign key property
        public int? CategoryId { get; set; }

        //Navigation property
        public Category? Category { get; set; }

        //public ICollection<CustomerItem>? CustomerItems { get; set; } = new List<CustomerItem>();
    }
}
