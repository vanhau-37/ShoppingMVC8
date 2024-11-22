using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_mvc8.Models;
using Shopping_mvc8.Repository;

namespace Shopping_mvc8.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
	{
		private readonly DataContext _dataContext;
		public CategoryController(DataContext context)
		{
			_dataContext = context;
		}
		public async Task<IActionResult> Index(int pg =1)
		{
			List<CategoryModel> category = _dataContext.Categories.ToList();
			const int pageSize = 10;
			if(pg < 1)
			{
				pg = 1;
			}
			int recsCount = category.Count();// dem so item 
			var pager = new Paginate(recsCount, pg, pageSize);
			int recSkip = (pg-1) * pageSize;
			var data = category.Skip(recSkip).Take(pager.PageSize).ToList();
			ViewBag.Pager = pager;
			return View(data);
		}
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CategoryModel category)
		{
			if(ModelState.IsValid)
			{
				category.Slug = category.Name.Replace(" ","-").ToLower();
				var slug = _dataContext.Categories.FirstOrDefault(c => c.Slug ==  category.Slug);
				if(slug != null)
				{
					ModelState.AddModelError("", "The database has this categoy.");
					return View(category);
				}
				_dataContext.Add(category);
				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Add category success";
				return RedirectToAction("Index");

			}
			else
			{
				TempData["error"] = "Model has some error";
				List<string> errors = new List<string>();
				foreach( var value in ModelState.Values )
				{
					foreach( var error in value.Errors )
					{
						errors.Add(error.ErrorMessage);
					}
				}
				string errorMessage = string.Join("\n", errors);
				return BadRequest(errorMessage);
            }
		}

		public async Task<IActionResult> Delete(int Id)
		{
			CategoryModel category = await _dataContext.Categories.FindAsync(Id);
			_dataContext.Remove(category);
			await _dataContext.SaveChangesAsync();
			TempData["success"] = "Delete category success";
			return RedirectToAction("Index");

        }

		public async Task<IActionResult> Edit(int Id)
		{
			CategoryModel category = await _dataContext.Categories.FindAsync(Id);
			return View(category);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.Replace(" ", "-").ToLower();
                _dataContext.Update(category);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Add category success";
                return RedirectToAction("Index");

            }
            else
            {
                TempData["error"] = "Model has some error";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
        }
    }
}
