namespace pms.app.Models
{
    public class CustomerItem
    {
        public int Id { get; set; }
        public required int CustomerId { get; set; }
        public int Quantity { get; set; }
        public required int ItemId { get; set; }
        public DateTime AssignedDate { get; set; } = DateTime.Now;

        public required Customer Customer { get; set; }
        public required Item Item { get; set; }
    }
}
