using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_mvc8.Models;
using Shopping_mvc8.Repository;
using System.Collections.Specialized;

namespace Shopping_mvc8.Controllers
{
	public class BrandController : Controller
	{
		private readonly DataContext _dataContext;
		public BrandController(DataContext Context)
		{
			_dataContext = Context;
		}
		public async Task<IActionResult> Index(string Slug = "")
		{
			BrandModel brand = _dataContext.Brands.Where(b => b.Slug == Slug).FirstOrDefault();
			if (brand == null) return RedirectToAction("Index");
			return View( await _dataContext.Products.Where(p => p.BrandId == brand.Id)
				.OrderByDescending(p => p.Id).ToListAsync());
		}
	}
}
