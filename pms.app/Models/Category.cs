using System.ComponentModel.DataAnnotations;

namespace pms.app.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Category Name is required.")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; }

        public ICollection<Item>? Items { get; set; }
    }
}
