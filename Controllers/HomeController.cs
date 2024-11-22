using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_mvc8.Models;
using Shopping_mvc8.Repository;
using System.Diagnostics;

namespace Shopping_mvc8.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _logger = logger;
            _dataContext = context;
        }

        public IActionResult Index(int pg =1)
        {
            List<ProductModel> product = _dataContext.Products.Include(p => p.Category).Include(p => p.Brand).ToList();
            const int pageSize = 3;
            if(pg < 1)
            {
                pg = 1;
            }
            int recsCount = product.Count();
            var pager = new Paginate(recsCount, pg, pageSize);
            int recSkip = pageSize*(pg-1);
            var data = product.Skip(recSkip).Take(pager.PageSize).ToList();
            ViewBag.Pager = pager;
            //var products = _dataContext.Products.Include("Category").Include("Brand").ToList();
            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statuscode)
        {
            if(statuscode == 404)
            {
                return View("NotFound");
            }
            else return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
