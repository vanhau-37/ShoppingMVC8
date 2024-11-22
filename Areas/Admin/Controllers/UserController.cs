using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping_mvc8.Models;
using Shopping_mvc8.Repository;
using System.Numerics;

namespace Shopping_mvc8.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/User")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _dataContext;
        public UserController(UserManager<AppUserModel> userManager, RoleManager<IdentityRole> roleManager, DataContext dataContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dataContext = dataContext;
        }
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var usersWithRoles = await (from u in _dataContext.Users
                                        join ur in _dataContext.UserRoles on u.Id equals ur.UserId
                                        join r in _dataContext.Roles on ur.RoleId equals r.Id
                                        select new { User = u, RoleName = r.Name }).ToListAsync();
            return View(usersWithRoles);
        }
        [Route("Create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            return View(new AppUserModel());
        }

        [Route("Create")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(AppUserModel user)
        {
            if(ModelState.IsValid)
            {
                var createUserResult = await _userManager.CreateAsync(user, user.PasswordHash);//tạo user
                if(createUserResult.Succeeded)
                {
                    var newUser = await _userManager.FindByEmailAsync(user.Email);//lấy user vừa tạo 
                    var role = _roleManager.FindByIdAsync(newUser.RoleId);//lấy role
                    var addToRoleResult = await _userManager.AddToRoleAsync(newUser, role.Result.Name);
                    if (!addToRoleResult.Succeeded)
                    {
                        AddIdentityErrors(addToRoleResult);
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    AddIdentityErrors(createUserResult);
                    return View(user);
                }
            }
            else
            {
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMesage = string.Join("\n", errors);
                return BadRequest(errorMesage);
            }
        }
        [Route("Delete")]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                return NotFound();
            }
            var deleteResult = await _userManager.DeleteAsync(user);
            if(!deleteResult.Succeeded)
            {
                return View("Error");
            }
            TempData["success"] = "Delete success";
            return RedirectToAction("Index");
        }

		[Route("Edit")]
        [HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			if(string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                return NotFound();
            }
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            return View(user);
		}
        [Route("Edit")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, AppUserModel user)
        {
            var existingUser = await _userManager.FindByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.RoleId = user.RoleId;

                var updateUserResult = await _userManager.UpdateAsync(existingUser);
                if (updateUserResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddIdentityErrors(updateUserResult);
                    return View(existingUser);
                }
            }
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            return View(existingUser);
            

        }

        private void AddIdentityErrors(IdentityResult result)
        {
            foreach(var error in result.Errors )
            {
                ModelState.AddModelError("", error.Description);
            }
        }
	}
}
