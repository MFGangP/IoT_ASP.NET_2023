using ASPNET02_WebApp.Data;
using ASPNET02_WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

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
        // startCount = 1, 11, 21, 31, 41
        // endCount = 10, 20, 30, 40, 50
        public IActionResult Index(int page = 1) // 게시판 최초화면 리스트
        {
            // EntityFramework 로 작업해도 되고 SQL 쿼리로 작업해도 된다
            //IEnumerable<Board> objBoardList = _db.Boards.ToList(); // SELECT * 쿼리
            //var objBoardList = _db.Boards.FromSql($"SELECT * FROM Boards").ToList();
            var totalCount = _db.Boards.Count();
            var pageSize = 10; // 게시판 한 페이지 10개씩 들어가는 리스트
            var totalPage = totalCount / pageSize; // 전체 페이지 / 페이지 사이즈
            if (totalCount % pageSize > 0) 
            {
                totalPage++; // 나머지 페이지가 있으면 페이지 수가 늘어나야함.
            }
            // 맨 첫 페이지, 맨 마지막 페이지
            var countPage = 10;
            var startPage = ((page - 1) / countPage) * countPage + 1;
            var endPage = startPage + countPage - 1;
            if (totalPage < endPage) 
            { 
                endPage = totalPage; 
            }
            int startCount = ((page - 1) * countPage) + 1;
            int endCount = startCount + (pageSize - 1);
            // HTML 화면에서 사용하기 위해서 선언 = ViewData, TempData 동일한 역할
            ViewBag.StartPage = startPage;
            ViewBag.EndPage = endPage;
            ViewBag.Page = page;
            ViewBag.TotalPage = totalPage;

            var StartCount = new MySqlParameter("startCount", startCount);
            var EndCount = new MySqlParameter("endCount", endCount);

            var objBoardList = _db.Boards.FromSql($"CALL New_PageingBoard({StartCount}, {EndCount})").ToList();

            return View(objBoardList);
        }
        // https://lovalhost:7102/Board/Create
        // GetMethod로 화면을 URL로 부를 때 사용하는 액션
        public IActionResult Create() // 게시판 글쓰기
        {
            return View();
        }
        // 같은 이름을 쓰는데 다른 일을 한다. POST
        // Submit이 발생해서 내부로 데이터를 전달 할 액션
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Board board) 
        {

            try
            {
                board.PostDate = DateTime.Now; // 현재 저장하는 일시를 할당
                                               // Board 값을 받아서 
                _db.Boards.Add(board); // INSERT
                _db.SaveChanges(); // COMMIT

                TempData["succeed"] = "새 게시글이 저장되었습니다."; // 성공 메세지
            }
            catch (Exception)
            {
                TempData["error"] = "게시글 작성 오류 발생";
            }
            // 다시 처음 값을 부름
            return RedirectToAction("Index", "Board"); // localhost/Board/Index 화면으로 이동해라
        }

        [HttpGet]
        public IActionResult Edit(int? Id) 
        { 
            if (Id == null || Id == 0 ) 
            { 
                return NotFound(); // Error.cshtml이 표시
            }

            var board = _db.Boards.Find(Id); // SELECT * FROM board WHERE Id = @id

            if (board == null)
            {
                return NotFound();
            }

            return View(board);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Board board)
        {
            board.PostDate = DateTime.Now; // 날짜를 다시 현재 저장하는 일시를 할당
            // 업데이트 쿼리문 UPDATE Query
            _db.Boards.Update(board);
            _db.SaveChanges(); // COMMIT

            TempData["succeed"] = "새 게시글이 수정되었습니다."; // 성공 메세지


            // 앞에 값이 액션, 뒤에 값이 목적지
            return RedirectToAction("Index", "Board");
        }

        [HttpGet]
        public IActionResult Delete(int? Id)
        {
            // HttpGet Edit Action의 로직과 완전 동일
            if (Id == null || Id == 0)
            {
                return NotFound(); // Error.cshtml이 표시
            }

            var board = _db.Boards.Find(Id); // SELECT * FROM board WHERE Id = @id

            if (board == null)
            {
                return NotFound();
            }

            return View(board);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? Id)
        {
            var board = _db.Boards.Find(Id);

            if(board == null)
            {
                return NotFound();
            }

            // 삭제 쿼리문 Delete Query
            _db.Boards.Remove(board);
            _db.SaveChanges(); // COMMIT

            TempData["succeed"] = "게시글이 삭제되었습니다."; // 성공 메세지

            // 앞에 값이 액션, 뒤에 값이 목적지
            return RedirectToAction("Index", "Board");
        }

        [HttpGet]
        public IActionResult Details(int? Id) 
        {
            // HttpGet Edit Action의 로직과 완전 동일
            if (Id == null || Id == 0)
            {
                return NotFound(); // Error.cshtml이 표시
            }

            var board = _db.Boards.Find(Id); // SELECT * FROM board WHERE Id = @id

            if (board == null)
            {
                return NotFound();
            }

            board.ReadCount++; // 조회수 1증가
            _db.Boards.Update(board); // UPDATE 쿼리 실행
            _db.SaveChanges(); // COMMIT

            return View(board);
        }
    }
}
