namespace pms.app.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public required string Status { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; }

        public ICollection<CustomerItem>? CustomerItems { get; set; } = new List<CustomerItem>();
    }
}
