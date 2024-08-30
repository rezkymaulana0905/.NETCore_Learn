using Microsoft.EntityFrameworkCore;
using PTC.Data;
using PTC.Models;


namespace PTC.Services;

public class GuestService
{
    public static async Task<object> GetGuestById(string id, PtcContext db)
    {
        var guest =  await db.RegGuest.AsNoTracking()
        .Join(
        db.DdedmEmployees,
        regGuest => regGuest.MetId,
        edm => edm.EmpId,
        (regGuest, edm) => new
        {
            Met = new 
            {
                MetName = edm.Name,
                MetDept = edm.Department,
                MetSect = edm.SectionStru
            },
            Guest = new 
            {
                regGuest.Id,
                regGuest.StartDate,
                regGuest.EndDate,
                GuestName = regGuest.Name,
                regGuest.NationalId,
                GuestCompany = regGuest.Company,
                GuestDeptSect = regGuest.DeptSect,
                regGuest.Requirement,
                regGuest.Total,
                ImageDate = regGuest.ImageData
            }
        })
        .FirstOrDefaultAsync(model => model.Guest.Id == id);

        return guest ?? throw new Exception("Data Not Found");
    }

    public static async Task<IResult> ConfirmEnter(string id, int total, UserAuth userAuth, PtcContext db)
    {
        var guestRegist = await db.RegGuest
            .Where(model => model.Id == id)
            .FirstOrDefaultAsync() ?? throw new Exception("ID Not Found");

        var date = DateOnly.FromDateTime(DateTime.Now);

        var check = await db.GuestAttndnc
            .Where(g => g.TransactionId == id && g.Flag == false)
            .FirstOrDefaultAsync();

        if  (check == null)
        {
            if (guestRegist.StartDate <= date && guestRegist.EndDate >= date)
            {
                guestRegist.Total = total;
                GuestAttndnc guestAttndnc = await SetGuestAttndnc(db, guestRegist);
                await SetGuestRecord(userAuth, db, guestAttndnc, "I"); return TypedResults.Ok();
            }
            throw new Exception("Wrong Date");
        }
        
        throw new Exception("This Guest Found Haven't Scanned Out");
    }

    public static async Task<IResult> ConfirmLeave(string id, UserAuth userAuth, PtcContext db)
    {
        var guestAttndnc = await db.GuestAttndnc
                        .Where(model => model.TransactionId == id && model.Flag == false)
                        .OrderBy(model => model.Id)
                        .LastOrDefaultAsync() ?? throw new Exception("Data Not Found");


        guestAttndnc.TimeOut = new TimeOnly(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        guestAttndnc.Flag = true;

        await db.SaveChangesAsync();


        await SetGuestRecord(userAuth, db, guestAttndnc, "O"); 
        
        return TypedResults.Ok();        
    }

    public static async Task<string> GetCategoryName(int id, PtcContext db)
    {
        var category = await db.GuestCategory.Where(c => c.Id == id).Select(c => c.CategoryName).FirstOrDefaultAsync();

        return category;
    }

    public static async Task<string> GetCountryName(string code, PtcContext db)
    {
        var country = await db.Country.Where(c => c.Code == code).Select(c => c.Name).FirstOrDefaultAsync();

        return country;
    }

    private static async Task<GuestAttndnc> SetGuestAttndnc(PtcContext db, GuestReg guestRegist)
    {
        var guestAttndnc = new GuestAttndnc
        {
            TransactionId = guestRegist.Id,
            Date = DateOnly.FromDateTime(DateTime.Now),
            Total = guestRegist.Total,
            TimeIn = new TimeOnly(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
            TimeOut = null,
            Flag = false
        };
        db.GuestAttndnc.Add(guestAttndnc);
        await db.SaveChangesAsync();
        return guestAttndnc;
    }

    private static async Task SetGuestRecord(UserAuth userAuth, PtcContext db, GuestAttndnc guestAttndnc, string type)
    {
        var record = new GuestScanRecord
        {
            Username = userAuth.UserName,
            LoginId = userAuth.LoginId,
            TransactionId = guestAttndnc.Id,
            Type = type,
            ScanTime = DateTime.Now
        };
        db.GuestScanRecord.Add(record);
        await db.SaveChangesAsync();
    }
}
