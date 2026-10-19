using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Skarbnik.DB;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Skarbnik.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
 
        private readonly PasswordHasher<string> _hasher;
        private readonly AppDbContext _context;

        public LoginModel()
        {
         
            _hasher = new PasswordHasher<string>();
           
        }

        [BindProperty]
        [Required]
        public string Username { get; set; }

        [BindProperty]
        [Required]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
           

            //var result = _hasher.VerifyHashedPassword(
            //    Username,
            //    _settings.PasswordHash,
            //    Password);

            //if (result == PasswordVerificationResult.Failed)
            //{
            //    ErrorMessage = "B³êdne has³o";
            //    l.CountError++;
            //    if (l.CountError >= 3)
            //    {
            //        l.IsLocked = true;
            //        ErrorMessage = " Konto zablokowane. Skontaktuj siê z administratorem.";
            //    }
            //    await _context.SaveChangesAsync();
            //    return Page();
            //}
            //l.CountError = 0;
            //l.LastTimeLogin = DateTime.UtcNow;
            //await _context.SaveChangesAsync();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Username)
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
