using System.ComponentModel.DataAnnotations;
namespace pms.app.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Customer Name is required.")]
        public string Name { get; set; }
        public string? Email { get; set; }
        [Required(ErrorMessage = "Customer Phone is required.")]
        [RegularExpression(@"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$", ErrorMessage = "Invalid phone number format. Please provide a valid phone number.")]
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string Status { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; }

        public ICollection<CustomerItem>? CustomerItems { get; set; } = new List<CustomerItem>();
    }
}
