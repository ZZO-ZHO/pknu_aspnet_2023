﻿using aspnet02_boardapp.Data;
using aspnet02_boardapp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace aspnet02_boardapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }
        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        [HttpGet]
        public IActionResult Index()
        {
            var model = _db.Portfolios.Where(p => p.Division == "PORTFOLIO").ToList();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}