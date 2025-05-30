using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MazraeatiBackOffice.Models;

namespace MazraeatiBackOffice.Controllers
{
    public class AccountController : Controller
    {
        public enum NotifyType
        {
            Success,
            Error
        }


        public AccountController()
        {
        }

        public IActionResult Login(string ReturnUrl = "")
        {
            if (HttpContext.User.Claims.Count() > 0)
                if (HttpContext.User.Claims.FirstOrDefault().Value != null)
                {
                    if (HttpContext.User.Claims.Where(c => c.Type.Contains("role")).Count() > 0)
                    {
                       return RedirectToAction("Index", "Home");
                    }
                }

            return View(new AccountModel());
        }
        public IActionResult AccessDenied(string ReturnUrl = "")
        {
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<ActionResult> Login(AccountModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
                {
                    return View(model);
                }

                if (model.UserName.ToLower().Trim() != "admin" || model.Password.ToLower().Trim() != "admin@123")
                {
                    model.ErrorMessage = "خطأ في اسم المستخدم او كلمة السر";
                    return View(model);
                }



                var claims = new List<Claim> { new Claim(ClaimTypes.Name, model.UserName, string.Format("{0}", model.UserName)), new Claim(ClaimTypes.Role, "admin") };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTime.Now.AddDays(30),
                    IsPersistent = true,
                    IssuedUtc = DateTimeOffset.Now,
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                return View(model);
            }
        }
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
