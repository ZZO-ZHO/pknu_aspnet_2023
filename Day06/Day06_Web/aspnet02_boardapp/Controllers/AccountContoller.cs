using aspnet02_boardapp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace aspnet02_boardapp.Controllers
{
    // 사용자 회원가입, 로그인, 로그아웃
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            // 생성자 마법사로 만드세요.
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        // https://localhost:7066/Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        // public IActionResult Register(RegisterModel model)
        // 비동기가 아니면 return 값은 IActionResult
        // 비동기가 되면 Task<IActionResult>
        public async Task<IActionResult> Register(RegisterModel model)
        {
            ModelState.Remove("PhoneNumber");   // PhoneNumber값은 입력값검증에서 제거
            if (ModelState.IsValid)  // 데이터를 제대로 입력해서 검증 성공하면
            {
                // ASP.NET user - aspnetusers 테이블에 데이터 넣기 위해서
                // 매핑되는 인스턴스를 생성
                var user = new IdentityUser()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };

                // aspnetusers 테이블에 사용자 데이터를 대입
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // 회원가입을 성공했으면 로그인한 뒤 localhost:7066/Home/Index로 가라
                    await signInManager.SignInAsync(user, isPersistent: false);
                    TempData["succeed"] = "로그인 성공했습니다.";
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);  // 회원가입을 실패하면 그 화면 그대로 유지
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // 파라미터 순서 : 아이디, 패스워드, isPersistent = RememberMe, 실패할때 계정잠그기
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if(result.Succeeded)
                {
                    TempData["succeed"] = "로그인 성공했습니다.";
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "로그인 실패!");
            }
            return View(model); // 입력검증이 실패하면 화면에 그대로 대기
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            TempData["succeed"] = "로그아웃 성공했습니다.";
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}