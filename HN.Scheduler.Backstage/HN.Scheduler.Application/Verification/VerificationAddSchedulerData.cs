using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using HN.Scheduler.Model.FarmerScheduler;
using HN.Scheduler.Repository.Petapoco;
using PetaPoco;

namespace HN.Scheduler.Application
{
    public class VerificationAddSchedulerData:AbstractValidator<CreateScheduler>
    {
        private Database _database;
        public VerificationAddSchedulerData()
        {
            _database = new Database(DatabaseConfigure.DBConnec, DatabaseConfigure.DBProvider, null);
            RuleFor(p => p.Name).Custom((value, context) =>
            {
                if (string.IsNullOrEmpty(value))
                {
                    context.AddFailure("任务名称不能为空");
                }
                else
                {
                    var NameIsExists = _database.SingleOrDefault<DataBaseScheduler>("where Name = @0", value);
                    if (NameIsExists != null) context.AddFailure("任务名称已存在，无法添加");
                }

            });
            RuleFor(p => p.TaskType).Must(value => value == "OneOff" || value == "TimedTask").WithMessage("任务类型只能是'OneOff'和'TimedTask'");
            RuleFor(p => p.BusinessType).NotEmpty().WithMessage("业务类型不能为空");
            RuleFor(p => p.PresetTime).Must(value=> {
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
                try
                {
                    
                    if (!string.IsNullOrEmpty(value))
                    {
                        var SplitInterval = value.Split(":");
                        if (SplitInterval.Length != 4) context.AddFailure("间隔时间的长度错误");
                        else if (Convert.ToInt32(SplitInterval[0]) < 0) context.AddFailure("间隔时间的天数不能小于0");
                        else if (Convert.ToInt32(SplitInterval[1]) > 23 || Convert.ToInt32(SplitInterval[1]) < 0) context.AddFailure("间隔时间的小时数不能大于23或小于0");
                        else if (Convert.ToInt32(SplitInterval[2]) > 59 || Convert.ToInt32(SplitInterval[2]) < 0) context.AddFailure("间隔时间的分钟数不能大于59或者小于0");
                        else if (Convert.ToInt32(SplitInterval[3]) > 59 || Convert.ToInt32(SplitInterval[3]) < 0) context.AddFailure("间隔时间的秒数不能大于59或者小于0");
                    }
                    
                    //else context.AddFailure("间隔时间的格式错误，请检查！！！");
                }
                catch (Exception ex)
                {

                    context.AddFailure(ex.Message);
                }
            });
            
        }
    }
}
