using System.ComponentModel.DataAnnotations;

namespace Shopping_mvc8.Models
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        public string Slug { get; set; }
        public int Status { get; set; }
    }
}
