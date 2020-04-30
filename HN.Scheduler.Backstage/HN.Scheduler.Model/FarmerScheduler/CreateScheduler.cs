using System;
using System.Collections.Generic;
using System.Text;
using PetaPoco;

namespace HN.Scheduler.Model.FarmerScheduler
{
    public class CreateScheduler:BasicScheduler
    { 
        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }
    }
}
