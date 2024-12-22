using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace wonderr.Pages.Dashboard
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            const string adminUsername = "admin";
            const string adminPassword = "as8080";

            if (Username == adminUsername && Password == adminPassword)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, Username)
        };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Redirect to the return URL if valid, otherwise go to the Dashboard home
                return LocalRedirect(returnUrl ?? "/Dashboard/Index");
            }

            ErrorMessage = "Invalid username or password";
            return Page();
        }
    }
}
