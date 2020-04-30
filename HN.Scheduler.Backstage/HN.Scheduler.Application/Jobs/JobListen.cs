using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HN.Scheduler.Model.FarmerScheduler;
using HN.Scheduler.Repository.Petapoco;
using PetaPoco;
using Quartz;

namespace HN.Scheduler.Application.Jobs
{
    class JobListen : IJobListener
    {
        static object LockData = new object();
        private Database _database;
        public JobListen()
        {
            _database = new Database(DatabaseConfigure.DBConnec, DatabaseConfigure.DBProvider, null);
        }
        public string Name => GetType().Name;

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => {
                Console.WriteLine("任务不会执行");
            });
        }

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => {
                Console.WriteLine($"任务：{context.JobDetail.Key.Name} 即将执行");
            });
        }

        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
        {
            var ExDate = TimeZoneInfo.ConvertTime(context.FireTimeUtc, TimeZoneInfo.Local).ToString("F");
            var UTCNextDate = context.NextFireTimeUtc;
            var UTCProDate = context.PreviousFireTimeUtc;
            var nextDate = UTCNextDate != null ? TimeZoneInfo.ConvertTime((DateTimeOffset)context.NextFireTimeUtc, TimeZoneInfo.Local).ToString("F") : ExDate;
            var prevDate = UTCProDate != null ? TimeZoneInfo.ConvertTime((DateTimeOffset)context.PreviousFireTimeUtc, TimeZoneInfo.Local).ToString("F") : ExDate;
            Sql sql = Sql.Builder
                .Set("PresetTime = @0, LastExecuteTime = @1, ExecuteReuslt = 1", nextDate, ExDate)
                .Where("Name = @0", context.JobDetail.Key.Name);
            return Task.Run(() => {
                lock (LockData)
                {
                    _database.Update<DataBaseScheduler>(sql);
                    Console.WriteLine(context.Scheduler.GetTriggerState(new TriggerKey(context.Trigger.Key.Name)).Result);
                    Console.WriteLine($"任务：{context.JobDetail.Key.Name} 已执行， 数据已更新");
                }

            });
        }
    }
}
