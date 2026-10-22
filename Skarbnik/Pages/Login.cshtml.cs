using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Skarbnik.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Skarbnik.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
 
        private readonly PasswordHasher<string> _hasher;

        public LoginModel()
        {
         
            _hasher = new PasswordHasher<string>();
           
        }

       

        [BindProperty]
        [Required]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
           

 string password="sk"+DateTime.UtcNow.ToWarsawTime().ToString("dd");           
#if DEBUG
            Console.WriteLine("tryb dewoleprski");
#else


             if (Password != password)
            {
                ErrorMessage = "Błędne hasło";
                return Page();
            }
#endif
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Administrator")
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            return RedirectToPage("/Index");
        }
    }
}
