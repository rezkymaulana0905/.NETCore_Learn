using Microsoft.EntityFrameworkCore;
using PTC.Data;
using PTC.Models;

namespace PTC.Services
{
    public class EmployeeService
    {
        public static async Task<DdedmEmployee> GetEmployeeByEmpId(string empId, PtcContext db)
        {
            if (string.IsNullOrEmpty(empId))
                throw new ArgumentException("Employee ID cannot be null or empty.", nameof(empId));

            var emp = await db.DdedmEmployees.FirstOrDefaultAsync(m => m.EmpId == empId);

            return emp ?? throw new ArgumentException("Employee ID Not Found");
        }
    }
}
