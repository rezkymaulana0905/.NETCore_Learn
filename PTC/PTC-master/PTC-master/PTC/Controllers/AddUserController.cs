using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTC.Data;
using PTC.Models;

namespace PTC.Controllers
{
    public class AddUserController(PtcContext context) : Controller
    {
        private readonly PtcContext db = context;
        public async Task<IActionResult> Index(string empId, string searchQuery)
        {
            var level = HttpContext.Session.GetString("_Level");

            if (level != "0")
            {
                return new ContentResult()
                {
                    Content = "Restirected"
                };
            }

            if (empId != null)
            {
                ViewData["userEdit"] = await db.SsoUsers.Where(e=> e.EmpId == empId).SingleOrDefaultAsync();
            }

            var model = await db.SsoUsers.ToListAsync();

            if (searchQuery != null)
            {
                model = model.Where(m => m.Username.ToLower().Contains(searchQuery.ToLower()) || m.EmpId.ToLower().Contains(searchQuery.ToLower())).ToList();
            }

            ViewData["Role"] = await db.UserRole.OrderByDescending(r => r.Id).ToListAsync();


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([Bind("EmpId", "Username", "RoleId")] SsoUser user)
        {
            try
            {
                db.SsoUsers.Add(user);
                await db.SaveChangesAsync();
            } catch (Exception ex)
            {
                return new ContentResult()
                {
                    Content = ex.Message
                };
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string empId)
        {
            try
            {
                var user = await db.SsoUsers.Where(u => u.EmpId == empId).SingleOrDefaultAsync() ?? throw new Exception("User Not Found");
                db.SsoUsers.Remove(user);

                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ContentResult()
                {
                    Content = ex.Message
                };
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit([Bind("EmpId", "Username", "RoleId")] SsoUser user)
        {
            db.SsoUsers.Update(user);

            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
