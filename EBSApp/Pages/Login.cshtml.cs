using System;
using EBSApp.Models.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EBSApp.Pages
{
    public class LoginModel : PageModel
    {
        public IActionResult OnPost()
        {
            UserLogin userLogin = new();
            userLogin.Email = Request.Form["email"];
            userLogin.Password = Request.Form["password"];



            return Page();
        }
    }
}

