using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using HN.Scheduler.Model.FarmerScheduler;
using HN.Scheduler.Repository.Petapoco;
using PetaPoco;

namespace HN.Scheduler.Application.Verification
{
    public class VerificationUpdateSchedulerData:AbstractValidator<UpdateScheduler>
    {
        private Database _database;
        public VerificationUpdateSchedulerData()
        {
            _database = new Database(DatabaseConfigure.DBConnec, DatabaseConfigure.DBProvider, null);
            //RuleFor(p => p.Name).NotEmpty().WithMessage("调度任务的名称不能为空");
            RuleFor(p => p.Name).Custom((value, context)=> {
                if (string.IsNullOrEmpty(value)) context.AddFailure("任务名称不能为空");
                var selectData = _database.SingleOrDefault<DataBaseScheduler>("where Name = @0", value);
                if (selectData.Name == null) context.AddFailure("任务名称不能进行修改");
            });
            RuleFor(p => p.TaskType).Must(value => value == "TimedTask").WithMessage("只能修改定时任务'");
            RuleFor(p => p.BusinessType).NotEmpty().WithMessage("业务类型不能为空");
            RuleFor(p => p.PresetTime).Must(value => {
                var NowDate = DateTime.Now;
                try
                {
                    var InputDate = Convert.ToDateTime(value);
                    return NowDate < InputDate;
                }
                catch
                {

                    return false;
                }
            }).WithMessage("执行时间不能小于当前时间或输入的时间格式不对");
            RuleFor(p => p.Interval).Custom((value, context) =>
            {
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        var SplitInterval = value.Split(":");
                        if (SplitInterval.Length != 4) context.AddFailure("间隔时间的长度错误");
                        else if (Convert.ToInt32(SplitInterval[0]) < 0) context.AddFailure("间隔时间的天数不能小于0");
                        else if (Convert.ToInt32(SplitInterval[1]) > 23 || Convert.ToInt32(SplitInterval[1]) < 0) context.AddFailure("间隔时间的小时数不能大于23或小于0");
                        else if (Convert.ToInt32(SplitInterval[2]) > 59 || Convert.ToInt32(SplitInterval[2]) < 0) context.AddFailure("间隔时间的分钟数不能大于59或者小于0");
                        else if (Convert.ToInt32(SplitInterval[3]) > 59 || Convert.ToInt32(SplitInterval[3]) < 0) context.AddFailure("间隔时间的秒数不能大于59或者小于0");

                    }
                    catch (Exception ex)
                    {

                        context.AddFailure(ex.Message);
                    }
                }
                
            });
        }
    }
}
