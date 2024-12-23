using ASPCMVC08.Data;
using ASPCMVC08.Models;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ASPCMVC08.Controllers
{
    public class Admin : Controller
    {
        ApplicationDbContext context; // контекст бд , текущего подключения
        UserManager<IdentityUser> userManager; // api упарвления пользователями хранилища ( модель пользователя )
        IPasswordHasher<IdentityUser> passwordHasher; // Абстракция хеширования паролей пользователей
        RoleManager<IdentityRole> roleManager;
        SignInManager<IdentityUser> signInManager;

        public Admin(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IPasswordHasher<IdentityUser> passwordHasher, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.passwordHasher = passwordHasher;
            this.roleManager = roleManager;
        }

        [Authorize(Roles = "Administrator")] // метод требует аутентификации (токен)
        [HttpGet("Admin/Index")]
        public async Task<IActionResult> Index()
        {
            var users = userManager.Users.ToList();
            var userRolesViewModel = new List<UserRolesViewModel>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                userRolesViewModel.Add(new UserRolesViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Roles = roles
                });
            }

            ViewBag.CurrentUserName = User.Identity.Name;
            ViewBag.CurrentUserRoles = await userManager.GetRolesAsync(await userManager.GetUserAsync(User));

            return View(userRolesViewModel);
        }



        [AllowAnonymous] // доступ к методу без аутентификации
        [HttpGet]
        public IActionResult Register(string _controller = "Admin", string _action = "Register")
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(string username, string password,string _controller = "Home" , string _action = "Index")
        {
            var user = new IdentityUser(username);
            var result = await userManager.CreateAsync(user,password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "User");
                await signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction(_action, _controller);
            }

            foreach (var error in result.Errors)
            {
                return RedirectToAction("Error", new { message = error.Description });
            }

            return RedirectToAction(_action,_controller);
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string oldpassword, string newpassword, string _controller = "Home", string _action = "Index")
        {


            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Admin");
            }

            var changePasswordResult = await userManager.ChangePasswordAsync(user, oldpassword, newpassword);

            if (changePasswordResult.Succeeded)
            {
                return RedirectToAction(_action, _controller);
            }
            else
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    return RedirectToAction("Error", new { message = error.Description });
                }
            }

            return RedirectToAction(_action, _controller);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignIn(string _controller = "Admin", string _action = "SignIn")
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignIn(string username, string password, string _controller = "Home", string _action = "Index")
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return RedirectToAction("Error", new { message = "Нет имени и пароля" });
            }

            var user = await userManager.FindByNameAsync(username);
            if (user == null || !await userManager.CheckPasswordAsync(user, password))
            {
                return RedirectToAction("Error", new { message = "Неверные имя и пароль" });
            }

            await signInManager.SignInAsync(user, isPersistent: true);

            if (User.Identity?.IsAuthenticated == true)
            {
                Console.WriteLine($"Пользователь {user.UserName} успешно вошел.");
            }
            else
            {
                Console.WriteLine($"Ошибка: Пользователь {user.UserName} не авторизован.");
            }

            return RedirectToAction(_action, _controller);
        }

        [HttpGet]
        [Authorize]
        public IActionResult SignOut(string _controller = "Admin", string _action = "SignOut")
        {

            return View();
        }

        [Authorize]
        [HttpPost("/Admin/SignOut")]
        public async Task<IActionResult> ISignOut(string _controller = "Home", string _action = "Index")
        {
            string? userName = this.HttpContext.User.Identity?.Name;
            if (userName != null)
            {
                await this.signInManager.SignOutAsync();
            }
            return RedirectToAction(_action, _controller);
        }


        [HttpGet]
        public async Task<IActionResult> Unsubscribe()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            await userManager.DeleteAsync(user);
            await this.signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }


        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult CreateUser(string _controller = "Admin", string _action = "CreateUser")
        {
            if (!User.IsInRole("Administrator"))
            {
                return RedirectToAction("AccessError", "Admin");
            }
            ViewBag.ReturnController = _controller;
            ViewBag.ReturnAction = _action;
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> CreateUser(string userName, string password, string returnController = "Admin", string returnAction = "Index")
        {
            var user = new IdentityUser { UserName = userName };
            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                var roleResult = await userManager.AddToRoleAsync(user, "User");

                if (roleResult.Succeeded)
                {
                    return RedirectToAction(returnAction, returnController);
                }
                else
                {
                    if (roleResult.Errors.Any())
                    {
                        return RedirectToAction("Error", new { message = roleResult.Errors.First().Description });
                    }
                }
            }
            else
            {
                if (result.Errors.Any())
                {
                    return RedirectToAction("Error", new { message = result.Errors.First().Description });
                }
            }

            return RedirectToAction(returnAction, returnController);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult DeleteUser(string returnController = "Admin", string returnAction = "Index")
        {
            ViewBag.ReturnController = returnController;
            ViewBag.ReturnAction = returnAction;
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userName, string returnController = "Admin", string returnAction = "Index")
        {
            var user = await userManager.FindByNameAsync(userName);

            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction(returnAction, returnController);
                }
                else
                {
                    if (result.Errors.Count() > 0)
                    {
                        return RedirectToAction("Error", new { message = result.Errors.First().Description });
                    }
                    ViewBag.ReturnController = returnController;
                    ViewBag.ReturnAction = returnAction;
                    return View();
                }
            }
            else
            {
                ViewBag.ReturnController = returnController;
                ViewBag.ReturnAction = returnAction;
                return RedirectToAction("Error", new { message = "Пользователь не найден" });
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult Assign(string returnController = "Admin", string returnAction = "Index")
        {
            ViewBag.ReturnController = returnController;
            ViewBag.ReturnAction = returnAction;
            return View("Assign");
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Assign(string userName, string roleName, string returnController = "Admin", string returnAction = "Index")
        {
            var user = await userManager.FindByNameAsync(userName);
            var roleExists = await roleManager.RoleExistsAsync(roleName);

            if (user != null && roleExists)
            {
                var result = await userManager.AddToRoleAsync(user, roleName);

                if (result.Succeeded)
                {
                    return RedirectToAction(returnAction, returnController);
                }
                else
                {
                    if(result.Errors.Count() > 0)
                    {
                        return RedirectToAction("Error", new { message = result.Errors.First().Description });
                    }
                    ViewBag.ReturnController = returnController;
                    ViewBag.ReturnAction = returnAction;
                    return RedirectToAction("Error", new { message = "Роль/пользователь не найдены"});

                }
            }
            else
            {
                ViewBag.ReturnController = returnController;
                ViewBag.ReturnAction = returnAction;
                return RedirectToAction("Error", new { message = "Роль/пользователь не найдены" });

            }
        }


        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult CreateRole(string returnController = "Admin", string returnAction = "Index")
        {
            ViewBag.ReturnController = returnController;
            ViewBag.ReturnAction = returnAction;
            return View();
        }


        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName, string returnController = "Admin", string returnAction = "Index")
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));

                if (result.Succeeded)
                {
                    return RedirectToAction(returnAction, returnController);
                }
                else
                {
                    if (result.Errors.Count() > 0)
                    {
                        return RedirectToAction("Error", new { message = result.Errors.First().Description });
                    }
                    ViewBag.ReturnController = returnController;
                    ViewBag.ReturnAction = returnAction;
                    return RedirectToAction("Error", new { message = "Роль существует" });
                }
            }
            else
            {
                ViewBag.ReturnController = returnController;
                ViewBag.ReturnAction = returnAction;
                return RedirectToAction("Error", new { message = "Роль существует" });

            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult DeleteRole(string returnController = "Admin", string returnAction = "Index")
        {
            ViewBag.ReturnController = returnController;
            ViewBag.ReturnAction = returnAction;
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string roleName, string returnController = "Admin", string returnAction = "Index")
        {
            var role = await roleManager.FindByNameAsync(roleName);

            if (role != null)
            {
                var result = await roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction(returnAction, returnController);
                }
                else
                {
                    if (result.Errors.Count() > 0)
                    {
                        return RedirectToAction("Error", new { message = result.Errors.First().Description });
                    }
                    ViewBag.ReturnController = returnController;
                    ViewBag.ReturnAction = returnAction;
                    return View();
                }
            }
            else
            {
                ViewBag.ReturnController = returnController;
                ViewBag.ReturnAction = returnAction;
                return RedirectToAction("Error", new { message = "Роль не найдена" });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Error(string message, string returnController = "Home", string returnAction = "Index")
        {
            ViewBag.Message = message;
            ViewBag.ReturnController = returnController;
            ViewBag.ReturnAction = returnAction;
            return View();
        }


    }
}
