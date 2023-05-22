using ASPNET02_WebApp.Data;
using ASPNET02_WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET02_WebApp.Controllers
{
    //https://localhost:7800/Board/Index
    public class BoardController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BoardController(ApplicationDbContext db)
        {
            _db = db; // 알아서 DB가 연결
        }

        public IActionResult Index() // 게시판 최초화면 리스트
        {
            IEnumerable<Board> objBoardList = _db.Boards.ToList(); // SELECT 쿼리
            return View(objBoardList);
        }
        // https://lovalhost:7102/Board/Create
        // GetMethod로 화면을 URL로 부를 때 사용하는 액션
        public IActionResult Create() // 게시판 글쓰기
        {
            return View();
        }
        // 같은 이름을 쓰는데 다른 일을 한다.
        // Submit이 발생해서 내부로 데이터를 전달 할 액션
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Board board) 
        {
            // Board 값을 받아서 
            _db.Boards.Add(board); // INSERT
            _db.SaveChanges(); // COMMIT
            // 다시 처음 값을 부름
            return RedirectToAction("Index", "Board");
        }
    }
}
