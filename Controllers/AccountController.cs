using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shopping_mvc8.Models;
using Shopping_mvc8.Models.ViewModels;

namespace Shopping_mvc8.Controllers
{
	public class AccountController : Controller
	{
		private UserManager<AppUserModel> _userManager;
		private SignInManager<AppUserModel> _signInManager;
		public AccountController(UserManager<AppUserModel> userManager, SignInManager<AppUserModel> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public IActionResult Login(string returnURL)
		{
			return View(new LoginViewModel { ReturnURL = returnURL});
		}
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginvm)
		{
			if (ModelState.IsValid)
			{
				Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginvm.UserName, loginvm.Password,false,false);
				if (result.Succeeded)
				{
					return Redirect(loginvm.ReturnURL ?? "/");
				}
				ModelState.AddModelError("", "Invalid username and password");
			}
			return View(loginvm);
		}

        public IActionResult Create()
		{
			return View();
		}
		[HttpPost,ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(UserModel user)
		{
			if(ModelState.IsValid)
			{
				AppUserModel newuser = new AppUserModel { UserName = user.UserName, Email = user.Email};
				IdentityResult result = await _userManager.CreateAsync(newuser,user.Password);
				if(result.Succeeded)
				{
					TempData["success"] = "Register success";
					return Redirect("/Account/Login");
				}
				foreach(IdentityError error in result.Errors)
				{
					ModelState.AddModelError("",error.Description);
				}
			}
			return View(user);
		}
		public async Task<IActionResult> Logout(string returnURL = "/")
		{
			await _signInManager.SignOutAsync();
			return Redirect(returnURL);
		}

	}
}
