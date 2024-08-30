using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTC.Data;
using PTC.Models;
using PTC.Utils;
using System.Diagnostics.Contracts;

namespace PTC.Controllers
{
    public class ShowEmpTrafficController(PtcContext context) : Controller
    {
        private readonly PtcContext db = context;
        private readonly DateOnly today = DateOnly.FromDateTime(DateTime.Now);

        public async Task<IActionResult> Index(string searchQuery, string startDate, string endDate, int? pageNumber)
        {
            ViewData["searchQuery"] = searchQuery;
            ViewData["startDate"] = startDate ?? today.ToString();
            ViewData["endDate"] = endDate ?? today.ToString();

            var level = HttpContext.Session.GetString("_Level");

            var emp = db.TrxTmsEmpPcDtKpsFltAct.AsQueryable(); ;

            if (searchQuery != null)
            {
                pageNumber = 1;
            }

            if (level == "3" || level == "4")
            {
                var user = HttpContext.Session.GetString("_Id");
                var dept = await db.DdedmEmployees
                    .Where(e => e.EmpId == user)
                    .Select(e => e.Department)
                    .SingleOrDefaultAsync();

                emp = emp.Where(e => e.Department == dept);
            }

            if (startDate != null && endDate != null)
            {
                var selectedStartDate = DateOnly.Parse(startDate);
                var selectedEndDate = DateOnly.Parse(endDate);
                emp = emp.Where(e => e.Date >= selectedStartDate && e.Date <= selectedEndDate);
            }
            else
            {
                emp = emp.Where(e => e.Date == today);
            }

            emp = !string.IsNullOrEmpty(searchQuery)
                ? emp
                    .Where(e => e.Name.ToLower().Contains(searchQuery.ToLower())
                    || e.Reason.ToLower().Contains(searchQuery.ToLower())
                    || e.TimereasonId.ToLower().Contains(searchQuery.ToLower())
                    || e.TimeOut.ToString().Contains(searchQuery.ToLower())
                    || e.TimeReturn.ToString().Contains(searchQuery.ToLower())
            )
            : emp;


            return View(await Pagination<TrxTmsEmpPcDtKpFltAct>.CreateAsync(emp, pageNumber ?? 1, 10));
        }
    }
}
