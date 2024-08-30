using Microsoft.EntityFrameworkCore;
using PTC.Data;
using PTC.Models;

namespace PTC.Services;

public class SpplierService
{
    public static async Task<object> GetSpplier(PtcContext db)
    {
        return await db.VhcSpplier
                        .Where(vhc => db.PsgSpplier
                        .Any(psg => psg.VhcId == vhc.Id && !psg.Flag))
                        .Select(vhc => new Spplier
                        {
                            VhcSpplier = vhc,
                            PsgSppliers = db.PsgSpplier
                                .Where(psg => psg.VhcId == vhc.Id && !psg.Flag)
                                .OrderBy(psg => psg.Id)
                                .ToArray()
                        })
                        .ToListAsync();
    }
    public static async Task<IResult> CreateSpplierData(Spplier add, UserAuth userAuth, PtcContext db)
    {
        db.VhcSpplier.Add(add.VhcSpplier);
        await db.SaveChangesAsync();
        foreach (PsgSpplier psg in add.PsgSppliers)
        {
            psg.VhcId = add.VhcSpplier.Id;
            db.PsgSpplier.Add(psg);
        }
        await db.SaveChangesAsync();

        await SetSpplierRecord(db, add.VhcSpplier, userAuth, "I");

        return TypedResults.Ok(add);
    }

    public static async Task<IResult> ConfirmSupplierOut(ConfirmVhc conf, UserAuth userAuth, PtcContext db)
    {
        var vhc = await db.VhcSpplier
                            .FirstOrDefaultAsync(m => m.Id == conf.VhcId) ?? throw new Exception("Supplier Data Not Found");


        vhc.Flag = true;
        vhc.ConfirmOutTime = DateTime.Now;

        if (conf.PsgId != null && conf.PsgId.Length != 0)
        {
            foreach (int id in conf.PsgId)
            {
                var psg = await db.PsgSpplier
                                  .FirstOrDefaultAsync(m => m.Id == id);

                if (psg != null)
                {
                    psg.Flag = true;
                    await db.SaveChangesAsync();
                }
            }
        }

        await db.SaveChangesAsync();

        await SetSpplierRecord(db, vhc, userAuth, "O");

        return TypedResults.Ok(conf);
    }

    public static async Task SetSpplierRecord(PtcContext db, VhcSpplier spplier, UserAuth userAuth, string type)
    {
        var record = new SpplierTransactionRecord
        {
            Username = userAuth.UserName,
            LoginId = userAuth.LoginId,
            TransactionId = spplier.Id,
            Type = type,
            ScanTime = DateTime.Now,
        };

        db.SpplierTransactionRecord.Add(record);
        await db.SaveChangesAsync();
    }
}
