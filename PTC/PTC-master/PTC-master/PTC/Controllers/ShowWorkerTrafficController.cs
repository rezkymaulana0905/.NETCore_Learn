using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTC.Data;
using PTC.Models;
using PTC.Utils;

namespace PTC.Controllers
{
    public class ShowWorkerTrafficController(PtcContext context) : Controller
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

            var contractor = db.WrkPermAttndnc
                .Join(db.WrkPermWorker,
                    attndnc => attndnc.WorkerId,
                    worker => worker.Id,
                    (attndnc, worker) => new
                    {
                        attndnc,
                        worker
                    }
                ).Join(db.WrkPermDesc,
                    attndnc => attndnc.attndnc.RegNum,
                    desc => desc.RegNum,
                    (attndnc, desc) => new
                    {
                        attndnc.worker,
                        attndnc.attndnc,
                        desc
                    }).Select(c => new ShowWrkPermAttndnc { 
                        Date = c.attndnc.Date,
                        Name = c.worker.Name,
                        Company = c.desc.CompName,
                        Location = c.desc.Location,
                        Description = c.desc.Title,
                        InTime = c.attndnc.InTime,
                        OutTime = c.attndnc.OutTime,
                    });

            if (level == "3" || level == "4")
            {
                var emp = HttpContext.Session.GetString("_Id");
                var dept = await db.DdedmEmployees
                    .Where(e => e.EmpId == emp)
                    .Select(e => e.Department)
                    .SingleOrDefaultAsync();


                contractor = contractor.Where(c => c.Location.ToLower().Contains(emp.ToLower()));
            }

            if (startDate != null && endDate != null)
            {
                var selectedStartDate = DateOnly.Parse(startDate);
                var selectedEndDate = DateOnly.Parse(endDate);
                contractor = contractor.Where(c => c.Date >= selectedStartDate && c.Date <= selectedEndDate);
            }
            else { contractor = contractor.Where(c => c.Date == today); }

            contractor = !string.IsNullOrEmpty(searchQuery)
               ? contractor
                   .Where(c => c.Name.ToLower().Contains(searchQuery.ToLower()) ||
                   c.Company.ToLower().Contains(searchQuery.ToLower()) ||
                   c.Location.ToLower().Contains(searchQuery.ToLower()))
               : contractor;

            return View(await Pagination<ShowWrkPermAttndnc>.CreateAsync(contractor, pageNumber ?? 1, 10));

        }
    }
}
