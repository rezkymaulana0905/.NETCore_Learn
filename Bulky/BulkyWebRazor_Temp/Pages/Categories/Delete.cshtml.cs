using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        //[BindProperty] //to bring property/input components into OnPost function add *just one property
        public Category Category { get; set; }
        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet(int? id) //get id to edit , Adding ? after a reference type allows the variable to hold a null value. This is useful in scenarios where null is a valid value for a variable, such as optional fields in a model.
        {
            if (id != 0 && id != null)
            {
                Category = _db.Categories.Find(id);
            }
        }

        public IActionResult OnPost()
        {
            Category? obj = _db.Categories.Find(Category.Id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(obj); //update
            _db.SaveChanges();
            //TempData["success"] = "Category created successfully"; //where we can display key "success", check on index /codesuccess
            return RedirectToPage("Index");
        }
    }
}
