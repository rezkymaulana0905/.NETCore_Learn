using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTC.Data;
using PTC.Services;

namespace PTC.Controllers;

public class AddMobileUserController(PtcContext context) : Controller
{
    private readonly PtcContext db = context;

    public async Task<IActionResult> Index()
    {
        var level = HttpContext.Session.GetString("_Level");

        if (level != "0")
        {
            return new ContentResult()
            {
                Content = "Restirected"
            };
        }
        var model = await db.User.ToListAsync();

        var isLocked = await db.UserLock.Select(l => l.UserId).ToListAsync();

        ViewData["LockedId"] = isLocked;

        return View(model);
    }

    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var user = await db.User.Where(u => u.Id == id).FirstOrDefaultAsync() ?? throw new Exception("User Not Found");

            var userLock = await db.UserLock.Where(u => u.UserId == id).ToListAsync();

            if (userLock != null)
            {
                db.UserLock.RemoveRange(userLock);

                await db.SaveChangesAsync();

            }

            var userPassDump = await db.PasswordDump.Where(u => u.UserId == id).ToListAsync();

            if (userLock != null)
            {
                db.PasswordDump.RemoveRange(userPassDump);

                await db.SaveChangesAsync();
            }

            var userPassAttempt = await db.UserPasswordAttempt.Where(u => u.UserId == id).ToListAsync();

            if(userPassAttempt != null)
            {
                db.UserPasswordAttempt.RemoveRange(userPassAttempt);

                await db.SaveChangesAsync();
            }

            db.User.Remove(user);
    
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return new ContentResult() { Content = $"Something wrong: {ex.Message}" };
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Add(string username)
    {
        try
        {
            await UserServices.Register(db, username);
        }
        catch (Exception ex)
        {
            return new ContentResult() { Content = $"Something wrong: {ex.Message}" };
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Reset(int id)
    {
        try
        {
            await UserServices.Reset(db, id);
        }
        catch (Exception ex)
        {
            return new ContentResult() { Content = $"Something wrong: {ex.Message}" };
        }
        return RedirectToAction("Index");   
    }

    public async Task<IActionResult> Unlock(int id)
    {
        try
        {
            await UserServices.Unlocked(db, id);
        }
        catch (Exception ex)
        {
            return new ContentResult() { Content = $"Something wrong: {ex.Message}" };
        }
        return RedirectToAction("Index");
    }
}