using System;
using System.Text;
using EBSApp.Models.Authentication;
using EBSApp.Services.General;
using EBSAuthenticationHandler.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EBSApp.Pages
{
    public class RegisterModel : PageModel
    {
        public string? ErrorMessage = null;
        private readonly IUserAuthService _authService;

        public RegisterModel(IUserAuthService authService)
        {
            _authService = authService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            UserLogin userLogin = new();
            userLogin.Email = Request.Form["email"];
            userLogin.Password = Request.Form["password"];

            byte[] psw = Encoding.UTF8.GetBytes(userLogin.Password);

            userLogin.Password = Convert.ToBase64String(psw);

            AuthenticateResult result = await _authService.RegisterClient(userLogin);

            if (!result.Succeeded)
            {
                ErrorMessage = result.Failure.Message;
                return Page();
            }

            await HttpContext.SignInAsync(result.Principal);
            return Redirect("./clientsetup");
        }
    }
}

