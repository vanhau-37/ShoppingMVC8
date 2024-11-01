using Microsoft.EntityFrameworkCore;
using Shopping_mvc8.Models;

namespace Shopping_mvc8.Repository
{
	public class SeedData
	{
		public static void SeedingData(DataContext _context)
		{
			_context.Database.Migrate();
			if(!_context.Products.Any())
			{
				CategoryModel macbook = new CategoryModel { Name = "Macbook", Slug = "macbook", Description = "Macbook is very expensive", Status = 1 };
				CategoryModel pc = new CategoryModel { Name = "Pc", Slug = "pc", Description = "Pc is very most", Status = 1 };

				BrandModel apple = new BrandModel { Name = "Apple", Slug = "apple", Description = "Apple is large brand in the world", Status = 1 };
				BrandModel samsung = new BrandModel { Name = "Samsung", Slug = "samsung", Description = "Apple is large brand in the world", Status = 1 };

				_context.Products.AddRange(
					new ProductModel { Name = "Macbook", Slug = "macbook", Description = "Macbook is best", Image = "1.jpg", Category = macbook, Brand = apple, Price = 1313 },
					new ProductModel { Name = "Pc", Slug = "pc", Description = "Pc is best", Image = "2.jpg", Category = pc, Brand = samsung, Price = 999 }
				);
				_context.SaveChanges();
			}
		}
	}
}
