using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        //3 create controller for Category, if you want to access it in link , add Category/Index in local debug link
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            //return View();
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }

        public IActionResult Create() //every view created must match with controller name
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj) //get model Category from view
        {
            if (obj.Name == obj.DisplayOrder.ToString()) // =========================================try validation
            {
                ModelState.AddModelError("name", "The Name cannot exactly same with Display Order");
            }
            //if (obj.Name != null && obj.Name == "test")
            //{
            //    ModelState.AddModelError("", "Test is invalid");
            //}
            if (ModelState.IsValid) //validation depends on model category rules / sesuai dengan ketentuan di model kategori atau tidak dijaga disini
            {
                //if (obj.Name != "")
                //{
                //    obj.Name = "ok";
                //}
                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Category created successfully"; //where we can display key "success", check on index /codesuccess
                return RedirectToAction("Index", "Category"); //redirect to controller Category with Action Index
            }
            return View();
            
        }
        public IActionResult Edit(int? id) //every view created must match with controller name
        {
            if (id==null || id == 0) //validation id / primary key
            {
                return NotFound();
            }
            //Category categoryFromDb = _db.Categories.Find(id); /*way 1*/

            Category? categoryFromDb = _db.Categories.Find(id);
            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u>=u.Id==id); // if you want edit data depends on primary key
            ////Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u >= u.Name.contains()); // if you want edit data depends on any field not primary
            //Category? categoryFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj) //get model Category from view
        {
            if (ModelState.IsValid) //validation depends on model category rules / sesuai dengan ketentuan di model kategori atau tidak dijaga disini
            {
                //if (obj.Name != "")
                //{
                //    obj.Name = "ok";
                //}
                _db.Categories.Update(obj); //update
                _db.SaveChanges();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index", "Category"); //redirect to controller Category with Action Index
            }
            return View();

        }

        public IActionResult Delete(int? id) //every view created must match with controller name
        {
            if (id == null || id == 0) //validation id / primary key
            {
                return NotFound();
            }
            //Category categoryFromDb = _db.Categories.Find(id); /*way 1*/

            Category? categoryFromDb = _db.Categories.Find(id);
            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u>=u.Id==id); // if you want edit data depends on primary key
            ////Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u >= u.Name.contains()); // if you want edit data depends on any field not primary
            //Category? categoryFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")] //reference to up this
        public IActionResult DeletePOST(int? id) //why post delete have function name different between get and post, because the parameter is same that cause function name cant be match or same
        {
            Category? obj = _db.Categories.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(obj); //update
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index","Category"); //redirect to controller Category with Action Index

        }
    }
}
