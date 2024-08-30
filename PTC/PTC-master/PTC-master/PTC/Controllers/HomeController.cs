using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PTC.Data;
using PTC.Models;
using PTC.Services;
using System.Globalization;

namespace PTC.Controllers
{
    public class HomeController(PtcContext context) : Controller
    {
        private readonly PtcContext db = context;
        readonly static DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        readonly static DateOnly yesterday = today.AddDays(-1);
        readonly static int month = today.Month;
        public string empUpdate;
        public string guestUpdate;
        public string suppUpdate;
        public string conUpdate;

        public async Task<IActionResult> Index(string filterSummary, string filterLog, string searchQuery)
        {
            try
            {
                var counts = await GetCounts(filterSummary);
                var total = counts.Values.Sum();
                var countDataPoints = GetCountDataPointsBar(counts, total);

                var level = HttpContext.Session.GetString("_Level") ?? "4";

                List<Log>? model;

                if (level == "3" || level == "4")
                {
                    model = await GetDeptModel(filterLog);
                }
                else
                {
                    model = await GetModel(filterLog);
                }

                if(!searchQuery.IsNullOrEmpty())
                {
                    model = model.Where(m => m.Name.ToLower().Contains(searchQuery.ToLower()) ||
                    m.Type.ToLower().Contains(searchQuery.ToLower()) ||
                    m.Detail.ToLower().Contains(searchQuery.ToLower()) ).ToList();
                }


                ViewBag.dataPoints = JsonConvert.SerializeObject(countDataPoints);
                ViewBag.total = total;
                ViewBag.sum = filterSummary ?? "today";
                ViewBag.log = filterLog ?? "plan";
                ViewBag.search = searchQuery;
                ViewBag.empCount = await CountEmployees(level);
                ViewBag.guestCount = await CountGuests(level);
                ViewBag.suppCount = await CountSuppliers();
                ViewBag.conCount = await CountContractors(level);
                ViewBag.empUpdate = empUpdate;
                ViewBag.guestUpdate = guestUpdate;
                ViewBag.suppUpdate = suppUpdate;
                ViewBag.conUpdate = conUpdate;

                return View(model);
            }
            catch (Exception ex)
            {
                return new ContentResult() { Content = $"Something wrong: {ex.Message}" };
            }   
        }
        private async Task<Dictionary<string, int>> GetCounts(string filterSummary)
        {
            var counts = new Dictionary<string, int> { { "emp", 0 }, { "guest", 0 }, { "guest-panasonic", 0 }, { "supplier", 0 }, { "contractor", 0 } };

            var level = HttpContext.Session.GetString("_Level");

            var empQuery = db.TrxTmsEmpPcDtKpsFltAct.AsQueryable();
            var guestQuery = db.GuestAttndnc
                    .Join(db.RegGuest,
                     attndnc => attndnc.TransactionId,
                     reg => reg.Id,
                     (attndnc, reg) => new { attndnc, reg })
                    .AsQueryable();
            var supplierQuery = db.VhcSpplier.AsQueryable();
            var contractorQuery = db.WrkPermAttndnc
                    .Join(db.WrkPermDesc,
                        attndnc => attndnc.RegNum,
                        desc => desc.RegNum,
                        (attndnc, desc) => new
                        {
                            attndnc,
                            desc
                        })
                    .AsQueryable();

            if (level == "3" || level == "4")
            {
                var empId = HttpContext.Session.GetString("_Id");
                var dept = await db.DdedmEmployees.Where(e => e.EmpId == empId).Select(e => e.Department).SingleOrDefaultAsync();

                var empIdsInDept = await db.DdedmEmployees
                    .Where(e => e.Department == dept)
                    .Select(e => e.EmpId)
                    .ToListAsync();

                empQuery = empQuery.Where(e => e.Department == dept);
                guestQuery = guestQuery.Where(g => empIdsInDept.Contains(g.reg.MetId));
                contractorQuery = contractorQuery.Where(c => c.desc.Location.ToLower().Contains(dept.ToLower()));
            }

            switch (filterSummary)
            {
                case "today":
                    counts["emp-out"] = await empQuery.CountAsync(e => e.Date == today);
                    counts["emp-in"] = await empQuery.CountAsync(e => e.Date == today && e.Flag == true);
                    counts["guest-in"] = await guestQuery.CountAsync(g => g.attndnc.Date == today && g.reg.CompanyType == "Non Panasonic");
                    counts["guest-out"] = await guestQuery.CountAsync(g => g.attndnc.Date == today && g.reg.CompanyType == "Non Panasonic" && g.attndnc.Flag == true);
                    counts["guest-panasonic-in"] = await guestQuery.CountAsync(g => g.attndnc.Date == today && g.reg.CompanyType == "Panasonic");
                    counts["guest-panasonic-out"] = await guestQuery.CountAsync(g => g.attndnc.Date == today && g.reg.CompanyType == "Panasonic" && g.attndnc.Flag == true);
                    counts["supplier-in"] = await supplierQuery.CountAsync(v => DateOnly.FromDateTime(v.InTime) == today);
                    counts["supplier-out"] = await supplierQuery.CountAsync(v => DateOnly.FromDateTime(v.InTime) == today && v.Flag == true);
                    counts["contractor-in"] = await contractorQuery.CountAsync(c => c.attndnc.Date == today);
                    counts["contractor-out"] = await contractorQuery.CountAsync(c => c.attndnc.Date == today && c.attndnc.Flag == true);
                    break;
                case "last-7-day":
                    var last7Days = today.AddDays(-7);
                    counts["emp-out"] = await empQuery.CountAsync(e => e.Date <= today && e.Date >= last7Days);
                    counts["emp-in"] = await empQuery.CountAsync(e => e.Date <= today && e.Date >= last7Days && e.Flag == true);
                    counts["guest-in"] = await guestQuery.CountAsync(g => g.attndnc.Date <= today && g.attndnc.Date >= last7Days && g.reg.CompanyType == "Non Panasonic");
                    counts["guest-out"] = await guestQuery.CountAsync(g => g.attndnc.Date <= today && g.attndnc.Date >= last7Days && g.reg.CompanyType == "Non Panasonic" && g.attndnc.Flag == true);
                    counts["guest-panasonic-in"] = await guestQuery.CountAsync(g => g.attndnc.Date <= today && g.attndnc.Date >= last7Days && g.reg.CompanyType == "Panasonic");
                    counts["guest-panasonic-out"] = await guestQuery.CountAsync(g => g.attndnc.Date <= today && g.attndnc.Date >= last7Days && g.reg.CompanyType == "Panasonic" && g.attndnc.Flag == true);
                    counts["supplier-in"] = await supplierQuery.CountAsync(v => DateOnly.FromDateTime(v.InTime) <= today && DateOnly.FromDateTime(v.InTime) >= last7Days);
                    counts["supplier-out"] = await supplierQuery.CountAsync(v => DateOnly.FromDateTime(v.InTime) <= today && DateOnly.FromDateTime(v.InTime) >= last7Days && v.Flag == true);
                    counts["contractor-in"] = await contractorQuery.CountAsync(c => c.attndnc.Date <= today && c.attndnc.Date >= last7Days);
                    counts["contractor-out"] = await contractorQuery.CountAsync(c => c.attndnc.Date <= today && c.attndnc.Date >= last7Days && c.attndnc.Flag == true);
                    break;
                case "this-month":
                    counts["emp-out"] = await empQuery.CountAsync(e => e.Date.Month == month);
                    counts["emp-in"] = await empQuery.CountAsync(e => e.Date.Month == month && e.Flag == true);
                    counts["guest-in"] = await guestQuery.CountAsync(g => g.attndnc.Date.Month == month && g.reg.CompanyType == "Non Panasonic");
                    counts["guest-out"] = await guestQuery.CountAsync(g => g.attndnc.Date.Month == month && g.reg.CompanyType == "Non Panasonic" && g.attndnc.Flag == true);
                    counts["guest-panasonic-in"] = await guestQuery.CountAsync(g => g.attndnc.Date.Month == month && g.reg.CompanyType == "Panasonic");
                    counts["guest-panasonic-out"] = await guestQuery.CountAsync(g => g.attndnc.Date.Month == month && g.reg.CompanyType == "Panasonic" && g.attndnc.Flag == true);
                    counts["supplier-in"] = await supplierQuery.CountAsync(v => DateOnly.FromDateTime(v.InTime).Month == month);
                    counts["supplier-out"] = await supplierQuery.CountAsync(v => DateOnly.FromDateTime(v.InTime).Month == month && v.Flag == true);
                    counts["contractor-in"] = await contractorQuery.CountAsync(c => c.attndnc.Date.Month == month);
                    counts["contractor-out"] = await contractorQuery.CountAsync(c => c.attndnc.Date.Month == month && c.attndnc.Flag == true);
                    break;
                case "last-3-month":
                    var last3Months = DateTime.Today.AddMonths(-3).Month;
                    counts["emp-out"] = await empQuery.CountAsync(e => e.Date <= today && e.Date.Month >= last3Months);
                    counts["emp-in"] = await empQuery.CountAsync(e => e.Date <= today && e.Date.Month >= last3Months && e.Flag == true);
                    counts["guest-in"] = await guestQuery.CountAsync(g => g.attndnc.Date <= today && g.attndnc.Date.Month >= last3Months && g.reg.CompanyType == "Non Panasonic");
                    counts["guest-out"] = await guestQuery.CountAsync(g => g.attndnc.Date <= today && g.attndnc.Date.Month >= last3Months && g.reg.CompanyType == "Non Panasonic" && g.attndnc.Flag == true);
                    counts["guest-panasonic-in"] = await guestQuery.CountAsync(g => g.attndnc.Date <= today && g.attndnc.Date.Month >= last3Months && g.reg.CompanyType == "Panasonic");
                    counts["guest-panasonic-out"] = await guestQuery.CountAsync(g => g.attndnc.Date <= today && g.attndnc.Date.Month >= last3Months && g.reg.CompanyType == "Panasonic" && g.attndnc.Flag == true);
                    counts["supplier-in"] = await supplierQuery.CountAsync(v => DateOnly.FromDateTime(v.InTime) <= today && DateOnly.FromDateTime(v.InTime).Month >= last3Months);
                    counts["supplier-out"] = await supplierQuery.CountAsync(v => DateOnly.FromDateTime(v.InTime) <= today && DateOnly.FromDateTime(v.InTime).Month >= last3Months && v.Flag == true);
                    counts["contractor-in"] = await contractorQuery.CountAsync(c => c.attndnc.Date <= today && c.attndnc.Date.Month >= last3Months);
                    counts["contractor-out"] = await contractorQuery.CountAsync(c => c.attndnc.Date <= today && c.attndnc.Date.Month >= last3Months && c.attndnc.Flag == true);
                    break;
                default:
                    counts = await GetCounts("today");
                    ViewBag.sum = "today";
                    break;
            }

            return counts;
        }
        private static List<DataPoint> GetCountDataPointsBar(Dictionary<string, int> counts, int total)
        {
            if (total == 0)
            {
                return [new DataPoint("No Data", 1, "rgb(115, 115, 115)")];
            }

            return
            [
                new DataPoint("Employee Out", counts["emp-out"], "rgb(20, 80, 180)"),
                new DataPoint("Employee In", counts["emp-in"], "rgb(20, 80, 180)"),
                new DataPoint("Guest In", counts["guest-in"], "rgb(245, 35, 120)"),
                new DataPoint("Guest Out", counts["guest-out"], "rgb(245, 35, 120)"),
                new DataPoint("Guest (Panasonic) In", counts["guest-panasonic-in"], "rgb(245, 35, 120)"),
                new DataPoint("Guest (Panasonic) Out", counts["guest-panasonic-out"], "rgb(245, 35, 120)"),
                new DataPoint("Supplier In", counts["supplier-in"], "rgb(233, 183, 54)"),
                new DataPoint("Supplier Out", counts["supplier-out"], "rgb(233, 183, 54)"),
                new DataPoint("Contractor In", counts["contractor-in"], "rgb(35, 135, 35)"),
                new DataPoint("Contractor Out", counts["contractor-out"], "rgb(35, 135, 35)")
            ];
        }
        private async Task<int> CountEmployees(string level)
        {
            var empId = HttpContext.Session.GetString("_Id");

            // Fetch department in a single query
            var dept = await db.DdedmEmployees
                .Where(e => e.EmpId == empId)
                .Select(e => e.Department)
                .SingleOrDefaultAsync();

            // Get employees in the department if level is "2"
            List<string> empIdsInDept = null;
            if (level == "3" || level == "4")
            {
                empIdsInDept = await db.DdedmEmployees
                    .Where(e => e.Department == dept)
                    .Select(e => e.EmpId)
                    .ToListAsync();
            }

            // Get clocking information based on the date and shifts
            var clockingsToday = db.EmployeeClockings
                .Where(e => e.ClockingDate == today && (e.ShiftId == "01" || e.ShiftId == "08") && e.InOut == "I");

            if (level == "3" || level == "4")
            {
                clockingsToday = clockingsToday.Where(e => empIdsInDept.Contains(e.EmpId));
            }

            // Count today's clockings  
            var countToday = await clockingsToday.CountAsync();

            int count;
            if (countToday == 0)
            {
                // Get clocking information for yesterday if no clockings today
                var clockingsYesterday = db.EmployeeClockings
                    .Where(e => e.ClockingDate == yesterday && (e.ShiftId == "01" || e.ShiftId == "08") && e.InOut == "I");

                if (level == "3" || level == "4")
                {
                    clockingsYesterday = clockingsYesterday.Where(e => empIdsInDept.Contains(e.EmpId));
                }

                var countYesterday = await clockingsYesterday.CountAsync();
                var faultyYesterday = await db.TrxTmsEmpPcDtKpsFltAct.Where(e => e.Date == yesterday).CountAsync();

                count = countYesterday - faultyYesterday;

                // Get the latest clock-in time from yesterday
                var lastClockInYesterday = await clockingsYesterday
                    .OrderByDescending(e => e.ClockingDate)
                    .ThenByDescending(e => e.ClockingTime)
                    .FirstOrDefaultAsync();

                if (lastClockInYesterday != null)
                {
                    empUpdate = $"{lastClockInYesterday.ClockingDate.ToString(CultureInfo.InvariantCulture)} {lastClockInYesterday.ClockingTime:HH:mm}";
                }
                else
                {
                    empUpdate = DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
                }
            }
            else
            {
                var faultyToday = await db.TrxTmsEmpPcDtKpsFltAct.Where(e => e.Date == today).CountAsync();
                count = countToday - faultyToday;

                // Get the latest clock-in time from today
                var lastClockInToday = await clockingsToday
                    .OrderByDescending(e => e.ClockingDate)
                    .ThenByDescending(e => e.ClockingTime)
                    .FirstOrDefaultAsync();

                if (lastClockInToday != null)
                {
                    empUpdate = $"{lastClockInToday.ClockingDate.ToString(CultureInfo.InvariantCulture)} {lastClockInToday.ClockingTime:HH:mm}";
                } else
                {
                    empUpdate = DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
                }
            }

            return count < 0 ? 0 : count ;
        }

        private async Task<int> CountGuests(string level)
        {
            var empId = HttpContext.Session.GetString("_Id");

            // Fetch department in a single query
            var dept = await db.DdedmEmployees
                .Where(e => e.EmpId == empId)
                .Select(e => e.Department)
                .SingleOrDefaultAsync();

            // Get guests and calculate total in a single query
            var query = db.GuestAttndnc
                .Where(g => !g.Flag)
                .Join(
                    db.RegGuest,
                    attndnc => attndnc.TransactionId,
                    reg => reg.Id,
                    (attndnc, reg) => new
                    {
                        attndnc.TimeIn,
                        attndnc.Date,
                        reg.MetId,
                        attndnc.Total
                    });

            // Filter by department if level is "2"
            if (level == "3" || level == "4")
            {
                query = query.Join(
                    db.DdedmEmployees.Where(e => e.Department == dept),
                    g => g.MetId,
                    e => e.EmpId,
                    (g, e) => g
                );
            }

            // Execute the query and aggregate results
            var result = await query
                .GroupBy(g => true)
                .Select(g => new
                {
                    TotalCount = g.Sum(x => x.Total),
                    LastUpdateTime = g.Max(x => x.TimeIn),
                    LastUpdateDate = g.Max(x => x.Date)
                })
                .FirstOrDefaultAsync();

            // Handle null results and construct the guestUpdate
            var count = result?.TotalCount ?? 0;
            var lastUpdateTime = result?.LastUpdateTime.ToString(CultureInfo.InvariantCulture) ?? TimeOnly.FromDateTime(DateTime.Now).ToString(CultureInfo.InvariantCulture);
            var lastUpdateDate = result?.LastUpdateDate.ToString(CultureInfo.InvariantCulture) ?? DateOnly.FromDateTime(DateTime.Now).ToString(CultureInfo.InvariantCulture);

            // Combine date and time for the last update
            guestUpdate = $"{lastUpdateDate} {lastUpdateTime:HH:mm}";

            return count;
        }
        private async Task<int> CountSuppliers()
        {
            var suppliers = db.VhcSpplier.Where(s => !s.Flag);

            // Execute the query once and get both the count and the last update time
            var result = await suppliers
                .GroupBy(s => true)
                .Select(g => new
                {
                    Count = g.Count(),
                    LastUpdate = g.Max(s => s.InTime)
                })
                .FirstOrDefaultAsync();

            // If there are no suppliers, set update to default value
            var update = result?.LastUpdate ?? DateTime.Now;

            suppUpdate = update.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
            return result?.Count ?? 0;
        }
        private async Task<int> CountContractors(string level)
        {
            var empId = HttpContext.Session.GetString("_Id");

            // Fetch department in a single query
            var dept = await db.DdedmEmployees
                .Where(e => e.EmpId == empId)
                .Select(e => e.Department)
                .SingleOrDefaultAsync();

            // Get contractors and calculate total count and latest update in a single query
            var query = db.WrkPermAttndnc
                .Where(w => !w.Flag)
                .Join(
                    db.WrkPermDesc,
                    attndnc => attndnc.RegNum,
                    reg => reg.RegNum,
                    (attndnc, reg) => new
                    {
                        attndnc.InTime,
                        attndnc.Date,
                        reg.Location
                    });

            // Filter by department if level is "3"
            if (level == "3" || level == "4")
            {
                query = query.Where(c => c.Location.ToLower().Contains(dept.ToLower()));
            }

            // Execute the query and aggregate results
            var result = await query
                .GroupBy(c => true)
                .Select(g => new
                {
                    TotalCount = g.Count(),
                    LastUpdate = g.Max(x => x.InTime)
                })
                .FirstOrDefaultAsync();

            // Handle null results
            var count = result?.TotalCount ?? 0;
            var update = result?.LastUpdate ?? TimeOnly.FromDateTime(DateTime.Now);

            conUpdate = DateOnly.FromDateTime(DateTime.Now).ToString(CultureInfo.InvariantCulture) + " " + update.ToString("HH:mm", CultureInfo.InvariantCulture);

            return count;
        }
        private async Task<List<Log>?> GetModel(string filterLog)
        {
            return filterLog switch
            {
                "plan" => await LogActivityService.GetPlan(db),
                "in" => await LogActivityService.GetIn(db),
                "out" => await LogActivityService.GetOut(db),
                _ => await LogActivityService.GetPlan(db)
            };
        }
        private async Task<List<Log>?> GetDeptModel(string filterLog)
        {
            var empId = HttpContext.Session.GetString("_Id") ?? throw new Exception("Session Id undetected");
            var dept = await db.DdedmEmployees.Where(e => e.EmpId == empId).Select(e => e.Department).SingleOrDefaultAsync() ?? throw new Exception("No Department found");

            return filterLog switch
            {
                "plan" => await LogActivityService.GetDeptPlan(db, dept),
                "in" => await LogActivityService.GetDeptIn(db, dept),
                "out" => await LogActivityService.GetDeptOut(db, dept),
                _ => await LogActivityService.GetDeptPlan(db, dept)
            };
        }
    }
}