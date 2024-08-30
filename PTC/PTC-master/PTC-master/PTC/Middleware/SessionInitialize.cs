using Microsoft.EntityFrameworkCore;
using PTC.Data;
namespace Middleware;

public class SessionInitializationMiddleware(RequestDelegate next, IList<string> excludedPaths)
{
    private readonly RequestDelegate _next = next;
    private readonly IList<string> _excludedPaths = excludedPaths;

    public async Task InvokeAsync(HttpContext context, PtcContext db)
    {
        var path = context.Request.Path.Value;

        // Check if the request path should be excluded
        if (_excludedPaths.Any(p => path.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
        {
            await _next(context);
            return;
        }

        if (!context.Session.Keys.Contains("_Id"))
        {
            //var form = context.Request.Form.Keys ?? throw new Exception("Null");
            //var key = form.FirstOrDefault();

            //if (key != null)
            //{
               //var data = DecodeFrom64(key);
               var data = "awan.akbar";

                var formdata = await db.SsoUsers
                    .Where(u => u.Username == data)
                    .Join(db.DdedmEmployees, sso => sso.EmpId, emp => emp.EmpId, (sso, emp) => new { emp.EmpId, emp.Name, sso.RoleId })
                    .FirstOrDefaultAsync() ?? throw new Exception("User Not Found");

                context.Session.SetString("_Id", formdata.EmpId);
                context.Session.SetString("_Name", formdata.Name);
                context.Session.SetString("_Level", formdata.RoleId.ToString());
           //}
        }

        await _next(context);   
    }

    private string DecodeFrom64(string data)
    {
        System.Text.UTF8Encoding encoder = new();
        System.Text.Decoder utf8Decode = encoder.GetDecoder();
        byte[] todecode_byte = Convert.FromBase64String(data);
        int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
        char[] decoded_char = new char[charCount];
        utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
        string result = new(decoded_char);
        return result;
    }
}
