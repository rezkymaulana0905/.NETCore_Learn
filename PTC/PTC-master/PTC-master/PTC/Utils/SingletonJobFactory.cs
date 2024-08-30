﻿using Quartz;
using Quartz.Spi;

namespace PTC.Utils;

public class SingletonJobFactory : IJobFactory
{
    private readonly IServiceProvider _serviceProvider;

    public SingletonJobFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        return _serviceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
    }

    public void ReturnJob(IJob job)
    {
        // Perform any cleanup if needed
    }
}
