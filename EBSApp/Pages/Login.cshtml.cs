using System;
using System.Security.Claims;
using EBSApp.Models.Authentication;
using EBSAuthenticationHandler.Helpers;
using EBSAuthenticationHandler.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EBSApp.Pages
{
    public class LoginModel : PageModel
    {
        public string? ErrorMessage = null;
        private readonly IUserAuthService _authService;

        public LoginModel(IUserAuthService authService)
        {
            _authService = authService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            UserLogin userLogin = new();
            userLogin.Email = Request.Form["email"];
            userLogin.Password = Request.Form["password"];

            userLogin.Password = PasswordHelper.EncodePassword(userLogin.Password);

            AuthenticateResult result = await _authService.LoginUser(userLogin);

            if (!result.Succeeded)
            {
                ErrorMessage = result.Failure.Message;
                return Page();
            }

            await HttpContext.SignInAsync(result.Principal);
            if (userLogin.Email == "admin@ebs.lt")
                return Redirect("./users");
            else
                return Redirect("./");
        }
    }
}

