using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping_mvc8.Models;
using Shopping_mvc8.Repository;

namespace Shopping_mvc8.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(DataContext Context, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = Context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Products.OrderByDescending(p =>p.Id).Include(p => p.Category).Include(p => p.Brand).ToListAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
            ViewBag.brands = new SelectList(_dataContext.Brands, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModel product)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);

            if (ModelState.IsValid)
            {
                product.Slug = product.Name.Replace(" ", "-").ToLower();
                var slug = _dataContext.Products.FirstOrDefault(p => p.Slug == product.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The database has this product.");
                    return View(product);
                }

                if (product.ImageUpLoad != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imageName = Guid.NewGuid().ToString() + Path.GetExtension(product.ImageUpLoad.FileName);
                    string filePath = Path.Combine(uploadsDir, imageName);

                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        await product.ImageUpLoad.CopyToAsync(fs);
                    }
                    product.Image = imageName;
                }

                _dataContext.Add(product);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Add product success";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Model has some error";
                return View(product); 
            }
        }

        public async Task<IActionResult> Edit(int Id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);
			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
			ViewBag.brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);

			return View(product);
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(ProductModel product)
		{
			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
			ViewBag.brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
            var existed_product = _dataContext.Products.Find(product.Id);
			if (ModelState.IsValid)
			{
				product.Slug = product.Name.Replace(" ", "-").ToLower();
				var slug = _dataContext.Products.FirstOrDefault(p => p.Slug == product.Slug);
				
				if (product.ImageUpLoad != null)
				{
                    //upload new image
					string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
					string imageName = Guid.NewGuid().ToString() + Path.GetExtension(product.ImageUpLoad.FileName);
					string filePath = Path.Combine(uploadsDir, imageName);

                    //delete old image 
                    string oldfileimage = Path.Combine(uploadsDir, existed_product.Image);
                    try
                    {
                        if (System.IO.File.Exists(oldfileimage))
                        {
                            System.IO.File.Delete(oldfileimage);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "An error occurred while deleting the product image.");
                    }
                    FileStream fs = new FileStream(filePath, FileMode.Create);
					await product.ImageUpLoad.CopyToAsync(fs);
					fs.Close();
					existed_product.Image = imageName;
                }
                existed_product.Name = product.Name;
                existed_product.Price = product.Price;
                existed_product.Description = product.Description;
                existed_product.CategoryId = product.CategoryId;
                existed_product.BrandId = product.BrandId;

				_dataContext.Update(existed_product);

				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Update product success";
				return RedirectToAction("Index");
			}
			else
			{
				TempData["error"] = "Model has some error";
				return View(product);
			}
		}

		public async Task<IActionResult> Delete(int Id)
        {
			ProductModel product = await _dataContext.Products.FindAsync(Id);
            if(!string.Equals(product.Image, "noimage.jpg"))
            {
				string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
				string oldfileimage = Path.Combine(uploadsDir, product.Image);
                if(System.IO.File.Exists(oldfileimage))
                {
                    System.IO.File.Delete(oldfileimage);
                }
			}
            _dataContext.Products.Remove(product);
            await _dataContext.SaveChangesAsync();
			TempData["success"] = "Delete product success";
            return RedirectToAction("Index");
		}
	}
}
