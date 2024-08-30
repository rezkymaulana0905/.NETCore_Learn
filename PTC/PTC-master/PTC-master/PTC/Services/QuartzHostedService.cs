using PTC.Utils;
using Quartz;
using Quartz.Spi;
namespace PTC.Services;

public class QuartzHostedService(ISchedulerFactory schedulerFactory,
                            IJobFactory jobFactory
                                ) : IHostedService
{
    private readonly ISchedulerFactory _schedulerFactory = schedulerFactory;
    private readonly IJobFactory _jobFactory = jobFactory;

    public IScheduler Scheduler { get; private set; }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
        Scheduler.JobFactory = _jobFactory;

        // Create a job detail
        var jobDetail = JobBuilder.Create<EmailSenderAuto>()
            .WithIdentity("SendMessageJob", "group1")
            .Build();

        // Create a trigger that fires every day at 23:59
        var trigger = TriggerBuilder.Create()
            .WithIdentity("SendMessageTrigger", "group1")
            .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(16, 00))
            .Build();

        // Schedule the job with the trigger
        await Scheduler.ScheduleJob(jobDetail, trigger, cancellationToken);

        // Start the scheduler
        await Scheduler.Start(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // Stop the scheduler when the application is stopped
        return Scheduler?.Shutdown(cancellationToken) ?? Task.CompletedTask;
    }
}
