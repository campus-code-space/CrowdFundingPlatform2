// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using EndeKisse2.Data;
using EndeKissie2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EndeKisse2.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IWebHostEnvironment webHostEnvironment,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Fayda Id Number")]
            public string FaydaIdNum { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Residence")]
            public string Residence { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Role")]
            public string role { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Gender")]
            public string Gender { get; set; }

            [Required]
            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            [Required]
            [DataType(DataType.DateTime)]
            [Display(Name = "Date Of Birth")]
            public DateTime DOB { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Gender")]
            public string LinckedinAcc { get; set; }

            [Required]
            [DataType(DataType.Upload)]
            [Display(Name = "User Photo")]
            [RegularExpression(@".*\.(jpg|jpeg|png|gif|bmp|tiff|webp|JPG|PNG|WEBP|GIF|BMP|JXL|AVIF|TIFF)$",
            ErrorMessage = "Invalid File Type. Please upload one of: jpg, jpeg, png, gif, bmp, tiff, webp.")]
            public IFormFile UserPhoto { get; set; }

            [Required]
            [DataType(DataType.Upload)]
            [Display(Name = "ID Photo")]
            [RegularExpression(@".*\.(jpg|jpeg|png|gif|bmp|tiff|webp|JPG|PNG|WEBP|GIF|BMP|JXL|AVIF|TIFF)$",
            ErrorMessage = "Invalid File Type. Please upload one of: jpg, jpeg, png, gif, bmp, tiff, webp.")]
            public IFormFile IdPhoto { get; set; }

            
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }


        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                user.FaydaIdNum = Input.FaydaIdNum;
                user.Gender = Input.Gender;
                user.DOB = Input.DOB;
                user.role = Input.role;
                user.Residence = Input.Residence;
                user.PhoneNumber = Input.PhoneNumber;
                user.LinkedinAcc = Input.LinckedinAcc;

                if (Input.UserPhoto != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Input.UserPhoto.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Input.UserPhoto.CopyToAsync(fileStream);
                    }
                    user.UserImageUrl = $"~/images/{uniqueFileName}";
                }

                if (Input.IdPhoto != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Input.IdPhoto.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Input.IdPhoto.CopyToAsync(fileStream);
                    }
                    user.IdImageUrl = $"~/images/{uniqueFileName}";
                }


                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { userId = userId, code = code, returnUrl = returnUrl },
                    //    protocol: Request.Scheme);

                    var callbackUrl = $"https://localhost:7018/Identity/Account/ConfirmEmail?userId={userId}&code={Uri.EscapeDataString(code)}&returnUrl={returnUrl}";


                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{callbackUrl}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }

        //public async Task<bool> UploadImage(IFormFile imageFile, string userId)
        //{
        //    if (imageFile == null || imageFile.Length == 0)
        //    {
        //        return false;
        //    }

        //    // Define the folder where images will be stored
        //    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
        //    Directory.CreateDirectory(uploadPath); // Ensure directory exists

        //    // Generate unique file name
        //    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
        //    var filePath = Path.Combine(uploadPath, fileName);

        //    // Save file locally
        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await imageFile.CopyToAsync(stream);
        //    }

        //    // Save image metadata to database
        //    var imageStore = new ImageStore
        //    {
        //        UserId = userId,
        //        ImageUrl = $"/images/{fileName}" // Relative path to the image
        //    };

        //    _context.ImageStore.Add(imageStore);
        //    await _context.SaveChangesAsync();

        //    return true;
        //}
    }
}
