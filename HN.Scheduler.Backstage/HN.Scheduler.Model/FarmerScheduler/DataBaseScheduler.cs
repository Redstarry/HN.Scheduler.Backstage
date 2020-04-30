using PetaPoco;
using System;
using System.Collections.Generic;
using System.Text;

namespace HN.Scheduler.Model.FarmerScheduler
{
    [TableName("Tasks")]
    [PrimaryKey("Id", AutoIncrement = true)]
    public class DataBaseScheduler:BasicScheduler
    {
        /// <summary>
        /// 任务编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
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
        public int IsExists { get; set; }
    }
}
