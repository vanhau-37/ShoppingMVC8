using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shopping_mvc8.Repository;

namespace Shopping_mvc8.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        public ProductController(DataContext Context) 
        { 
            _dataContext = Context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int Id)
        {
            if(Id == null) return RedirectToAction("Index");
            var productById = _dataContext.Products.Where(predicate => predicate.Id == Id).FirstOrDefault();
            return View(productById);
        }
    }
}
