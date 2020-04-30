using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace HN.Scheduler.Application.Jobs
{
    public class ExecuteJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var container = context.JobDetail.JobDataMap.GetString("Business");
            var ExDate = TimeZoneInfo.ConvertTime(context.FireTimeUtc, TimeZoneInfo.Local);
            return Task.Run(() => {

                Console.WriteLine($"{container}, 当前执行时间：{ExDate}");
            });
        }
    }
}
