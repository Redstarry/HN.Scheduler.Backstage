using System;
using System.Collections.Generic;
using System.Text;

namespace HN.Scheduler.Model.FarmerScheduler
{
    public class BasicScheduler
    {
        /// <summary>
        /// 任务名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 任务类型
        /// </summary>
        public string TaskType { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string BusinessType { get; set; }
        /// <summary>
        /// 预定时间
        /// </summary>
        public string PresetTime { get; set; }
        /// <summary>
        /// 时间间隔
        /// </summary>
        public string Interval { get; set; }
    }
}
