using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using HN.Scheduler.Model.FarmerScheduler;

namespace HN.Scheduler.Application.Verification
{
    class VerificationSelectSchedulerData:AbstractValidator<SelectScheduler>
    {
    }
}
