using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HN.Scheduler.Application.AuxiliaryMethod;
using HN.Scheduler.Model.FarmerScheduler;
using Quartz;

namespace HN.Scheduler.Application.Jobs
{
    public class JobDetailAndTrigger
    {
        private ISchedulerFactory _schedulerFactory;
        private IScheduler scheduler;
        private ITrigger _trigger;
        public JobDetailAndTrigger(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory ?? throw new ArgumentNullException(nameof(schedulerFactory));
        }
        public async Task<bool> AddSchedulerType(DataBaseScheduler OperationSqlData)
        {
            scheduler =await _schedulerFactory.GetScheduler();
            await scheduler.Start();
            var jobDetail = JobBuilder.Create<ExecuteJob>()
                .WithIdentity(OperationSqlData.Name)
                .Build();
            switch (OperationSqlData.TaskType)
            {
                case "OneOff":
                    _trigger = TriggerBuilder.Create()
                        .StartNow()
                        .Build();
                    break;
                case "TimedTask":
                    if (string.IsNullOrEmpty(OperationSqlData.Interval))
                    {
                        _trigger = TriggerBuilder.Create()
                            .WithIdentity(OperationSqlData.Name)
                            .StartAt(Convert.ToDateTime(OperationSqlData.PresetTime))
                            .Build();
                    }
                    else
                    {
                        var arryDate = ConversionTime.ChangeInterval(OperationSqlData.Interval);
                        TimeSpan timeSpan = new TimeSpan(arryDate[0], arryDate[1], arryDate[2], arryDate[3]);
                        _trigger = TriggerBuilder.Create()
                            .WithIdentity(OperationSqlData.Name)
                            .WithSimpleSchedule(x => x.WithInterval(timeSpan).RepeatForever())
                            .StartAt(Convert.ToDateTime(OperationSqlData.PresetTime))
                            .Build();
                    }
                    break;
                default:
                    return false;
            }
            await scheduler.ScheduleJob(jobDetail, _trigger);
            scheduler.ListenerManager.AddJobListener(new JobListen());
            return true;
        }
    }
}
