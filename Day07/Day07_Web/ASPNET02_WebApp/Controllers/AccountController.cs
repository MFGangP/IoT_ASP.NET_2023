using ASPNET02_WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ASPNET02_WebApp.Controllers
{
    // 사용자 회원가입, 로그인, 로그아웃

    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            // Null 검사추가 체크
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }
        // https://localhost:7125/Account/Register

        [HttpGet]
        public IActionResult Register()
        {
            return View(); // 화면 출력
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // public IActionResult Register(RegisterModel model)
        // 비동기가 아니면 return 값은 IActionResult
        // 비동기가 되면 Task<IActionResult>
        public async Task<IActionResult> Register(RegisterModel model)
        {
            ModelState.Remove("PhoneNember");// 휴대전화 번호는 입력값 검증에서 제거

            if (ModelState.IsValid) // 데이터 제대로 입력했는지 검증, 성공하면
            {
                // ASP.NET user - aspnetusers 테이블에 데이터 넣기 위해서
                // 매핑되는 인스턴스를 생성
                var user = new IdentityUser { 
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber // 휴대폰 번호 추가
                };

                // aspnetusers 테이블에 사용자 데이터를 대입
                var result = await _userManager.CreateAsync(user, model.Password);           

                if (result.Succeeded)
                {
                    // 로그인
                    // 회원 가입을 성공했으면 로그인 한 뒤 localhost:7125/Home/Index로 가라
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    TempData["succeed"] = "회원가입 성공했습니다";
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model); // 회원가입을 실패하면  그 화면 그대로 유지
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
            if(ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe ,false);
                // 파라미터 순서 : 아이디, 패스워드, IsPersistent = RememberMe, 실패 할 때 계정 잠그기
                if(result.Succeeded) 
                {
                    // TODO : 
                    TempData["succeed"] = "로그인 했습니다";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("","로그인 실패");
                }

            }
            return View(model); // 입력검증이 실패하면 화면에 그대로 대기
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            // TODO : 로그아웃 후 토스트 메시지 띄우기
            TempData["succeed"] = "로그아웃 했습니다";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string userName)
        {
            Debug.WriteLine(userName);

            var curUser = await _userManager.FindByNameAsync(userName);

            if (curUser == null)
            {
                TempData["error"] = "사용자가 없습니다.";
                return RedirectToAction("Index", "Home");
            }

            var model = new RegisterModel()
            {
                Email = curUser.Email,
                PhoneNumber = curUser.PhoneNumber,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);

                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
                // 비밀번호 변경은 어렵다.ㅎ
                var result = await _userManager.UpdateAsync(user); 

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    TempData["succeed"] = "프로필 변경 성공했습니다";
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model); // 회원가입을 실패하면  그 화면 그대로 유지
        }
    }
}
