using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PTC.Data;
using PTC.Models;
using PTC.Utils;
namespace PTC.Services;

public class UserServices
{

    public static async Task<IResult> Login(PtcContext db, string username, string password, string address, IOptions<JwtSettings> jwtSettings)
    {
        var foundUser = await db.User.FirstOrDefaultAsync(u => u.Username == username)
            ?? throw new Exception("User Not Found");

        var isLocked = await db.UserLock.Where(u => u.UserId == foundUser.Id).SingleOrDefaultAsync() != null;

        if (!isLocked) {
            var attempt = await db.UserPasswordAttempt.Where(e => e.UserId == foundUser.Id).SingleOrDefaultAsync();

            if (foundUser.Password == Hasher.SHA512(password))
            {
                if (foundUser.Password != Hasher.SHA512("panasonic123"))
                {
                    var record = await SetUserRecord(db, foundUser, address);
                    var LoginToken = JWT.GenerateJwtToken(jwtSettings,foundUser, address, record);

                    if (attempt != null)
                    {
                        db.UserPasswordAttempt.Remove(attempt);
                        await db.SaveChangesAsync();
                    }
                    return Results.Ok(new { 
                        result = "Login",
                        token = LoginToken
                    });
                }
                var CpToken = JWT.GenerateJwtToken(jwtSettings, foundUser, address);
                return Results.Ok(new {
                    result = "ChangePassword",
                    token = CpToken
                });
            }

            if (attempt == null)
            {
                db.UserPasswordAttempt.Add(new UserPasswordAttempt { UserId = foundUser.Id, Attempt = 1 });
                await db.SaveChangesAsync();
            }
            else
            {
                attempt.Attempt++;

                if (attempt.Attempt >= 8)
                {
                    db.UserLock.Add(new UserLock { UserId = attempt.UserId, CreateDate = DateTime.Now });
                    await db.SaveChangesAsync();
                }

                await db.SaveChangesAsync();
            }

            throw new Exception("Wrong Password");
        }
        throw new Exception("Account is Locked");
    }

    public static async Task<IResult> ChangePassword(PtcContext db, string username, string newPassword, string address, IOptions<JwtSettings> jwtSettings)
    {
        var foundUser = await db.User.SingleOrDefaultAsync(u => u.Username == username)
            ?? throw new Exception("User not found");

        var passwordDump = await db.PasswordDump
            .Where(u => u.UserId == foundUser.Id)
            .OrderByDescending(u => u.CreateDate)
            .Take(3)
            .ToListAsync();

        string hashedNewPassword = Hasher.SHA512(newPassword);

        if (passwordDump.Any(dump => dump.Password == hashedNewPassword))
        {
            throw new Exception("New password cannot be the same as any of your last 3 passwords");
        }

        foundUser.Password = hashedNewPassword;

        db.PasswordDump.Add(new PasswordDump
        {
            UserId = foundUser.Id,
            Password = hashedNewPassword,
            CreateDate = DateTime.Now
        });

        if (passwordDump.Count == 3)
        {
            db.PasswordDump.Remove(passwordDump.Last());
        }

        await db.SaveChangesAsync();

        var record = await SetUserRecord(db, foundUser, address);
        var loginToken = JWT.GenerateJwtToken(jwtSettings, foundUser, address, record);

        return Results.Ok(new
        {
            result = "Login",
            token = loginToken
        });
    }

    public static async Task<IResult> Register(PtcContext db, string username)
    {
        var password = "panasonic123";
        var hashedpassword = Hasher.SHA512(password);

        User user = new()
        {
            Username = username,
            Password = hashedpassword
        };

        db.User.Add(user);

        await db.SaveChangesAsync();

        return Results.Ok();
    }

    public static async Task Reset(PtcContext db, int id)
    {
        var password = "panasonic123";

        var user = await db.User.Where(u => u.Id == id).SingleOrDefaultAsync() ?? throw new Exception("No User Found");

        user.Password = Hasher.SHA512(password);

        await db.SaveChangesAsync();
    }

    public static async Task Unlocked(PtcContext db, int id)
    {
        var locked = await db.UserLock.Where(l => l.UserId == id).SingleOrDefaultAsync() ?? throw new Exception("No Locked User Found");
        db.UserLock.Remove(locked);
        await db.SaveChangesAsync();

        await Reset(db, id);
    }

    private static async Task<UserRecord> SetUserRecord(PtcContext db, User foundUser, string address)
    {
        UserRecord record = new()
        {
            Username = foundUser.Username,
            Address = address,
            LoginTime = DateTime.Now,
        };

        db.UserRecord.Add(record);
        await db.SaveChangesAsync();

        return record;
    }
}