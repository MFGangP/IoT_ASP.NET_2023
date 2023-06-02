using ASPNET02_WebApp.Data;
using ASPNET03_portfolioWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASPNET03_portfolioWebApp.Controllers
{
    public class PortfolioController : Controller
    {
        public readonly ApplicationDbContext _db;

        // 파일 업로드용 웹 환경을 위한 객체가 필요함. (필수!)
        private readonly IWebHostEnvironment _environment;

        public PortfolioController(ApplicationDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ViewData["NoScroll"] = "true";
            var list = _db.Portfolios.ToList(); // SELECT *
            return View(list);
        }
        [HttpGet]
        public IActionResult Create()
        {

            // PortfolioModel을 선택하는게 아니라, TempPortfolioModel을 선택(O)
            return View();
        }

        [HttpPost]
        public IActionResult Create(TempPortfolioModel temp)
        {

            // 파일 업로드, Temp -> Model db저장
            if (ModelState.IsValid)
            {
                // 파일 업로드되면 새로운 이미지 파일명을 받음
                string upFileName = UploadImageFile(temp);

                // TempPortfolioModel -> PortfolioModel 변경
                var portfolio = new PortfolioModel()
                {
                    Division = temp.Division,
                    Title = temp.Title,
                    Description = temp.Description,
                    Url = temp.Url,
                    FileName = upFileName // 이게 핵심!
                };

                _db.Portfolios.Add(portfolio);
                _db.SaveChanges();

                TempData["succeed"] = "포트폴리오 저장완료!";
                return RedirectToAction("Index", "Portfolio");
            }
            return View(temp);
        }
        // 파일 업로드 하기위한 메서드 
        // Routing 이나 GET/POST랑 관계 없음.
        private string UploadImageFile(TempPortfolioModel temp)
        {
            var resultFileName = "";
            try
            {
                if (temp.PortfolioImage != null)
                {
                    // "portfolio01.png"를 올렸다고 가정했을 때 중복되면 큰일난다.
                    // _environment.WebRootPath 서버 위치 값
                    string uploadFolder = Path.Combine(_environment.WebRootPath, "uploads"); // wwwroot 밑에 uploads라는 폴더가 존재 해야된다
                    resultFileName = Guid.NewGuid() + "_" + temp.PortfolioImage.FileName; // 중복된 이미지 파일명 제거
                    string filePath = Path.Combine(uploadFolder, resultFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        temp.PortfolioImage.CopyTo(fileStream);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }

            return resultFileName;
        }
    }
}
