using Doorang.Core.DTOs.AccountDto;
using Doorang.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DoorangApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            User user = new User()
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Surname = registerDto.Surname,
                UserName = registerDto.UserName,
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
             await _userManager.AddToRoleAsync(user,"Member");
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var user = await _userManager.FindByNameAsync(loginDto.EmailOrUsername);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(loginDto.EmailOrUsername);
                if (user == null)
                {
                    ModelState.AddModelError("", "Email/Username ve ya password yanlisdir!!!");
                    return View();
                }
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Birazdan yeniden sina");
                return View();
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Email/Usernema ve ya password yanlisdir");
                return View();
            }
           await _signInManager.SignInAsync(user, loginDto.IsRemember);

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> CreateRole()
        {
            IdentityRole role= new IdentityRole("Admin");
            IdentityRole role1 = new IdentityRole("Member");
            IdentityRole role2 = new IdentityRole("SuperAdmin");
            await _roleManager.CreateAsync(role);
            await _roleManager.CreateAsync(role1);

            await _roleManager.CreateAsync(role2);
            return Ok();




        }
    }
}
