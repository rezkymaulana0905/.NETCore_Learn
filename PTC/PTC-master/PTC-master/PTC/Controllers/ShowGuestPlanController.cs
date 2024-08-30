using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTC.Data;
using PTC.Utils;
namespace PTC.Controllers;

public class ShowGuestPlanController(PtcContext context) : Controller
{
    private readonly PtcContext db = context;
    private readonly DateOnly today = DateOnly.FromDateTime(DateTime.Now);
    public async Task<IActionResult> Index(string searchQuery, string startDate, string endDate, int? pageNumber)
    {
        ViewData["searchQuery"] = searchQuery;
        ViewData["startDate"] = startDate ?? today.ToString();
        ViewData["endDate"] = endDate ?? today.ToString();

        var level = HttpContext.Session.GetString("_Level");

        var guests = db.RegGuest.Select(s => new ShowGuestModel
        {
            Id = s.Id,
            StartDate = s.StartDate,
            EndDate = s.EndDate,
            Name = s.Name,
            Company = s.Company,
            Category = db.GuestCategory.Where(c => c.Id == s.CategoryId).Select(c => c.CategoryName).FirstOrDefault(),
            Country = db.Country.Where(c => c.Code == s.CountryCode).Select(c => c.Name).FirstOrDefault(),
            CompanyType = s.CompanyType,
            DeptSect = s.DeptSect,
            NationalId = s.NationalId,
            Total = s.Total,
            ImageData = s.ImageData,
            Requirement = s.Requirement,
            Requester = db.DdedmEmployees.Where(e => e.EmpId == s.ReqId).Select(e => e.Name).FirstOrDefault() ?? s.ReqId,
            Met = db.DdedmEmployees.Where(e => e.EmpId == s.MetId).Select(e => e.Name).FirstOrDefault() ?? s.MetId,
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

        if (startDate != null && endDate != null) {
            var selectedStartDate = DateOnly.Parse(startDate);
            var selectedEndDate = DateOnly.Parse(endDate);
            guests = guests.Where(g => g.StartDate <= selectedEndDate && g.EndDate >= selectedStartDate);
        }
        else { guests = guests.Where(g => g.StartDate <= today && g.EndDate >= today); }

        guests = !string.IsNullOrEmpty(searchQuery)
            ? guests.Where(g => g.Name.ToLower().Contains(searchQuery.ToLower()) || g.Company.ToLower().Contains(searchQuery.ToLower()))
            : guests;
        
        return View(await Pagination<ShowGuestModel>.CreateAsync(guests, pageNumber ?? 1, 6));
    }
}

public class ShowGuestModel
{
    public string Id { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string Country { get; set; }
    public string Company { get; set; }
    public string CompanyType { get; set; }
    public string DeptSect { get; set; }
    public string NationalId { get; set; }
    public int Total { get; set; }
    public byte[] ImageData { get; set; }
    public string Requirement { get; set; }
    public string Requester { get; set; }
    public string Met { get; set; }
}

