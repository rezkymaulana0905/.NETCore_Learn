using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PTC.Data;
using PTC.Models;

namespace PTC.Services;

public class TrafficOutService
{
    public static async Task<IResult> GetEmpLeave(string id, PtcContext db)
    {
        var trafficOut = await db.TrxTmsEmpPcDtKpsFlt
            .Where(e => e.EmpId == id && e.Date == DateOnly.FromDateTime(DateTime.Now) && !e.Flag)
            .OrderBy(e => e.SeqNo)
            .FirstOrDefaultAsync() ?? throw new Exception("Data Not Found");

        return TypedResults.Ok(trafficOut);
    }

    public static async Task<object> GetEmpEnter(string id, PtcContext db)
    {
        var trafficOut = await db.TrxTmsEmpPcDtKpsFltAct
        .Where(e => e.EmpId == id && !e.Flag)
        .OrderBy(e => e.SeqNo)
        .LastOrDefaultAsync() ?? throw new Exception("Data Not Found");

        return TypedResults.Ok(trafficOut);
    }

    public static async Task<Ok> ConfirmLeave(PtcContext db, UserAuth userAuth, int seqNo)
    {

       var trafficOut = await db.TrxTmsEmpPcDtKpsFlt
       .Where(e => e.SeqNo == seqNo)
       .OrderBy(e => e.SeqNo)
       .LastOrDefaultAsync() ?? throw new Exception("Data Not Found");

       trafficOut.Flag = true;

        var actualTrafficOut = new TrxTmsEmpPcDtKpFltAct
       {
           SeqNo = seqNo,
           EmpId = trafficOut.EmpId,
           Flag = false,
           Name = trafficOut.Name,
           Department = trafficOut.Department,
           Date = DateOnly.FromDateTime(DateTime.Now),
           TimeOut = new TimeOnly(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
           TimereasonId = trafficOut?.TimereasonId ?? null,
           Reason = trafficOut?.Reason ?? null,
           CreateBy = trafficOut?.CreateBy ?? null,
       };
       db.TrxTmsEmpPcDtKpsFltAct.Add(actualTrafficOut);
       await db.SaveChangesAsync();

       await SetEmpRecord(db, userAuth, actualTrafficOut, "O");
       return TypedResults.Ok();
    }
    public static async Task<Ok> ConfirmEnter(int id, UserAuth userAuth, PtcContext db)
    {
        var actualTrafficOut = await db.TrxTmsEmpPcDtKpsFltAct
        .Where(e => e.Id == id && !e.Flag)
        .FirstOrDefaultAsync() ?? throw new Exception("Record Not Found");

        actualTrafficOut.TimeReturn = new TimeOnly(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        actualTrafficOut.Flag = true;

        await db.SaveChangesAsync();

        await SetEmpRecord(db, userAuth, actualTrafficOut, "I");

        return TypedResults.Ok();
    }

    public static async Task SetEmpRecord(PtcContext db, UserAuth userAuth, TrxTmsEmpPcDtKpFltAct attndnc, string type)
    {
        var record = new EmpScanRecord
        {
            Username = userAuth.UserName,
            LoginId = userAuth.LoginId,
            TransactionId = attndnc.Id,
            Type = type,
            ScanTime = DateTime.Now,
        };
        db.EmpScanRecord.Add(record);
        await db.SaveChangesAsync();
    }
}