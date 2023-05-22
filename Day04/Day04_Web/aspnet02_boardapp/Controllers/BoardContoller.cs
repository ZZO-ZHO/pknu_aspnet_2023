using aspnet02_boardapp.Data;
using aspnet02_boardapp.Models;
using Microsoft.AspNetCore.Mvc;

namespace aspnet02_boardapp.Controllers
{
    // https://localhost:7066/Board/Index
    public class BoardController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BoardController(ApplicationDbContext db)
        {
            _db = db;   // 알아서 DB연결
        }

        public IActionResult Index()    // 게시판 최초연결
        {
            IEnumerable<Board> objNoteList = _db.Boards.ToList();
            return View(objNoteList);
        }

        // https://localhost:7066/Create

        [HttpGet]
        public IActionResult Create()   // 게시판 글쓰기
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Board board)
        {
            _db.Boards.Add(board);
            _db.SaveChanges();

            return RedirectToAction("Index","Board");
        }
    }
}
