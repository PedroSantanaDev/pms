namespace pms.app.Models
{
    public class Item
    {
        public int Id { get; set; }
        public int SKU { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; }
        // Foreign key property
        public int? CategoryId { get; set; }

        //Navigation property
        public Category? Category { get; set; }
    }
}
