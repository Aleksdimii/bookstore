using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace bookstore.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser>
    _userManager;
    private readonly SignInManager<IdentityUser>
        _signInManager;
        private readonly RoleManager<IdentityRole>
            _roleManager;

            public RegisterModel(
            UserManager<IdentityUser>
                userManager,
                SignInManager<IdentityUser>
                    signInManager,
                    RoleManager<IdentityRole>
                        roleManager)
                        {
                        _userManager = userManager;
                        _signInManager = signInManager;
                        _roleManager = roleManager;
                        }

                        [BindProperty]
                        public InputModel Input { get; set; } = null!;

                        public string ReturnUrl { get; set; } = string.Empty;

                        public class InputModel
                        {
                        [Required]
                        [EmailAddress]
                        public string Email { get; set; } = null!;

                        [Required]
                        [DataType(DataType.Password)]
                        public string Password { get; set; } = null!;

                        [DataType(DataType.Password)]
                        [Compare("Password")]
                        public string ConfirmPassword { get; set; } = null!;
                        }

                        public void OnGet(string? returnUrl = null)
                        {
                        ReturnUrl = returnUrl ?? Url.Content("~/");
                        }

                        public async Task<IActionResult>
                            OnPostAsync(string? returnUrl = null)
                            {
                            returnUrl ??= Url.Content("~/");
                            if (ModelState.IsValid)
                            {
                            var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                            var result = await _userManager.CreateAsync(user, Input.Password);
                            if (result.Succeeded)
                            {
                            if (!await _roleManager.RoleExistsAsync("Customer"))
                            await _roleManager.CreateAsync(new IdentityRole("Customer"));

                            await _userManager.AddToRoleAsync(user, "Customer");

                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                            }

                            foreach (var error in result.Errors)
                            ModelState.AddModelError(string.Empty, error.Description);
                            }

                            return Page();
                            }
                            }
                            }
