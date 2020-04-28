using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HN.Scheduler.Application;
using HN.Scheduler.Model.FarmerScheduler;
using HN.Scheduler.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HN.Scheduler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulerController : ControllerBase
    {
        private readonly IFarmerSchedulerRepository _farmerSchedulerRepository;

        public SchedulerController(IFarmerSchedulerRepository farmerSchedulerRepository)
        {
            _farmerSchedulerRepository = farmerSchedulerRepository;
        }
        /// <summary>
        ///  查询全部数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<IActionResult> GetSchedulerItems()
        {
            throw new Exception();
        }
        /// <summary>
        ///  根据条件进行查询数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("ConditionQuery")]
        public Task<IActionResult> GetScheduler(SelectScheduler selectScheduler)
        {
            throw new Exception();
        }
        /// <summary>
        ///  添加数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Task<IActionResult> AddScheduler(CreateScheduler createScheduler)
        {
            
            throw new Exception();
        }
        /// <summary>
        ///  更新数据
        /// </summary>
        /// <returns></returns>
        [HttpPut("id")]
        public Task<IActionResult> UpdateScheduler(string id, [FromBody]UpdateScheduler updateScheduler)
        {
            throw new Exception();
        }
        /// <summary>
        ///  删除数据
        /// </summary>
        /// <returns></returns>
        [HttpDelete("id")]
        public Task<IActionResult> DeleteScheduler(string id)
        {
            throw new Exception();
        }
    }
}