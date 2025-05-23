using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_Laptop.Models;
using Shopping_Laptop.Models.ViewModels;
using System.Threading.Tasks;
using Shopping_Laptop.Repository;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Shopping_Laptop.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUserModel> _userManage;
        private SignInManager<AppUserModel> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly DataContext _dataContext;
        public AccountController(IEmailSender emailSender, UserManager<AppUserModel> userManage,
            SignInManager<AppUserModel> signInManager, DataContext context)
        {
            _userManage = userManage;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _dataContext = context;

        }
        public IActionResult Login(string returnUrl)
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UpdateAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Challenge();
            }

            var user = await _userManage.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["error"] = "User not found.";
                return RedirectToAction("Index", "Home");
            }

            var viewModel = new UpdateAccountViewModel
            {
                UserId = user.Id,
                UserName = user?.UserName ?? string.Empty,
                Email = user?.Email ?? string.Empty,
                Occupation = user?.Occupation,
                Gender = user?.Gender,
                DateOfBirth = user?.DateOfBirth,
                HeightCm = user?.HeightCm,
                WeightKg = user?.WeightKg,
                ShoeSize = user?.ShoeSize
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAccount(UpdateAccountViewModel model)
        {
            if (string.IsNullOrEmpty(model.UserId))
            {
                return BadRequest("User ID is missing.");
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (model.UserId != currentUserId)
            {
                TempData["error"] = "Unauthorized action.";
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                var user = await _userManage.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    user.Occupation = model.Occupation ?? string.Empty;
                    user.Gender = model.Gender;
                    user.DateOfBirth = model.DateOfBirth;
                    user.HeightCm = model.HeightCm;
                    user.WeightKg = model.WeightKg;
                    user.ShoeSize = model.ShoeSize;

                    var result = await _userManage.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        TempData["success"] = "Thông tin tài khoản đã được cập nhật thành công!";
                        return RedirectToAction("UpdateAccount");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    TempData["error"] = "User not found.";
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SendMailForgotPass(AppUserModel user)
        {
            var checkMail = await _userManage.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (checkMail == null)
            {
                TempData["error"] = "Email not found";
                return RedirectToAction("ForgotPass", "Account");
            }
            else
            {
                string token = Guid.NewGuid().ToString();
                //update token to user
                checkMail.Token = token;
                _dataContext.Update(checkMail);
                await _dataContext.SaveChangesAsync();
                var receiver = checkMail.Email;
                var subject = "Change password for user " + checkMail.Email;
                var message = "Click on link to change password " +
                    "<a href='" + $"{Request.Scheme}://{Request.Host}/Account/NewPass?email=" + checkMail.Email + "&token=" + token + "'>";

                await _emailSender.SendEmailAsync(receiver!, subject, message);
            }


            TempData["success"] = "An email has been sent to your registered email address with password reset instructions.";
            return RedirectToAction("ForgotPass", "Account");
        }
        public IActionResult ForgotPass()
        {
            return View();
        }
        public async Task<IActionResult> NewPass(AppUserModel user, string token)
        {
            var checkuser = await _userManage.Users
                .Where(u => u.Email == user.Email)
                .Where(u => u.Token == user.Token).FirstOrDefaultAsync();

            if (checkuser != null)
            {
                ViewBag.Email = checkuser.Email;
                ViewBag.Token = token;
                return View(checkuser);
            }
            else
            {
                TempData["error"] = "Email not found or token is not right";
                return RedirectToAction("ForgotPass", "Account");
            }
        }
        public async Task<IActionResult> UpdateNewPassword(AppUserModel user, string token)
        {
            var checkuser = await _userManage.Users
                .Where(u => u.Email == user.Email)
                .Where(u => u.Token == user.Token).FirstOrDefaultAsync();

            if (checkuser != null)
            {
                //update user with new password and token
                string newtoken = Guid.NewGuid().ToString();
                // Hash the new password
                var newPassword = Request.Form["Password"].ToString();
                if (string.IsNullOrEmpty(newPassword))
                {
                    TempData["error"] = "Password cannot be empty.";
                    return RedirectToAction("NewPass", "Account", new { email = user.Email, token = user.Token });
                }
                var passwordHasher = new PasswordHasher<AppUserModel>();
                var passwordHash = passwordHasher.HashPassword(checkuser, newPassword);

                checkuser.PasswordHash = passwordHash;
                checkuser.Token = newtoken;

                await _userManage.UpdateAsync(checkuser);
                TempData["success"] = "Password updated successfully.";
                return RedirectToAction("Login", "Account");
            }
            else
            {
                TempData["error"] = "Email not found or token is not right";
                return RedirectToAction("ForgotPass", "Account");
            }
        }
        public async Task<IActionResult> History()
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                // User is not logged in, redirect to login
                return RedirectToAction("Login", "Account"); // Replace "Account" with your controller name
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var Orders = await _dataContext.Orders
                .Where(od => od.UserName == userEmail).OrderByDescending(od => od.Id).ToListAsync();
            ViewBag.UserEmail = userEmail;
            return View(Orders);
        }

        public async Task<IActionResult> CancelOrder(string ordercode)
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                // User is not logged in, redirect to login
                return RedirectToAction("Login", "Account");
            }
            try
            {
                var order = await _dataContext.Orders.Where(o => o.OrderCode == ordercode).FirstAsync();
                order.Status = 3;
                _dataContext.Update(order);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                return BadRequest("An error occurred while canceling the order.");
            }


            return RedirectToAction("History", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginVM.UserName, loginVM.Password, false, false);
                if (result.Succeeded)
                {
                    var user = await _userManage.FindByNameAsync(loginVM.UserName);
                    if (user == null)
                    {
                        return RedirectToAction("Login");
                    }
                    var roles = await _userManage.GetRolesAsync(user);


                    if (roles.Contains("Admin"))
                    {
                        return RedirectToAction("Index", "Admin"); // hoặc tên controller admin
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home"); // người dùng thường
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            return View(loginVM);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserModel user)
        {
            if (ModelState.IsValid)
            {
                AppUserModel newUser = new AppUserModel { UserName = user.UserName, Email = user.Email };
                IdentityResult result = await _userManage.CreateAsync(newUser, user.Password);
                if (result.Succeeded)
                {
                    TempData["success"] = "Tạo thành viên thành công";
                    return Redirect("/account/login");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }


        public async Task<IActionResult> Logout(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync();
            return Redirect(returnUrl);
        }



        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            var identity = result.Principal?.Identities.FirstOrDefault();
            if (identity == null)
            {
                return RedirectToAction("Login");
            }

            var claims = identity.Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });

            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                TempData["error"] = "Could not retrieve email from Google. Please try again.";
                return RedirectToAction("Login");
            }

            string emailName = email.Split('@')[0];
            var existingUser = await _userManage.FindByEmailAsync(email);

            if (existingUser == null)
            {
                var newUser = new AppUserModel { UserName = emailName, Email = email };
                var passwordHasher = new PasswordHasher<AppUserModel>();
                var hashedPassword = passwordHasher.HashPassword(newUser, "Acb@123456789");
                newUser.PasswordHash = hashedPassword;

                var createUserResult = await _userManage.CreateAsync(newUser);
                if (!createUserResult.Succeeded)
                {
                    TempData["error"] = "Đăng ký tài khoản thất bại. Vui lòng thử lại sau.";
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    await _signInManager.SignInAsync(newUser, isPersistent: false);
                    TempData["success"] = "Đăng ký tài khoản thành công.";
                }
            }
            else
            {
                await _signInManager.SignInAsync(existingUser, isPersistent: false);
            }

            // Gửi email khi đăng nhập thành công
            var subject = "Thông báo đăng nhập thành công";
            var message = $@"
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            padding: 20px;
        }}
        .container {{
            background-color: #ffffff;
            padding: 30px;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }}
        h2 {{
            color: #333333;
            font-size: 24px;
        }}
        .content {{
            margin-top: 20px;
            font-size: 16px;
            line-height: 1.5;
            color: #666;
        }}
        .footer {{
            margin-top: 30px;
            color: #888;
            font-size: 14px;
            text-align: center;
        }}
        .button {{
            background-color: #007bff;
            color: white;
            padding: 12px 30px;
            border-radius: 5px;
            text-decoration: none;
            font-size: 16px;
            display: inline-block;
            margin-top: 20px;
        }}
        .button:hover {{
            background-color: #0056b3;
        }}
        .signature {{
            margin-top: 30px;
            font-size: 16px;
            font-weight: bold;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <h2>Kính gửi {email},</h2>
        <p class='content'>Chúng tôi rất vui mừng thông báo rằng bạn đã đăng nhập thành công vào tài khoản của mình trên website của chúng tôi.</p>
        <p class='content'>Chúc mừng bạn đã gia nhập cộng đồng mua sắm trực tuyến của chúng tôi. Tại TCon, bạn có thể dễ dàng khám phá và lựa chọn các sản phẩm công nghệ hàng đầu như điện thoại di động, phụ kiện điện thoại, máy tính xách tay, và nhiều thiết bị điện tử cao cấp khác.</p>
        <p class='content'>Chúng tôi cam kết mang đến cho bạn những trải nghiệm mua sắm tiện lợi, an toàn, và nhanh chóng. Với đội ngũ hỗ trợ khách hàng tận tình, bạn sẽ luôn nhận được sự hỗ trợ kịp thời mỗi khi cần.</p>
        <p class='content'>Nếu bạn cần bất kỳ sự trợ giúp nào hoặc có thắc mắc về sản phẩm, vui lòng liên hệ với chúng tôi qua email hoặc số điện thoại hỗ trợ khách hàng.</p>
        <p class='content'>Cảm ơn bạn đã tin tưởng và lựa chọn TCon!</p>
        <a href='https://www.tcon.com' class='button'>Khám Phá Ngay</a>
        <div class='footer'>
            <p>Trân trọng,<br>Đội ngũ TCon – Cửa hàng điện thoại và phụ kiện công nghệ</p>
        </div>
    </div>
</body>
</html>
";

            await _emailSender.SendEmailAsync(email, subject, message);

            TempData["Success"] = "Đăng Nhập Thành Công";
            return RedirectToAction("Index", "Home");
        }


        [Authorize(Roles = "Admin,Company,Employee")]
        public IActionResult IndexAdmin(string returnUrl = "/Admin")
        {
            return Redirect(returnUrl);
        }

        public async Task LoginByGoogle()
        {
            // Use Google authentication scheme for challenge
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponse")
                });
        }
    }
}