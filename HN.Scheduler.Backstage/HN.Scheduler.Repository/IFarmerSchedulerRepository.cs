using HN.Scheduler.Model;
using HN.Scheduler.Model.FarmerScheduler;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HN.Scheduler.Repository
{
    public interface IFarmerSchedulerRepository
    {
        Task<ResponseData> AddScheduler(CreateScheduler createScheduler);
        Task<ResponseData> DeleteScheduler(string id);
        Task<ResponseData> GetSchedulerItmes(Page page);
        Task<ResponseData> GetConditionQuery(SelectScheduler selectScheduler);
        Task<ResponseData> UpDataScheduler(string id, UpdateScheduler updateScheduler);
    }
}
