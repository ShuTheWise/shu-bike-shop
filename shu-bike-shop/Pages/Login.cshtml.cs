using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace shu_bike_shop.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IUserService userService;

        public LoginModel(IUserService userService)
        {
            this.userService = userService;
        }

        public string ReturnUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string paramUsername, string paramPassword)
        {
            string returnUrl = Url.Content("~/");
            try
            {
                // Clear the existing external cookie
                await HttpContext
                    .SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch {

            }

            DataAccessLibrary.Models.UserModel userModel = null;
            try
            {
                userModel = await userService.Login(new UserLoginModel()
                {
                    EmailAddress = paramUsername,
                    EncryptedPassword = paramPassword
                });
            }
            catch
            {
            }

            if (userModel == null)
            {
                return LocalRedirect(returnUrl);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, paramUsername),
                new Claim(ClaimTypes.Role, userModel.Role.ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                RedirectUri = this.Request.Host.Value
            };

            try
            {
                await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return LocalRedirect(returnUrl);
        }
    }
}