using Microsoft.EntityFrameworkCore;
using PTC.Models;
using PTC.Data;
using System.Security.Claims;
namespace PTC.Services;

public class WorkPermitService
{
    public static readonly DateOnly today = DateOnly.FromDateTime(DateTime.Now);
    public static async Task<IResult> GetWorker(int id, PtcContext db)
    {
        var worker = await db.WrkPermWorker
        .Where(w => w.Id == id)
        .Join(db.WrkPermDesc.Where(d => d.Start <= today && d.End >= today),
        w => w.RegNum,
        d => d.RegNum,
        (w, d) => new
        {
            worker = w,
            desc = d
        }).Select(
            j => new
            {
                j.worker.Id,
                j.desc.RegNum,
                j.desc.Title,
                j.desc.CompName,
                j.desc.Location,
                j.desc.Start,
                j.desc.End,
                j.worker.Name,
                j.worker.NationalId
            }
            )
        .FirstOrDefaultAsync() ?? throw new Exception("Worker Not Found");
        return Results.Ok(worker);
    }
    public static async Task<IResult> ConfirmIn(PtcContext db, UserAuth userAuth, int id)
    {
        var worker = await db.WrkPermWorker
                            .Where(w => w.Id == id)
                            .FirstOrDefaultAsync() ?? throw new Exception("Worker Not Found");
        var check = await db.WrkPermAttndnc
                        .Where(w => w.WorkerId == id && w.Flag == false)
                        .FirstOrDefaultAsync();

        if (check == null)
        {
            var attndnc = new WrkPermAttndnc
            {
                RegNum = worker.RegNum,
                WorkerId = worker.Id,
                Date = DateOnly.FromDateTime(DateTime.Now),
                InTime = new TimeOnly(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                OutTime = null,
                Flag = false,
            };

            db.Add(attndnc);
            await db.SaveChangesAsync();

            await SetWrkerRecord(db, userAuth, attndnc, "I");

            return Results.Ok(attndnc);
        }

        throw new Exception("The Worker Found haven't Scanned Out");
    }
    public static async Task<IResult> GetWorkerRange(DateOnly start, DateOnly end, PtcContext db)
    {

        if (end > start)
        {
            throw new Exception(
                "Bad request : End Date value can't more than Start Date");
        }
        List<CompanyAttendance> result = [];

        var records = await db.WrkPermDesc
            .GroupJoin(db.WrkPermAttndnc.Where(a => a.Date >= start && a.Date <= end),
                       desc => desc.RegNum,
                       attndnc => attndnc.RegNum,
                       (desc, attendances) => new
                       {
                           Company = desc.CompName,
                           Total = attendances.Count()
                       })
            .OrderBy(o => o.Company)
            .Where(record => record.Total > 0)
            .ToListAsync();

        for (int i = 0; i < records.Count; i++)
        {
            if (i == 0 || records[i].Company != records[i - 1].Company)
            {
                var item = new CompanyAttendance
                {
                    Company = records[i].Company,
                    Total = records[i].Total
                };
                result.Add(item);
            }
            else
            {
                var existingRecord = result.LastOrDefault(r => r.Company == records[i].Company);
                if (existingRecord != null)
                {
                    existingRecord.Total += records[i].Total;
                }
            }
        }

        return Results.Ok(result);
    }
    public static async Task<IResult> ConfirmLeave(PtcContext db, UserAuth userAuth, int id)
    {
        var attdnc = await db.WrkPermAttndnc
                    .Where(a => a.WorkerId == id && a.Flag == false)
                    .OrderBy(a => a.Id)
                    .LastOrDefaultAsync() ?? throw new Exception("Attendance record Not Found.");
        attdnc.Flag = true;
        attdnc.OutTime = new TimeOnly(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        await db.SaveChangesAsync();

        await SetWrkerRecord(db, userAuth, attdnc, "O");
        return Results.Ok(attdnc);
    }
    public static async Task AddWorkerData(WorkPermit add, PtcContext db)
    {
        db.WrkPermDesc.Add(add.Description);
        await db.SaveChangesAsync();
        foreach (WrkPermWorker worker in add.Worker)
        {
            db.WrkPermWorker.Add(worker);
        }
        await db.SaveChangesAsync();
    }
    public static async Task<IResult> GetActual(PtcContext db)
    {
        var result = await db.WrkPermDesc
                    .Where(d => d.Start <= today && d.End >= today)
                    .Select(s => new
                    {
                        description = s,
                        worker = db.WrkPermAttndnc
                            .Where(a => a.RegNum == s.RegNum &&
                            a.Date == today
                            && !a.Flag)
                            .Join(db.WrkPermWorker,
                            a => a.WorkerId,
                            w => w.Id,
                            (a, w) => new
                            {
                                attndnc = a,
                                worker = w,
                            }
                            )
                            .OrderBy(j => j.attndnc.InTime)
                            .Select(j => new
                            {
                                id = j.worker.Id,
                                regNum = j.worker.RegNum,
                                name = j.worker.Name,
                                inTime = j.attndnc.InTime
                            })
                            .ToArray()
                    })
                    .ToListAsync();

        return Results.Ok(result);
    }
    public static async Task<IResult> GetActiveWorker(string reqNum, DateOnly date, PtcContext db)
    {
        var result = await db.WrkPermDesc
                    .Where(d => d.RegNum == reqNum)
                    .Select(s => new
                    {
                        description = s,
                        worker = db.WrkPermAttndnc
                            .Where(a => a.RegNum == s.RegNum &&
                            a.Date == date
                            )
                            .Join(db.WrkPermWorker,
                            a => a.WorkerId,
                            w => w.Id,
                            (a, w) => new
                            {
                                attndnc = a,
                                worker = w,
                            }
                            )
                            .OrderBy(j => j.attndnc.InTime)
                            .Select(j => new
                            {
                                id = j.worker.Id,
                                regNum = j.worker.RegNum,
                                name = j.worker.Name,
                                inTime = j.attndnc.InTime,
                                outTime = j.attndnc.OutTime
                            })
                            .ToArray()
                    })
                    .ToListAsync();

        return Results.Ok(result);
    }

    public static async Task SetWrkerRecord(PtcContext db, UserAuth userAuth, WrkPermAttndnc attndnc, string type)
    {
        var record = new WrkPermScanRecord
        {
            Username = userAuth.UserName,
            LoginId = userAuth.LoginId,
            TransactionId = attndnc.Id,
            Type = type,
            ScanTime = DateTime.Now
        };

        db.WrkPermScanRecord.Add(record);
        await db.SaveChangesAsync();
    }
}