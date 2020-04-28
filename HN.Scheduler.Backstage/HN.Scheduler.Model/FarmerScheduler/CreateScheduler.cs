using System;
using System.Collections.Generic;
using System.Text;
using PetaPoco;

namespace HN.Scheduler.Model.FarmerScheduler
{
    [TableName("Tasks")]
    [PrimaryKey("Id", AutoIncrement = true)]
    public class CreateScheduler
    {
        /// <summary>
        /// 任务编号
        /// </summary>

        public int Id { get; set; }
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
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 预定时间
        /// </summary>
        public string PresetTime { get; set; }
        /// <summary>
        /// 时间间隔
        /// </summary>
        public string Interval { get; set; }
        /// <summary>
        /// 最后执行时间
        /// </summary>
        public string LastExecuteTime { get; set; }
        /// <summary>
        /// 执行结果 0 是False 1是True
        /// </summary>
        public int ExecuteReuslt { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }
        /// <summary>
        /// 是否存在
        /// </summary>
        public int Isexists { get; set; }
    }
}
