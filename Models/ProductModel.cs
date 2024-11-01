using Shopping_mvc8.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping_mvc8.Models
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }

		[Required/*(ErrorMessage = "Yêu cầu nhập tên sản phẩm")*/]
		public string Name { get; set; }

		[Required/*(ErrorMessage = "Yêu cầu nhập mô tả sản phẩm")*/]
		public string Description { get; set; }
        public string Slug { get; set; }

        [Required/*(ErrorMessage = "Yêu cầu nhập giá sản phẩm")*/]
        [Range(0, Double.MaxValue)]
        public decimal Price { get; set; }

        [NotMapped]
        [FileExtension]
        public IFormFile? ImageUpLoad { get; set; }

        public string Image {  get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Choose 1 Catagory")]
        public int CategoryId { get; set; }
        [Required,Range(1, int.MaxValue, ErrorMessage ="Choose 1 Brand")]
        public int BrandId { get; set; }

        public CategoryModel Category { get; set; }
        public BrandModel Brand { get; set; }

        
    }
}
