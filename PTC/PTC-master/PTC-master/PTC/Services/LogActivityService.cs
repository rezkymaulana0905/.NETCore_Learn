using Microsoft.EntityFrameworkCore;
using PTC.Data;
using PTC.Models;

namespace PTC.Services;

public class LogActivityService
{
    public static DateOnly today = DateOnly.FromDateTime(DateTime.Now);

    public static async Task<List<Log>> GetPlan(PtcContext db)
    {
        var guest = await db.RegGuest
                .Where(g => g.StartDate <= today && g.EndDate >= today)
                .Select(g => new Log
                {
                    Date = today,
                    Name = g.Name + " - " + g.Total + " - " + g.Company + (g.CompanyType == "Panasonic" ? " - Panasonic" : ""),
                    Time = null,
                    Detail = "Guest",
                    Type = "Guest - In"
                }).ToListAsync();

        var emp = await db.TrxTmsEmpPcDtKpsFlt
        .Where(e => e.Date == today)
        .Select(e => new Log
        {
            Date = today,
            Name = e.Name + " - " + e.TimereasonId,
            Time = e.TimeOut,
            Detail = "Employee",
            Type = "Employee - Out"
        }).ToListAsync();

        var worker = await db.WrkPermDesc
        .Where(d => d.Start <= today && d.End >= today)
        .Join(db.WrkPermWorker,
        desc => desc.RegNum,
        worker => worker.RegNum,
        (desc, worker) => new
        {
            Desc = desc,
            Worker = worker
        }).Select(j => new Log
        {
            Date = today,
            Name = j.Worker.Name + " - " + j.Desc.CompName,
            Time = null,
            Detail = "Contractor",
            Type = "Contractor - In"
        }).ToListAsync();

        var combinedResults = guest.Concat(emp).Concat(worker);

        var orderedResults = combinedResults
                            .OrderByDescending(r => r.Time)
                            .ToList();

        return orderedResults;
    }
    public static async Task<List<Log>> GetDeptPlan(PtcContext db, string dept)
    {
        var empIdsInDept = await db.DdedmEmployees
                    .Where(e => e.Department == dept)
                    .Select(e => e.EmpId)
                    .ToListAsync();

        var guest = await db.RegGuest
                .Where(g => g.StartDate <= today && g.EndDate >= today && empIdsInDept.Contains(g.MetId))
                .Select(g => new Log
                {
                    Date = today,
                    Name = g.Name + " - " + g.Total + " - " + g.Company + (g.CompanyType == "Panasonic" ? " - Panasonic" : ""),
                    Time = null,
                    Detail = "Guest",
                    Type = "Guest - In"
                }).ToListAsync();

        var emp = await db.TrxTmsEmpPcDtKpsFlt
        .Where(e => e.Date == today && e.Department == dept)
        .Select(e => new Log
        {
            Date = today,
            Name = e.Name,
            Time = e.TimeOut,
            Detail = "Employee",
            Type = "Employee - Out"
        }).ToListAsync();

        var worker = await db.WrkPermDesc
        .Where(d => d.Start <= today && d.End >= today && d.Location.ToLower().Contains(dept.ToLower()))
        .Join(db.WrkPermWorker,
        desc => desc.RegNum,
        worker => worker.RegNum,
        (desc, worker) => new
        {
            Desc = desc,
            Worker = worker
        }).Select(j => new Log
        {
            Date = today,
            Name = j.Worker.Name + " - " + j.Desc.CompName,
            Time = null,
            Detail = "Contractor",
            Type = "Contractor - In"
        }).ToListAsync();

        var combinedResults = guest.Concat(emp).Concat(worker);

        var orderedResults = combinedResults
                            .OrderByDescending(r => r.Time)
                            .ToList();

        return orderedResults;
    }
    public static async Task<List<Log>> GetOut(PtcContext db)
    {
        // Fetch all the required data in a single query
        var guestDetails = await db.GuestAttndnc
        .Join(db.RegGuest,
          attndnc => attndnc.TransactionId,
          reg => reg.Id,
          (attndnc, reg) => new { attndnc.TransactionId, reg.Id, reg.Name, reg.Company, reg.CompanyType })
        .GroupBy(x => x.TransactionId)
        .Select(g => g.FirstOrDefault())
        .ToListAsync();


        // Create dictionaries from the fetched data
        var names = guestDetails.ToDictionary(g => g.Id, g => g.Name);
        var company = guestDetails.ToDictionary(g => g.Id, g => g.Company);
        var companyType = guestDetails.ToDictionary(g => g.Id, g => g.CompanyType);


        var guestOut = await db.GuestAttndnc
            .Where(g => g.Date == today && g.TimeOut != null)
            .Select(g => new Log
            {
                Date = g.Date,
                Name = names[g.TransactionId] + " - " + company[g.TransactionId] + (companyType[g.TransactionId] == "Panasonic" ? " - Panasonic" : ""),
                Time = g.TimeOut ?? TimeOnly.MinValue,
                Detail = "Guest",
                Type = "Out"
            })
            .ToListAsync();

        var empOut = await db.TrxTmsEmpPcDtKpsFltAct
            .Where(e => e.Date == today && !e.Flag)
            .Select(t => new Log
            {
                Date = t.Date,
                Name = t.Name,
                Time = t.TimeOut ?? TimeOnly.MinValue,
                Detail = "Employee",
                Type = "Out"
            })
            .ToListAsync();

        var vhcOut = await db.VhcSpplier
            .Where(v => DateOnly.FromDateTime(v.InTime) == today && v.ConfirmOutTime != null)
            .Select(v => new Log
            {
                Date = DateOnly.FromDateTime(v.InTime),
                Name = v.VehicleId + "-" + v.VehicleType + "-" + v.Company,
                Time = v.ConfirmOutTime.HasValue ? new TimeOnly(v.ConfirmOutTime.Value.Hour, v.ConfirmOutTime.Value.Minute, v.ConfirmOutTime.Value.Second) : TimeOnly.MinValue,
                Detail = "Supplier",
                Type = "Out"
            })
            .ToListAsync();

        var workerOut = await db.WrkPermAttndnc
            .Where(w => w.Date == today && w.Flag == true)
            .Join(db.WrkPermWorker,
            attndnc => attndnc.WorkerId,
            worker => worker.Id,
            (attndnc, worker) => new
            {
                Attndnc = attndnc,
                Worker = worker
            })
            .Join(db.WrkPermDesc,
            w => w.Attndnc.RegNum,
            desc => desc.RegNum,
            (w, desc) => new
            {
                w.Attndnc,
                w.Worker,
                Desc = desc
            })
            .Select(w => new Log
            {
                Date = w.Attndnc.Date,
                Name = w.Worker.Name + " - " + w.Desc.CompName,
                Time = w.Attndnc.OutTime,
                Detail = "Contractor",
                Type = "Out"
            })
            .ToListAsync();

        var combinedResults = guestOut.Concat(vhcOut).Concat(empOut).Concat(workerOut);

        var orderedResults = combinedResults
            .OrderByDescending(r => r.Date)
            .ThenByDescending(r => r.Time)
            .ToList();

        return orderedResults;
    }

    public static async Task<List<Log>> GetDeptOut(PtcContext db, string dept)
    {
        var empIdsInDept = await db.DdedmEmployees
                    .Where(e => e.Department == dept)
                    .Select(e => e.EmpId)
                    .ToListAsync();

        // Fetch all the required data in a single query
        var guestDetails = await db.GuestAttndnc
                    .Join(db.RegGuest,
                      attndnc => attndnc.TransactionId,
                      reg => reg.Id,
                      (attndnc, reg) => new { attndnc.TransactionId, reg.Id, reg.Name, reg.Company, reg.CompanyType })
                    .GroupBy(x => x.TransactionId)
                    .Select(g => g.FirstOrDefault())
                    .ToListAsync();


        // Create dictionaries from the fetched data
        var names = guestDetails.ToDictionary(g => g.Id, g => g.Name);
        var company = guestDetails.ToDictionary(g => g.Id, g => g.Company);
        var companyType = guestDetails.ToDictionary(g => g.Id, g => g.CompanyType);


        var guestOut = await db.GuestAttndnc
            .Join(db.RegGuest,
            attndnc => attndnc.TransactionId,
            reg => reg.Id,
            (attndnc, req) => new
            {
                attndnc,
                req
            })
            .Where(g => g.attndnc.Date == today && g.attndnc.TimeOut != null && empIdsInDept.Contains(g.req.MetId))
            .Select(g => new Log
            {
                Date = g.attndnc.Date,
                Name = names[g.attndnc.TransactionId] + " - " + company[g.attndnc.TransactionId] + (companyType[g.attndnc.TransactionId] == "Panasonic" ? " - Panasonic" : ""),
                Time = g.attndnc.TimeOut ?? TimeOnly.MinValue,
                Detail = "Guest",
                Type = "Out"
            })
            .ToListAsync();

        var empOut = await db.TrxTmsEmpPcDtKpsFltAct
            .Where(e => e.Date == today && !e.Flag && e.Department == dept)
            .Select(t => new Log
            {
                Date = t.Date,
                Name = t.Name,
                Time = t.TimeOut ?? TimeOnly.MinValue,
                Detail = "Employee",
                Type = "Out"
            })
            .ToListAsync();

        var vhcOut = await db.VhcSpplier
            .Where(v => DateOnly.FromDateTime(v.InTime) == today && v.ConfirmOutTime != null)
            .Select(v => new Log
            {
                Date = DateOnly.FromDateTime(v.InTime),
                Name = v.VehicleId + "-" + v.VehicleType + "-" + v.Company,
                Time = v.ConfirmOutTime.HasValue ? new TimeOnly(v.ConfirmOutTime.Value.Hour, v.ConfirmOutTime.Value.Minute, v.ConfirmOutTime.Value.Second) : TimeOnly.MinValue,
                Detail = "Supplier",
                Type = "Out"
            })
            .ToListAsync();

        var workerOut = await db.WrkPermAttndnc
            .Join(db.WrkPermWorker,
            attndnc => attndnc.WorkerId,
            worker => worker.Id,
            (attndnc, worker) => new
            {
                attndnc = attndnc,
                worker = worker
            })
            .Join(db.WrkPermDesc,
            w => w.attndnc.RegNum,
            desc => desc.RegNum,
            (w, desc) => new
            {
                w.attndnc,
                w.worker,
                desc = desc
            })
            .Where(w => w.attndnc.Date == today && w.attndnc.Flag == true && w.desc.Location.ToLower().Contains(dept.ToLower()))
            .Select(w => new Log
            {
                Date = w.attndnc.Date,
                Name = w.worker.Name + " - " + w.desc.CompName,
                Time = w.attndnc.OutTime,
                Detail = "Contractor",
                Type = "Out"
            })
            .ToListAsync();

        var combinedResults = guestOut.Concat(vhcOut).Concat(empOut).Concat(workerOut);

        var orderedResults = combinedResults
            .OrderByDescending(r => r.Date)
            .ThenByDescending(r => r.Time)
            .ToList();

        return orderedResults;
    }

    public static async Task<List<Log>> GetIn(PtcContext db)
    {
        // Fetch all the required data in a single query
        var guestDetails = await db.GuestAttndnc
    .Join(db.RegGuest,
          attndnc => attndnc.TransactionId,
          reg => reg.Id,
          (attndnc, reg) => new { attndnc.TransactionId, reg.Id, reg.Name, reg.Company, reg.CompanyType })
    .GroupBy(x => x.TransactionId)
    .Select(g => g.FirstOrDefault())
    .ToListAsync();


        // Create dictionaries from the fetched data
        var names = guestDetails.ToDictionary(g => g.Id, g => g.Name);
        var company = guestDetails.ToDictionary(g => g.Id, g => g.Company);
        var companyType = guestDetails.ToDictionary(g => g.Id, g => g.CompanyType);

        var guestIn = await db.GuestAttndnc
            .Where(e => e.Date == today)
            .Select(g => new Log
            {
                Date = g.Date,
                Name = names[g.TransactionId] + " - " + company[g.TransactionId] + (companyType[g.TransactionId] == "Panasonic" ? " - Panasonic" : ""),
                Time = g.TimeIn,
                Detail = "Guest",
                Type = "In"
            })
            .ToListAsync();

        var empIn = await db.TrxTmsEmpPcDtKpsFltAct
            .Where(e => e.Date == today && e.TimeReturn != null)
            .Select(t => new Log
            {
                Date = t.Date,
                Name = t.Name,
                Time = t.TimeReturn ?? TimeOnly.MinValue,
                Detail = "Employee",
                Type = "In"
            })
            .ToListAsync();

        var vhcIn = await db.VhcSpplier
            .Where(v => DateOnly.FromDateTime(v.InTime) == today)
            .Select(v => new Log
            {
                Date = DateOnly.FromDateTime(v.InTime),
                Name = v.VehicleId + "-" + v.VehicleType + "-" + v.Company,
                Time = new TimeOnly(v.InTime.Hour, v.InTime.Minute, v.InTime.Second),
                Detail = "Supplier",
                Type = "In"
            })
            .ToListAsync();

        var workerIn = await db.WrkPermAttndnc
            .Where(w => w.Date == today)
            .Join(db.WrkPermWorker,
            attndnc => attndnc.WorkerId,
            worker => worker.Id,
            (attndnc, worker) => new
            {
                Attndnc = attndnc,
                Worker = worker
            })
            .Join(db.WrkPermDesc,
            w => w.Attndnc.RegNum,
            desc => desc.RegNum,
            (w, desc) => new
            {
                w.Attndnc,
                w.Worker,
                Desc = desc
            })
            .Select(w => new Log
            {
                Date = w.Attndnc.Date,
                Name = w.Worker.Name + " - " + w.Desc.CompName,
                Time = w.Attndnc.InTime,
                Detail = "Contractor",
                Type = "In"
            })
            .ToListAsync();


        var combinedResults = guestIn.Concat(vhcIn).Concat(empIn).Concat(workerIn);

        var orderedResults = combinedResults
            .OrderByDescending(r => r.Date)
            .ThenByDescending(r => r.Time)
            .ToList();

        return orderedResults;
    }

    public static async Task<List<Log>> GetDeptIn(PtcContext db, string dept)
    {
        var empIdsInDept = await db.DdedmEmployees
                    .Where(e => e.Department == dept)
                    .Select(e => e.EmpId)
                    .ToListAsync();

        // Fetch all the required data in a single query
        var guestDetails = await db.GuestAttndnc
        .Join(db.RegGuest,
        attndnc => attndnc.TransactionId,
        reg => reg.Id,
        (attndnc, reg) => new { attndnc.TransactionId, reg.Id, reg.Name, reg.Company, reg.CompanyType })
        .GroupBy(x => x.TransactionId)
        .Select(g => g.FirstOrDefault())
        .ToListAsync();


        // Create dictionaries from the fetched data
        var names = guestDetails.ToDictionary(g => g.Id, g => g.Name);
        var company = guestDetails.ToDictionary(g => g.Id, g => g.Company);
        var companyType = guestDetails.ToDictionary(g => g.Id, g => g.CompanyType);

        var guestIn = await db.GuestAttndnc
            .Join(db.RegGuest,
            attndnc => attndnc.TransactionId,
            reg => reg.Id,
            (attndnc, req) => new
            {
                attndnc,
                req
            })
            .Where(g => g.attndnc.Date == today && empIdsInDept.Contains(g.req.MetId))
            .Select(g => new Log
            {
                Date = g.attndnc.Date,
                Name = names[g.attndnc.TransactionId] + " - " + company[g.attndnc.TransactionId] + (companyType[g.attndnc.TransactionId] == "Panasonic" ? " - Panasonic" : ""),
                Time = g.attndnc.TimeOut ?? TimeOnly.MinValue,
                Detail = "Guest",
                Type = "Out"
            })
            .ToListAsync();

        var empIn = await db.TrxTmsEmpPcDtKpsFltAct
            .Where(e => e.Date == today && e.TimeReturn != null && e.Department == dept)
            .Select(t => new Log
            {
                Date = t.Date,
                Name = t.Name,
                Time = t.TimeReturn ?? TimeOnly.MinValue,
                Detail = "Employee",
                Type = "In"
            })
            .ToListAsync();

        var vhcIn = await db.VhcSpplier
            .Where(v => DateOnly.FromDateTime(v.InTime) == today)
            .Select(v => new Log
            {
                Date = DateOnly.FromDateTime(v.InTime),
                Name = v.VehicleId + "-" + v.VehicleType + "-" + v.Company,
                Time = new TimeOnly(v.InTime.Hour, v.InTime.Minute, v.InTime.Second),
                Detail = "Supplier",
                Type = "In"
            })
            .ToListAsync();

        var workerIn = await db.WrkPermAttndnc
            .Join(db.WrkPermWorker,
            attndnc => attndnc.WorkerId,
            worker => worker.Id,
            (attndnc, worker) => new
            {
                Attndnc = attndnc,
                Worker = worker
            })
            .Join(db.WrkPermDesc,
            w => w.Attndnc.RegNum,
            desc => desc.RegNum,
            (w, desc) => new
            {
                w.Attndnc,
                w.Worker,
                Desc = desc
            })
            .Where(w => w.Attndnc.Date == today && w.Desc.Location.ToLower().Contains(dept.ToLower()))
            .Select(w => new Log
            {
                Date = w.Attndnc.Date,
                Name = w.Worker.Name + " - " + w.Desc.CompName,
                Time = w.Attndnc.InTime,
                Detail = "Contractor",
                Type = "In"
            })
            .ToListAsync();


        var combinedResults = guestIn.Concat(vhcIn).Concat(empIn).Concat(workerIn);

        var orderedResults = combinedResults
            .OrderByDescending(r => r.Date)
            .ThenByDescending(r => r.Time)
            .ToList();

        return orderedResults;
    }
}