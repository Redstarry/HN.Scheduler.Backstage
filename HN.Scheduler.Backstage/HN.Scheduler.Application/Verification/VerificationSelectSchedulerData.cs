using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using HN.Scheduler.Model.FarmerScheduler;

namespace HN.Scheduler.Application.Verification
{
    public class VerificationSelectSchedulerData:AbstractValidator<SelectScheduler>
    {
        public VerificationSelectSchedulerData()
        {
            RuleFor(p => p.TaskType).Must(value => value == "OneOff" || value == "TimedTask" || value=="all").WithMessage("任务类型只能是'OneOff'和'TimedTask'");
            RuleFor(p => p.ExecuteReuslt).Must(value => value == "1" || value == "0" || value == "2").WithMessage("输入的值不在范围中");
        }

    }
}
