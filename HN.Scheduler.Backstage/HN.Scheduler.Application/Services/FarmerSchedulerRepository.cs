using HN.Scheduler.Repository.Petapoco;
using System;
using System.Collections.Generic;
using System.Text;
using PetaPoco;
using System.Threading.Tasks;
using HN.Scheduler.Model;
using HN.Scheduler.Model.FarmerScheduler;
using HN.Scheduler.Application;
using AutoMapper;
using HN.Scheduler.Application.Jobs;
using Quartz;
using HN.Scheduler.Application.AuxiliaryMethod;
using HN.Scheduler.Application.Verification;

namespace HN.Scheduler.Repository
{
    public class FarmerSchedulerRepository:IFarmerSchedulerRepository
    {
        private  Database _Database;
        private  AutoMapper.IMapper _mapper;
        VerificationAddSchedulerData _verificationAddSchedulerData;
        VerificationSelectSchedulerData _verificationSelectSchedulerData;
        VerificationUpdateSchedulerData _verificationUpdateSchedulerData;
        JobDetailAndTrigger _jobDetailAndTrigger;
        private readonly ISchedulerFactory _schedulerFactory;
        private IScheduler scheduler;
        public FarmerSchedulerRepository(AutoMapper.IMapper mapper, ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory ?? throw new ArgumentNullException(nameof(schedulerFactory));
            _mapper = mapper;
            _Database = new Database( DatabaseConfigure.DBConnec, DatabaseConfigure.DBProvider, null);
            _verificationAddSchedulerData = new VerificationAddSchedulerData();
            _verificationSelectSchedulerData = new VerificationSelectSchedulerData();
            _verificationUpdateSchedulerData = new VerificationUpdateSchedulerData();
            _jobDetailAndTrigger = new JobDetailAndTrigger(schedulerFactory);
            scheduler = _schedulerFactory.GetScheduler().Result;
        }

        public async Task<ResponseData> AddScheduler(CreateScheduler createScheduler)
        {
            var Result = _verificationAddSchedulerData.Validate(createScheduler);
            if (!Result.IsValid)
            {
                return new ResponseData(Result.ToString(), "", StatusCode.Fail);
            }
            var OperationSqlData = _mapper.Map<DataBaseScheduler>(createScheduler);
            bool result = await _jobDetailAndTrigger.AddSchedulerType(OperationSqlData);
            if (!result) return new ResponseData("任务创建失败", "", StatusCode.Fail);
            //var Trigger = await scheduler.GetTrigger(new TriggerKey(OperationSqlData.Name, "defalut"));
            //获取任务的触发器
            var Trigger = await scheduler.GetTrigger(new TriggerKey(OperationSqlData.Name));
            OperationSqlData.IsExists = 1;
            OperationSqlData.ExecuteReuslt = 0;
            OperationSqlData.CreateTime = DateTime.Now.ToString("F");
            // 获取任务的执行时间
            OperationSqlData.PresetTime = TimeZoneInfo.ConvertTime((DateTimeOffset)Trigger.GetNextFireTimeUtc(), TimeZoneInfo.Local).ToString("F");
            await _Database.InsertAsync(OperationSqlData);
            return new ResponseData("新增成功", OperationSqlData, StatusCode.Success);
        }

        public async Task<ResponseData> DeleteScheduler(string id)
        {
            var selectData = _Database.SingleOrDefault<DataBaseScheduler>("where id = @0", id);
            if (selectData == null)
            {
                return new ResponseData("删除的数据不存在", "", StatusCode.Fail);
            }
            JobKey jobKey = new JobKey(selectData.Name);
            await scheduler.DeleteJob(jobKey);
            await _Database.UpdateAsync<DataBaseScheduler>("set IsExists = 0 where Id = @0", id);
            return new ResponseData("删除任务成功", "", StatusCode.Success);
        }

        public async Task<ResponseData> GetSchedulerItmes(Page page)
        {
            var Data = await _Database.PageAsync<DataBaseScheduler>(page.PageNumber, page.PageSize, "select * from Tasks where IsExists=1");
            return new ResponseData("查询成功", Data, StatusCode.Success);
        }

        public async Task<ResponseData> GetConditionQuery(SelectScheduler selectScheduler)
        {
            var Result = _verificationSelectSchedulerData.Validate(selectScheduler);
            if (!Result.IsValid)
            {
                return new ResponseData(Result.ToString(), "", StatusCode.Fail);
            }
            // 组装条件查询的SQL语句
            var sql = SqlStatement.SqlAssembly(selectScheduler);
            var ResultData = await _Database.PageAsync<DataBaseScheduler>(selectScheduler.PageNumber, selectScheduler.PageSize, sql);
            return new ResponseData("查询成功", ResultData, StatusCode.Success);
        }

        public async Task<ResponseData> UpDataScheduler(string id,UpdateScheduler updateScheduler)
        {
            var Result = _verificationUpdateSchedulerData.Validate(updateScheduler);
            ITrigger trigger;
            if (!Result.IsValid)
            {
                return new ResponseData(Result.ToString(), "", StatusCode.Fail);
            }
            var selectData = await _Database.SingleAsync<DataBaseScheduler>("where id = @0", id);
            if(string.IsNullOrEmpty(selectData.Interval) && selectData.ExecuteReuslt == 1)return new ResponseData("非循环任务不能进行修改","", StatusCode.Fail);
            //暂停任务
           await  scheduler.PauseJob(new JobKey(updateScheduler.Name));
            var job = await scheduler.GetJobDetail(new JobKey(updateScheduler.Name));
           await scheduler.UnscheduleJob(new TriggerKey(updateScheduler.Name)); // 取消触发器
            // 检查更改的数据是否有间隔时间
            if (string.IsNullOrEmpty(updateScheduler.Interval))
            {
                trigger = TriggerBuilder.Create()
                    .WithIdentity(updateScheduler.Name)
                    .StartAt(Convert.ToDateTime(updateScheduler.PresetTime))
                .Build();
            }
            else
            {
                var arryDate = ConversionTime.ChangeInterval(updateScheduler.Interval);
                TimeSpan timeSpan = new TimeSpan(arryDate[0], arryDate[1], arryDate[2], arryDate[3]);
                trigger = TriggerBuilder.Create()
                    .WithIdentity(updateScheduler.Name)
                    .WithSimpleSchedule(x => x.WithInterval(timeSpan).RepeatForever())
                    .StartAt(Convert.ToDateTime(updateScheduler.PresetTime))
                .Build();
            }
            
            // 把任务和触发器绑定
            await scheduler.ScheduleJob(job, trigger);
           
            Sql sql = new Sql();
            sql.Set("PresetTime = @0, Interval = @1", updateScheduler.PresetTime, updateScheduler.Interval)
                .Where("id = @0", id);

            if(await _Database.UpdateAsync<DataBaseScheduler>(sql) < 0) return new ResponseData("修改失败", "", StatusCode.Fail);
            // 重启任务
            await scheduler.ResumeJob(new JobKey(updateScheduler.Name));
            return new ResponseData("修改成功","",StatusCode.Success);
        }
    }
}
