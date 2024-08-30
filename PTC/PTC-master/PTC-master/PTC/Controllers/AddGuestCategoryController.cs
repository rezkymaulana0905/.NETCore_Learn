using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTC.Data;
using PTC.Models;

namespace PTC.Controllers;

public class AddGuestCategoryController(PtcContext context) : Controller
{
    private readonly PtcContext db = context;

    public async Task<IActionResult> Index()
    {
        var level = HttpContext.Session.GetString("_Level");

        if (level != "1")
        {
            return new ContentResult()
            {
                Content = "Restirected"
            };
        }
        var model = await db.GuestCategory.ToListAsync();

        return View(model);
    }

    public async Task<IActionResult> Add(string categoryName)
    {
        try
        {
            db.GuestCategory.Add(new GuestCategory
            {
                Id = 0,
                CategoryName = categoryName
            });

            await db.SaveChangesAsync();

        } catch (Exception ex)
        {
            return new ContentResult() { Content = $"Something wrong: {ex.Message}" };

        }
        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var category = await db.GuestCategory.Where(c => c.Id == id).FirstOrDefaultAsync() ?? throw new Exception("Category Not Found");

            db.GuestCategory.Remove(category);

            await db.SaveChangesAsync();
        } catch (Exception ex)
        {
            return new ContentResult() { Content = $"Something wrong: {ex.Message}" };
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> EditPage(int id)
    {
        var level = HttpContext.Session.GetString("_Level");

        if (level != "2")
        {
            return new ContentResult()
            {
                Content = "Restirected"
            };
        }
        var model = await db.GuestCategory.ToListAsync();

        var edit = await db.GuestCategory.Where(c => c.Id == id).FirstOrDefaultAsync();

        ViewData["Category"] = edit;

        return View(model);
    }

    public async Task<IActionResult> Edit(int categoryId, string categoryName)
    {
        try
        {
            var category = await db.GuestCategory.Where(c => c.Id == categoryId).FirstOrDefaultAsync();

            category.CategoryName = categoryName;

            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return new ContentResult() { Content = $"Something wrong: {ex.Message}" };
        }

        return RedirectToAction("Index");
    }
}

