using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTC.Data;
using PTC.Utils;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PTC.Controllers;

public class ShowGuestTrafficController(PtcContext context) : Controller
{
    private readonly PtcContext db = context;
    private readonly DateOnly today = DateOnly.FromDateTime(DateTime.Now);
    public async Task<IActionResult> Index(string searchQuery, string startDate, string endDate, int? pageNumber)
    {
        ViewData["searchQuery"] = searchQuery;
        ViewData["startDate"] = startDate ?? today.ToString();
        ViewData["endDate"] = endDate ?? today.ToString();

        var level = HttpContext.Session.GetString("_Level");

        var guests = db.GuestAttndnc.Join(
                db.RegGuest,
                attndnc => attndnc.TransactionId,
                reg => reg.Id,
                (attndnc, req) => new
                {
                    attndnc,
                    req
                }
            ).Select(g => new ShowGuestAttndnc
            {
                Id = g.attndnc.Id,
                Name = g.req.Name,
                Company = g.req.Company,
                Date = g.attndnc.Date,
                TimeIn = g.attndnc.TimeIn,
                TimeOut = g.attndnc.TimeOut,
                People = g.attndnc.Total,
                Requirement = g.req.Requirement,
                Requester = db.DdedmEmployees.Where(e => e.EmpId == g.req.ReqId).Select(e => e.Name).FirstOrDefault() ?? g.req.ReqId,
                Met = db.DdedmEmployees.Where(e => e.EmpId == g.req.MetId).Select(e => e.Name).FirstOrDefault() ?? g.req.MetId,
            }).AsQueryable();

        if (level == "3" || level == "4")
        {
            var emp = HttpContext.Session.GetString("_Id");
            var dept = await db.DdedmEmployees
                .Where(e => e.EmpId == emp)
                .Select(e => e.Department)
                .SingleOrDefaultAsync();

            var empIdsInDept = await db.DdedmEmployees
                .Where(e => e.Department == dept)
                .Select(e => e.EmpId)
                .ToListAsync();

            guests = guests.Where(g => empIdsInDept.Contains(g.Met));
        }

        if (searchQuery != null)
        {
            pageNumber = 1;
        }

        if (startDate != null && endDate != null)
        {
            var selectedStartDate = DateOnly.Parse(startDate);
            var selectedEndDate = DateOnly.Parse(endDate);
            guests = guests.Where(g => g.Date >= selectedStartDate && g.Date <= selectedEndDate);
        }
        else { guests = guests.Where(g => g.Date == today); }

        guests = !string.IsNullOrEmpty(searchQuery)
            ? guests.Where(g => g.Name.ToLower().Contains(searchQuery.ToLower()) || g.Company.ToLower().Contains(searchQuery.ToLower()))
            : guests;

        return View(await Pagination<ShowGuestAttndnc>.CreateAsync(guests, pageNumber ?? 1, 6));
    }

    public async Task<IActionResult> EditGuestTraffic(int id)
    {
        var model = await db.GuestAttndnc.Join(
                db.RegGuest,
                attndnc => attndnc.TransactionId,
                reg => reg.Id,
                (attndnc, req) => new
                {
                    attndnc,
                    req
                }
            ).Select(g => new ShowGuestAttndnc
            {
                Id = g.attndnc.Id,
                Name = g.req.Name,
                Company = g.req.Company,
                Date = g.attndnc.Date,
                TimeIn = g.attndnc.TimeIn,
                TimeOut = g.attndnc.TimeOut,
                People = g.attndnc.Total,
                Requirement = g.req.Requirement,
                Requester = db.DdedmEmployees.Where(e => e.EmpId == g.req.ReqId).Select(e => e.Name).FirstOrDefault() ?? g.req.ReqId,
                Met = db.DdedmEmployees.Where(e => e.EmpId == g.req.MetId).Select(e => e.Name).FirstOrDefault() ?? g.req.MetId,
            }).Where(g => g.Id == id).SingleOrDefaultAsync();

        return View(model);
    }

    public async Task<IActionResult> Update([Bind("Id", "Name", "Company", "Date", "TimeIn", "TimeOut", "People", "Requirement", "Requester", "Met")] ShowGuestAttndnc guest)
    {
        try
        {
            var attndnc = await db.GuestAttndnc.Where(g => g.Id == guest.Id).FirstOrDefaultAsync();

            if (attndnc != null)
            {
                attndnc.Total = guest.People;

                await db.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            return new ContentResult() { Content = $"Something wrong: {ex.Message}" };
        }

        return RedirectToAction("Index");
    }



}
public class ShowGuestAttndnc
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Company { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly TimeIn { get; set; }
    public TimeOnly? TimeOut { get; set; }
    public int People { get; set; }
    public string Requirement { get; set; }
    public string Requester { get; set; }
    public string Met { get; set; }
}
