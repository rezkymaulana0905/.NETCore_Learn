using Microsoft.AspNetCore.Mvc;
using BulkyWeb.Data;
using BulkyWeb.Models; 

namespace BulkyWeb.Controllers
{
    public class EmploymentController : Controller
    {
        private readonly ApplicationDbContext _dbEMP;
        public EmploymentController(ApplicationDbContext db)
        {
            _dbEMP = db;
        }
        public IActionResult Index()
        {
            List<Employment> empList = _dbEMP.Employees.ToList();
            return View(empList);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employment obj)
        {
            if (ModelState.IsValid) { 
                _dbEMP.Employees.Add(obj);
                _dbEMP.SaveChanges();
                TempData["success"] = "Employment Created Successfully";
                return RedirectToAction("Index","Employment");
            }
            return View();
        }
    }
}
