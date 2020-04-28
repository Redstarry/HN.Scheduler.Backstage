using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using HN.Scheduler.Model.FarmerScheduler;

namespace HN.Scheduler.Application
{
    public class VerificationAddSchedulerData:AbstractValidator<CreateScheduler>
    {
        public VerificationAddSchedulerData()
        {
            //RuleFor(p => p.Name).NotEmpty().Must(JudgeName).WithMessage("汉语名字不能超过5个字，英文名字不能超过10个字母");
            //RuleFor(p => p.Phone).NotEmpty().WithMessage("电话号码为空").Must(judgePhone).WithMessage("电话号码长度必须是11位且格式正确");
            //RuleFor(p => p.IdCard).NotEmpty().Must(JudgeIdCardnumber).WithMessage("身份证号位必须是15位或18位或格式不正确");
            RuleFor(p => p.Name).NotEmpty().WithMessage("调度任务的名称不能为空");
            RuleFor(p => p.TaskType).Must(value => value != "OneOff" || value != "TimedTask").WithMessage("任务类型只能是'OneOff'和'TimedTask'");
        }
        public bool VerificationName(CreateScheduler createScheduler, string arg)
        {
            return true;
        }
    }
}
