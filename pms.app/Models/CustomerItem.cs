using System.ComponentModel.DataAnnotations;

namespace pms.app.Models
{
    public class CustomerItem
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
        public int Quantity { get; set; }
        public int? ItemId { get; set; }
        public DateTime AssignedDate { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; }

        public Customer Customer { get; set; }
        public Item Item { get; set; }
    }
}
