using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        //[BindProperty] //to bring property/input components into OnPost function add *just one property
        public Category Category { get; set; }
        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet(int? id) //get id to edit
        {
            if (id != 0 && id != null)
            {
                Category = _db.Categories.Find(id);
            }
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid) //validation depends on model category rules / sesuai dengan ketentuan di model kategori atau tidak dijaga disini
            {
                //if (obj.Name != "")
                //{
                //    obj.Name = "ok";
                //}
                _db.Categories.Update(Category);
                _db.SaveChanges();
                //TempData["success"] = "Category created successfully"; //where we can display key "success", check on index /codesuccess
                return RedirectToAction("Index", "Category"); //redirect to controller Category with Action Index
            }
            return RedirectToPage("Index");
        }
    }
}
