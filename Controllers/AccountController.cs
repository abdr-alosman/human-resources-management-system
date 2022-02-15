using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using NewKaratIk.Data;
using NewKaratIk.Dtos;
using NewKaratIk.Extentsions;
using NewKaratIk.Helper;
using NewKaratIk.Models;

namespace NewKaratIk.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IEmailSender _emailSender;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender, ApplicationDbContext db)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;

        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new User
            {
                
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                UserName = model.Email

            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                //Generate Token
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail", "Account", new
                {
                    userId = user.Id,
                    token = code
                });

                //Email
                await _emailSender.SendEmailAsync(model.Email, "Hesabınızı Onaylayınız", $"Lütfen email hesabınızı onaylamak için <a href='https://ik.karatguc.com{url}' target='_blank'>Tıklayınız</a>");
                TempData.Put("message", new AlertMessage()
                {
                    Message = "Hesabınızı aktivleştirmek için E-posta adresinize gönderilen linki tıklayınız lütfen",
                    AlertType = "success"
                });

                return RedirectToAction(nameof(Login));
            }
            else
            {
                result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
            }


            return View(model);
        }

        public IActionResult Login(string? ReturnUrl = null)
        {
            return View(new LoginModel()
            {
                ReturnUrl = ReturnUrl
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "bu Kullanıcı Emaili ile daha önce hesap oluşturulmamıştır !");
                return View(model);
            }
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", " Lütfen email hesabınza gelen link ile üyeliğinizi onaylayınız. ");
                return View(model);
            }
            if (user.Status == false)
            {

                ModelState.AddModelError("", " Hesbınız Kilitli !");
            }
            else
            {
                CustomPasswordHasher cm = new CustomPasswordHasher();
                string pass = cm.HashPassword(user, model.Password);
                var result = await _signInManager.PasswordSignInAsync(user, pass, true, false);

                if (result.Succeeded)
                {
                    return Redirect(model.ReturnUrl ?? "~/");
                }
                ModelState.AddModelError("", "Girilen Kullanıcı Adı Veya Parola Yanlıştır !");
            }


            return View(model);
        }
        public async Task<IActionResult> Logout(RegisterModel model)
        {
            await _signInManager.SignOutAsync();
            TempData.Put("message", new AlertMessage()
            {
                Message = "Hesabınız Güvenli Bir şekilde Kapatıldı.",
                AlertType = "success"
            });
            return Redirect("~/");
        }

        public async Task<IActionResult> ConfirmEmail(string UserId, string token)
        {
            if (UserId == null || token == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(UserId);

            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        Message = "E-posta adresiniz onaylandı, Hesabınız Admin tarafından onaylandıktan sonra giriş yapabilirsinz",
                        AlertType = "success"
                    });
                    return RedirectToAction(nameof(Login));
                }
            }
            TempData.Put("message", new AlertMessage()
            {
                Message = "Hesabınız Onaylanmadı",
                AlertType = "warning"
            });

            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                return View();
            }
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                TempData.Put("message", new AlertMessage()
                {
                    Message = "bu eposta adrese ait bir kullanıcı bulunamadı, Lütfen Emaili kontrol ediniz.",
                    AlertType = "danger"
                });
                return View();
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var url = Url.Action("ResetPassword", "Account", new
            {
                userId = user.Id,
                token = code
            });

            //Email
            await _emailSender.SendEmailAsync(Email, "Reset Password", $"Parolanızı Yenilemek için linki <a href='https://ik.karatguc.com{url}' target='_blank'>Tıklayınız</a>");
            TempData.Put("message", new AlertMessage()
            {
                Message = "eposta adresinize şifre güncelleme linki gönderildi ,Lütfen mailinizi kontrol edin .",
                AlertType = "success"
            });
            return View();
        }

        public IActionResult ResetPassword(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return NotFound();
            }
            var model = new ResetPasswordModel { Token = token };
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.Password != model.RePassword)
            {
                ModelState.AddModelError("", "Girilen şifreler aynı değil !");
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                TempData.Put("message", new AlertMessage()
                {
                    Message = "Şifreniz Başarı ile Güncellenmiştir, yeni şifrenizle giriş yapabilirsiniz",
                    AlertType = "success"
                });


                return RedirectToAction("Login", "Account");
            }
            return View(model);

        }
        [Authorize]
        public IActionResult Accessdenied()
        {
            return View();
        }
    }
}
