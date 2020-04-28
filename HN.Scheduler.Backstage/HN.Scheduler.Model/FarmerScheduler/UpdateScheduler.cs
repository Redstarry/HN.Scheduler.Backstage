﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HN.Scheduler.Model.FarmerScheduler
{
    public class UpdateScheduler
    {
        /// <summary>
        /// 任务名字
        /// </summary>
        public string Task_Name { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string Task_BusinessType { get; set; }
        /// <summary>
        /// 预定时间
        /// </summary>
        public string Task_PresetTime { get; set; }
        /// <summary>
        /// 间隔时间
        /// </summary>
        public string Task_Interval { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Task_Describe { get; set; }
    }
}
