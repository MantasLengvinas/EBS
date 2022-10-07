using System;
using System.Security.Claims;
using EBSApp.Models.Authentication;
using EBSAuthenticationHandler.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EBSApp.Pages
{
    public class LoginModel : PageModel
    {
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

            AuthenticateResult result = await _authService.LoginUser(userLogin);

            if (result.Succeeded)
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, result.Ticket.Principal);

            return Page();
        }
    }
}

