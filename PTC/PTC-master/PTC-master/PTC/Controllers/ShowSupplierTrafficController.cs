using Microsoft.AspNetCore.Mvc;
using PTC.Data;
using PTC.Models;
using PTC.Utils;

namespace PTC.Controllers
{
    public class ShowSupplierTrafficController(PtcContext context) : Controller
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

            var supplier = db.VhcSpplier.AsQueryable();

            if (endDate != null && startDate != null)
            {
                var selectedStartDate = DateOnly.Parse(startDate);
                var selectedEndDate = DateOnly.Parse(endDate); 
                supplier = supplier.Where(s => DateOnly.FromDateTime(s.InTime) <= selectedEndDate && DateOnly.FromDateTime(s.InTime) >= selectedStartDate);
            }
            else { supplier = supplier.Where(s => DateOnly.FromDateTime(s.InTime) == today); }

            supplier = !string.IsNullOrEmpty(searchQuery)
               ? supplier
                   .Where(s => s.VehicleId.ToLower().Contains(searchQuery.ToLower()) ||
                   s.Company.ToLower().Contains(searchQuery.ToLower()) ||
                   s.VehicleType.ToLower().Contains(searchQuery.ToLower()))
               : supplier;

            return View(await Pagination<VhcSpplier>.CreateAsync(supplier, pageNumber ?? 1, 10));
        }
    }
}
