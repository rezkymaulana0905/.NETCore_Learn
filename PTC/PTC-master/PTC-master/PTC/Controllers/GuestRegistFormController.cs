using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTC.Data;
using PTC.Models;
using PTC.Services;
using PTC.Utils;
namespace PTC.Controllers;

public class GuestRegistFormController(IEmailSender emailSender, PtcContext context) : Controller
{
    private readonly PtcContext db = context;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task<IActionResult> RegularForm(string MetId)
    {
        ViewData["Country"] = await db.Country.ToListAsync();

        ViewData["Category"] = await db.GuestCategory.ToListAsync();

        ViewData["Met"] = await db.DdedmEmployees.Where(e => e.EmpId == MetId).Select(e => e.Name).FirstOrDefaultAsync();
        ViewData["MetId"] = MetId;

        return View();
    }
    public async Task<IActionResult> VipForm(string MetId)
    {
        ViewData["Country"] = await db.Country.ToListAsync();

        ViewData["Category"] = await db.GuestCategory.ToListAsync();

        ViewData["Met"] = await db.DdedmEmployees.Where(e => e.EmpId == MetId).Select(e => e.Name).FirstOrDefaultAsync();
        ViewData["MetId"] = MetId;

        return View();
    }
    public async Task<IActionResult> EditGuestForm(string id)
    {
        ViewData["Country"] = await db.Country.OrderBy(c => c.Name).ToListAsync();
        ViewData["Category"] = await db.GuestCategory.ToListAsync();

        var model = await db.RegGuest.Where(g => g.Id == id).SingleOrDefaultAsync();

        ViewData["Met"] = await db.DdedmEmployees.Where(e => e.EmpId == model.MetId).Select(e => e.Name).FirstOrDefaultAsync();

        return View(model);
    }

    public async Task<IActionResult> Delete(string id)
    {
        var attndnc = await db.GuestAttndnc.Where(g => g.TransactionId == id).FirstOrDefaultAsync() ?? null;

        if(attndnc != null)
        {
            return new ContentResult() { Content = $"Something wrong: cant delete used plan" };
        }
        var model = await db.RegGuest.Where(g => g.Id == id).SingleOrDefaultAsync();

        if (model == null)
        {
            return NotFound();
        }

        db.RegGuest.Remove(model);
        await db.SaveChangesAsync();

        return RedirectToAction("index", "ShowGuestPlan");
    }

    [HttpPost]
    public async Task<IActionResult> Update(
        [Bind("Id", "Name", "Email", "Company", "DeptSect", "NationalId", "Total", "Requirement", 
        "ReqId", "MetId", "ImageData", "StartDate", "EndDate")] GuestReg guest, 
        IFormFile Image, string CompanyType, string SelectCountry, int SelectCategory)
    {
        try
        {
            if (guest == null)
            {
                return new ContentResult() { Content = "Guest data is null" };
            }

            // Handle CompanyType
            guest.CompanyType = CompanyType == "true" ? "Panasonic" : "Non Panasonic";
            guest.CountryCode = SelectCountry;
            guest.CategoryId = SelectCategory;

            // Handle Image upload
            if (Image != null && Image.Length > 0)
            {
                await SaveImage(guest, Image);
            } else
            {
                guest.ImageData = await db.RegGuest.Where(g => g.Id == guest.Id).Select(g => g.ImageData).SingleOrDefaultAsync();
            }

            db.RegGuest.Update(guest);

            //var met = await EmployeeService.GetEmployeeByEmpId(existingGuest.MetId, db);
            //var req = await EmployeeService.GetEmployeeByEmpId(existingGuest.ReqId, db);

            //await SendMail(guest, met, req);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in the update process: {ex}");
            return new ContentResult() { Content = $"Error in the update process: {ex.Message}" };
        } finally
        {
            string filePath = $"wwwroot/qr/{guest.Id}.png";
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        // Save changes to the database
        await db.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Regist([Bind("Id", "Name", "Email", "Company", "DeptSect", "NationalId", "Total", 
        "Requirement", "ReqId", "MetId", "ImageData", "StartDate", "EndDate")] GuestReg guest, 
        IFormFile Image, string CompanyType, string SelectCountry, int SelectCategory)
    {
        try
        {
            await SetGuestProperties(guest);
            guest.CountryCode = SelectCountry ?? throw new ArgumentNullException(nameof(SelectCountry));
            guest.CategoryId = SelectCategory;
            guest.CompanyType = CompanyType == "true" ? "Panasonic" : "Non Panasonic";


            if (Image != null && Image.Length > 0)
            {
                await SaveImage(guest, Image);
            }

            var met = await EmployeeService.GetEmployeeByEmpId(guest.MetId, db);
            var req = await EmployeeService.GetEmployeeByEmpId(guest.ReqId, db);

            await SendMail(guest, met, req);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in the registration process: {ex}");
            return new ContentResult() { Content = $"Error in the registration process: {ex}" };
        } finally
        {
            string filePath = $"wwwroot/qr/{guest.Id}.png";
            if (System.IO.File.Exists(filePath))
               {
                System.IO.File.Delete(filePath);
            }

        }

        db.RegGuest.Add(guest);
        await db.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }


    private async Task SetGuestProperties(GuestReg guest)
    {
    // Retrieve ReqId from HttpContext.Session
    guest.ReqId = HttpContext.Session.GetString("_Id") ?? "";

    var today = DateTime.Today.ToString("yyyyMMdd");

    // Check if there are any entries in GuestIdTemp that are not for today's date
    var oldEntries = await db.GuestIdTemp.Where(t => DateOnly.FromDateTime(t.Date) != DateOnly.FromDateTime(DateTime.Now)).ToListAsync();

    // If there are old entries, remove them and save changes
    if (oldEntries.Count != 0)
    {
       db.GuestIdTemp.RemoveRange(oldEntries);
       await db.SaveChangesAsync();
    }

    var entry = await db.GuestIdTemp.OrderBy(t => t.Id).LastOrDefaultAsync();

    // Add a new entry to GuestIdTemp for today
    var newEntry = new GuestIdTemp();

        if(entry != null)
        {
            newEntry.Id = entry.Id + 1;
        } else
        {
            newEntry.Id = 1;
        }

    db.GuestIdTemp.Add(newEntry);
    await db.SaveChangesAsync();

    // Get the latest entry (assuming Id is auto-incremented or sequenced)
    var latestTempId = await db.GuestIdTemp.OrderBy(t => t.Id).LastOrDefaultAsync();

    // Determine guest.Id based on whether there's a lastId from RegGuest
    var lastId = await db.RegGuest
        .Where(g => g.CreatedAt.Date == DateTime.Today)
        .OrderByDescending(g => g.Id)
        .Select(g => g.Id)
        .FirstOrDefaultAsync();

    if (lastId == null)
    {
        guest.Id = $"G{today}001";
    }
    else
    {
        guest.Id = $"G{today}{latestTempId.Id:D3}";
    }
}

    private static async Task SaveImage(GuestReg guest, IFormFile Image)
    {
        using var memoryStream = new MemoryStream();
        await Image.CopyToAsync(memoryStream);
        guest.ImageData = memoryStream.ToArray();
    }

    private async Task SendMail(GuestReg guest, DdedmEmployee met, DdedmEmployee req)
    {
        var mailController = new MailController(_emailSender, db);
        await mailController.Index(guest, met, req);
    }
}