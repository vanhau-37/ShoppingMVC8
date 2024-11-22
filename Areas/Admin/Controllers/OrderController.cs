using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_mvc8.Models;
using Shopping_mvc8.Repository;

namespace Shopping_mvc8.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
	{
		private readonly DataContext _dataContext;
		public OrderController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		public async Task<IActionResult> Index(int pg =1)
		{
			List<OrderModel> order = _dataContext.Orders.ToList();
			const int pageSize = 10;
			if(pg > 0)
			{
				pg = 1;
			}
			int recsCount = order.Count();
			var pager = new Paginate(recsCount,pg,pageSize);
			int recSkip = (pg-1) * pageSize;
			var data = order.Skip(recSkip).Take(pager.PageSize).ToList();
			ViewBag.Pager = pager;
			return View(data);
		}
		public async Task<IActionResult> ViewOrder(string orderCode)
		{
			OrderModel order = await _dataContext.Orders.FirstOrDefaultAsync(o => o.OrderCode == orderCode);

			return View(await _dataContext.Orders.OrderByDescending(o => o.Id).ToListAsync());
		}
	}
}
