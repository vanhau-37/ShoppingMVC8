using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_mvc8.Models;
using Shopping_mvc8.Repository;

namespace Shopping_mvc8.Areas.Admin.Controllers
{
	[Area("Admin"), Authorize]
	public class OrderController : Controller
	{
		private readonly DataContext _dataContext;
		public OrderController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		public async Task<IActionResult> Index()
		{
			return View(await _dataContext.Orders.OrderByDescending(o => o.Id).ToListAsync());
		}
		public async Task<IActionResult> ViewOrder(string orderCode)
		{
			OrderModel order = await _dataContext.Orders.FirstOrDefaultAsync(o => o.OrderCode == orderCode);

			return View(await _dataContext.Orders.OrderByDescending(o => o.Id).ToListAsync());
		}
	}
}
