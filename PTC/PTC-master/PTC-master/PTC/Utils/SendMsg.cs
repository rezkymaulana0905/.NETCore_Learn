using Microsoft.EntityFrameworkCore;
using PTC.Data;
using PTC.Models;
using Quartz;
namespace PTC.Utils;

public class EmailSenderAuto(IServiceProvider serviceProvider, IEmailSender emailSender) : IJob
{
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task Execute(IJobExecutionContext context)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);

        using var scope = _serviceProvider.CreateScope();
        var scopedServiceProvider = scope.ServiceProvider;
        var dbContext = scopedServiceProvider.GetRequiredService<PtcContext>();

        var receiver = await dbContext.TrxTmsEmpPcDtKpsFltAct
            .Where(x => x.Flag == false && (x.TimereasonId == "KP" || x.TimereasonId == "DL"))
            .ToListAsync();

        foreach (var x in receiver)
        {
            await MailProperties(dbContext, x);
        }
    }

    private async Task MailProperties(PtcContext dbContext, TrxTmsEmpPcDtKpFltAct? x)
    {
        var recipient = await dbContext.DdedmEmployees
                                .Where(e => e.EmpId == x.EmpId)
                                .FirstOrDefaultAsync();

        if (recipient != null)
        {
            string recipientEmail = recipient.Email ?? recipient.PrivateEmail ?? "";
            string subject = "Alert";
            string message = "You haven't scanned enter yet. This is an auto-send message.";

            await _emailSender.SendEmailEmp(recipientEmail, subject, message);
        }
    }
}