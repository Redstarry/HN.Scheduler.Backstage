using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HN.Scheduler.Application;
using HN.Scheduler.Model;
using HN.Scheduler.Model.FarmerScheduler;
using HN.Scheduler.Repository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HN.Scheduler.Controllers
{
    [EnableCors("Domain")]
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
        public async Task<IActionResult> GetSchedulerItems([FromQuery] Page page)
        {
            var Message = await _farmerSchedulerRepository.GetSchedulerItmes(page);
            return Ok(Message);
        }
        /// <summary>
        ///  根据条件进行查询数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("ConditionQuery")]
        public async Task<IActionResult> GetScheduler(SelectScheduler selectScheduler)
        {
            var Message = await _farmerSchedulerRepository.GetConditionQuery(selectScheduler);
            return Ok(Message);
        }
        /// <summary>
        ///  添加数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddScheduler(CreateScheduler createScheduler)
        {

           var Message =  await _farmerSchedulerRepository.AddScheduler(createScheduler);
            return Ok(Message);
        }
        /// <summary>
        ///  更新数据
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateScheduler(string id, [FromBody]UpdateScheduler updateScheduler)
        {
            var Message = await _farmerSchedulerRepository.UpDataScheduler(id, updateScheduler);
            return Ok(Message);
        }
        /// <summary>
        ///  删除数据
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScheduler(string id)
        {
            var Message = await _farmerSchedulerRepository.DeleteScheduler(id);
            return Ok(Message);
        }
    }
}