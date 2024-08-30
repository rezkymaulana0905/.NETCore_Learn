using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTC.Data;
using PTC.Models;

namespace PTC.Controllers
{
    public class EmployeeIn(PtcContext context) : Controller
    {
        private readonly PtcContext db = context;

        public async Task<IActionResult> Index(string searchQuery)
        {
            ViewBag.currentFilter = searchQuery;
            var id = HttpContext.Session.GetString("_Id");
            var level = HttpContext.Session.GetString("_Level");

            if (level == "4")
            {
                return new ContentResult() { Content = "Unauthorized" };
            }

            if (string.IsNullOrEmpty(id))
            {
                // Handle missing session ID case
                return new ContentResult() { Content = "Session ID is missing" };
            }

            // Query to get the pending employees
            var empQuery = db.TrxTmsEmpPcDtKpsFltAct
                .Where(e => !e.Flag && (e.TimereasonId == "KP" || e.TimereasonId == "DL"));

            if (level == "3")
            {
                var department = await db.DdedmEmployees
                    .Where(e => e.EmpId == id)
                    .Select(e => e.Department)
                    .SingleOrDefaultAsync();

                if (string.IsNullOrEmpty(department))
                {
                    // Handle missing department case
                    return new ContentResult() { Content = "Department not found for the employee" };
                }

                empQuery = empQuery.Where(e => e.Department == department);
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                empQuery = empQuery.Where(e => e.Name.Contains(searchQuery));
            }

            var empPendingIn = await empQuery.ToListAsync();

            // Get empPendingOut which does not contain records from empPendingIn
            var empPendingOutQuery = db.TrxTmsEmpPcDtKpsFlt.Where(e => e.Date == DateOnly.FromDateTime(DateTime.Now) && e.Flag == false && 
            (e.TimereasonId == "KP" || e.TimereasonId == "DL" || e.TimereasonId == "PC")).AsQueryable();

            if (level == "3")
            {
                var department = await db.DdedmEmployees
                    .Where(e => e.EmpId == id)
                    .Select(e => e.Department)
                    .SingleOrDefaultAsync();

                if (!string.IsNullOrEmpty(department))
                {
                    empPendingOutQuery = empPendingOutQuery.Where(e => e.Department == department);
                }
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                empPendingOutQuery = empPendingOutQuery.Where(e => e.Name.Contains(searchQuery));
            }

            var empPendingOut = await empPendingOutQuery
                .Where(e => !empPendingIn.Select(empIn => empIn.SeqNo).Contains(e.SeqNo))
                .ToListAsync();



            // Create EmployeeTransaction objects
            var empTransactionsIn = empPendingIn.Select(emp => new EmployeeTransaction {
                Id = emp.Id,
                Date = emp.Date.ToString(),
                EmpId = emp.EmpId,
                Name = emp.Name,
                TimeReasonId = emp.TimereasonId,
                TimeOut = emp.TimeOut.ToString(),
                Reason = emp.Reason, 
                pending = "In" }).ToList();
            var empTransactionsOut = empPendingOut.Select(emp => new EmployeeTransaction {
                Id = 0,
                Date = emp.Date.ToString(),
                EmpId = emp.EmpId,
                Name = emp.Name,
                TimeReasonId = emp.TimereasonId,
                TimeOut = null,
                Reason = emp.Reason,
                pending = "Out"
            }).ToList();

            // Combine the lists
            var employeeTransactions = empTransactionsIn.Concat(empTransactionsOut).OrderBy(emp => emp.pending).ToList();

            return View(employeeTransactions);
        }

        [HttpPost]
        public async Task<IActionResult> SetIn(int id)
        {
            try
            {
                var emp = await db.TrxTmsEmpPcDtKpsFltAct
                    .SingleOrDefaultAsync(e => e.Id == id);

                if (emp == null)
                {
                    return new ContentResult() { Content = "Employee not found" };
                }

                emp.Flag = true;

                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            { 
                return new ContentResult() { Content = "Error in the process" };
            }
        }
    }

    public class EmployeeTransaction
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string EmpId { get; set; }
        public string Name { get; set; }
        public string TimeReasonId { get; set; }
        public string TimeOut { get; set; }
        public string Reason {  get; set; }
        public string pending;
    }
}
