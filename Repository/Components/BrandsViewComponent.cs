using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Shopping_mvc8.Repository.Components
{
	public class BrandsViewComponent : ViewComponent
	{
		private readonly DataContext _DataContext;
		public BrandsViewComponent(DataContext dataContext)
		{
			_DataContext = dataContext;
		}
		public async Task<IViewComponentResult> InvokeAsync() => View(await _DataContext.Brands.ToListAsync());
	}
}
