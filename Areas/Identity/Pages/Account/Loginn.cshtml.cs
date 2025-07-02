using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

public class LoginModel : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IEmailSender _emailSender;

    public LoginModel(SignInManager<IdentityUser> signInManager, IEmailSender emailSender)
    {
        _signInManager = signInManager;
        _emailSender = emailSender;
    }

    [BindProperty]
    public string Email { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var result = await _signInManager.PasswordSignInAsync(Email, Password, false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            await _emailSender.SendEmailAsync(Email, "Login Notification", "You have successfully logged in.");

            return RedirectToPage("/Index");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
    }
}
