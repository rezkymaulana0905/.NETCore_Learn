using Microsoft.EntityFrameworkCore;
using PTC.Data;

namespace PTC.Services;

public class PeopleCountService
{
    private static readonly DateOnly today = DateOnly.FromDateTime(DateTime.Now);

    public static async Task<int> DepartmentPeopleCount(PtcContext db, string department)
    {
        var emp = await db.DdedmEmployees
                                  .Where(e => e.Department == department && e.Active == true)
                                  .CountAsync();

        var guest = await db.GuestAttndnc
                    .Where(g => g.Date == today && !g.Flag)
                    .Join(db.RegGuest,
                          attendance => attendance.TransactionId,
                          guest => guest.Id,
                          (attendance, guest) => guest)
                    .Join(db.DdedmEmployees,
                          guest => guest.MetId,
                          employee => employee.EmpId,
                          (guest, employee) => new { Guest = guest, Employee = employee })
                    .Where(joinResult => joinResult.Employee.Department == department)
                    .CountAsync();

        var empMin = await db.TrxTmsEmpPcDtKpsFltAct
                    .Where(e => e.Date == today && !e.Flag && e.Department == department)
                    .CountAsync();

        var count = emp + guest - empMin;
        return count;
    }
    public static async Task<int> GetPeopleCount(PtcContext db)
    {
        var emp = await db.DdedmEmployees.CountAsync();

        var guest = await db.GuestAttndnc
                    .Where(g => g.Date == today && !g.Flag)
                    .CountAsync();
        var empMin = await db.TrxTmsEmpPcDtKpsFltAct
                    .Where(e => e.Date == today && !e.Flag)
                    .CountAsync();

        var supplier = await db.VhcSpplier
                    .Where(s => DateOnly.FromDateTime(s.InTime) == today && s.ConfirmOutTime == null).CountAsync();

        var contractor = await db.WrkPermAttndnc
                    .Where(w => w.Date == today && !w.Flag).CountAsync();

        var count = emp + guest - empMin + supplier + contractor;

        return count;
    }
}
