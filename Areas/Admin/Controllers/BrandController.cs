using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_mvc8.Models;
using Shopping_mvc8.Repository;

namespace Shopping_mvc8.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class BrandController : Controller
    {
        private readonly DataContext _dataContext;
        public BrandController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Brands.OrderByDescending(b => b.Id).ToListAsync());
        }

        public async Task<IActionResult> Delete(int Id)
        {
            BrandModel brand = await _dataContext.Brands.FindAsync(Id);
            _dataContext.Brands.Remove(brand);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Delete brand success";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandModel brand)
        {
            if (ModelState.IsValid)
            {
                brand.Slug = brand.Name.Replace(" ","-").ToLower();
                var slug = await _dataContext.Brands.FirstOrDefaultAsync(b => b.Slug == brand.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The database has this brand.");
                    return View(brand);
                }
                _dataContext.Add(brand);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Add brand success";
                return RedirectToAction("Index");
            }
            else
            {
                List<string> errors = new List<string>();
                foreach( var value in ModelState.Values)
                {
                    foreach( var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMesage = string.Join("\n", errors);
                return BadRequest(errorMesage);
            }
        }
        public async Task<IActionResult> Edit(int Id)
        {
            BrandModel brand = await _dataContext.Brands.FindAsync(Id);
            return View(brand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BrandModel brand)
        {
            if(ModelState.IsValid)
            {
                brand.Slug = brand.Name.Replace(" ","-").ToLower();
                _dataContext.Update(brand);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Edit category success";
                return RedirectToAction("Index");
            }
            else
            {
                List<string> errors = new List<string>();
                foreach( var value in ModelState.Values )
                {
                    foreach( var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n",errors);
                return BadRequest(errorMessage);
            }
        }
    }
}
