using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTC.Data;
using PTC.Models;
using PTC.Utils;
namespace PTC.Controllers;

public class ShowWorkerPlanController(PtcContext context) : Controller
{
    private readonly PtcContext db = context;
    private readonly DateOnly today = DateOnly.FromDateTime(DateTime.Now);
    public async Task<IActionResult> Index(string searchQuery, string startDate, string endDate, int? pageNumber)
    {
        ViewData["searchQuery"] = searchQuery;
        ViewData["startDate"] = startDate ?? today.ToString();
        ViewData["endDate"] = endDate ?? today.ToString();

        var level = HttpContext.Session.GetString("_Level");


        if (searchQuery != null)
        {
            pageNumber = 1;
        }

        var contractor = db.WrkPermDesc
            .Join(db.WrkPermWorker,
            desc => desc.RegNum,
            worker => worker.RegNum,
            (desc, worker) => new
            {
                Desc = desc,
                Worker = worker
            }).Select(j => new WorkPermitModel
            {
                Desc = j.Desc,
                Worker = j.Worker,
            });

        if (level == "3" || level == "4")
        {
            var emp = HttpContext.Session.GetString("_Id");
            var dept = await db.DdedmEmployees
                .Where(e => e.EmpId == emp)
                .Select(e => e.Department)
                .SingleOrDefaultAsync();

         
            contractor = contractor.Where(c => c.Desc.Location.ToLower().Contains(emp.ToLower()));
        }

        if (startDate != null && endDate != null) {
            var selectedStartDate = DateOnly.Parse(startDate);
            var selectedEndDate = DateOnly.Parse(endDate);
            contractor = contractor.Where(c => c.Desc.Start <= selectedEndDate && c.Desc.End >= selectedStartDate);
        } else { contractor = contractor.Where(c => c.Desc.Start <= today && c.Desc.End >= today); }

         contractor = !string.IsNullOrEmpty(searchQuery)
            ? contractor
                .Where(c => c.Worker.Name.ToLower().Contains(searchQuery.ToLower()) ||
                c.Desc.CompName.ToLower().Contains(searchQuery.ToLower()) ||
                c.Desc.Location.ToLower().Contains(searchQuery.ToLower()))
            : contractor;

        return View(await Pagination<WorkPermitModel>.CreateAsync(contractor, pageNumber ?? 1, 10));
    }

}

