using Microsoft.AspNetCore.Mvc;
using Shopping_mvc8.Models;
using Shopping_mvc8.Repository;
using System.Security.Claims;

namespace Shopping_mvc8.Controllers
{
	public class CheckoutController : Controller
	{
		private readonly DataContext _dataContext;
		public CheckoutController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		public IActionResult Index()
		{
			return View();
		}
		public async Task<IActionResult> Checkout() 
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			if(userEmail == null)
			{
				return RedirectToAction("Login", "Account");
			}
			else
			{
				var ordercode = Guid.NewGuid().ToString();
				var orderitem = new OrderModel();
				orderitem.OrderCode = ordercode;
				orderitem.UserName = userEmail;
				orderitem.Status = 1;
				orderitem.CreatedDate = DateTime.Now;
				_dataContext.Add(orderitem);
				_dataContext.SaveChanges();
				List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
				foreach(var item in cartItems) 
				{
					var orderDetails = new OrderDetails();
					orderDetails.UserName = userEmail;
					orderDetails.OrderCode = ordercode;
					orderDetails.ProductId = item.ProductId  ;
					orderDetails.Price = item.Price;
					orderDetails.Quantity = item.Quantity;
					_dataContext.Add(orderDetails);
				}
				await _dataContext.SaveChangesAsync();
				HttpContext.Session.Remove("Cart");
				TempData["success"] = "Checkout success";
				return RedirectToAction("Index", "Cart");
			}
		}
	}
}
