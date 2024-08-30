using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PTC.Data;
using PTC.Models;

namespace PTC.Controllers
{
    public class GuestRegistController(PtcContext context) : Controller
    {
        private readonly PtcContext db = context;
        private readonly string[] boardRoles = ["Direksi", "Wakil Direktur Utama"];

        public async Task<IActionResult> Index(string selectedSection)
        {
            var empId = HttpContext.Session.GetString("_Id");
            var level = HttpContext.Session.GetString("_Level");

            List<SelectListItem> form = [];
            form.Add(new SelectListItem { Text = "Regular Form", Value = "RegularForm" });
            form.Add(new SelectListItem { Text = "VIP Form", Value = "VipForm" });

            if (string.IsNullOrEmpty(empId))
            {
                return Unauthorized();
            }

            var department = await GetDepartmentByEmpId(empId);
            if (string.IsNullOrEmpty(department))
            {
                return NotFound("Department not found for the employee");
            }

            var sections = await GetSectionsByDepartment(department, level);
            ViewBag.DropDownSec = new SelectList(sections, "SectionStru", "SectionStru");

            if (!string.IsNullOrEmpty(selectedSection))
            {
                var employeesInSection = await GetEmployeesBySection(selectedSection);
                ViewBag.DropDownEmp = new SelectList(employeesInSection, "EmpId", "Name");
                ViewBag.DropDownForm = new SelectList(form, "Value", "Text");
            }

            return View();
        }

        public async Task<IActionResult> Form(string? id, string formType)
        {
            var employee = await db.DdedmEmployees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            var viewName = formType;

            return RedirectToAction(viewName, "GuestRegistForm", new { MetId = id });
        }

        private async Task<string> GetDepartmentByEmpId(string empId)
        {
            return await db.DdedmEmployees
                .Where(x => x.EmpId == empId)
                .Select(x => x.Department)
                .FirstOrDefaultAsync();
        }

        private async Task<List<DdedmEmployee>> GetSectionsByDepartment(string department, string level)
        {
            var section = await db.DdedmEmployees
                               .Where(x => x.Department.Contains(department))
                               .GroupBy(x => x.SectionStru)
                               .Select(group => group.First())
                               .ToListAsync(); 
            if (level == "2")
            {
                var bod = await db.DdedmEmployees
                                .Where(x => boardRoles.Contains(x.Department))
                                .GroupBy(x => x.SectionStru)
                                .Select(group => group.First())
                                .ToListAsync();

                section.AddRange(bod);
            }

            return section;
        }

        private async Task<List<DdedmEmployee>> GetEmployeesBySection(string section)
        {
            return await db.DdedmEmployees
                .Where(x => x.SectionStru == section)
                .ToListAsync();
        }
    }
}