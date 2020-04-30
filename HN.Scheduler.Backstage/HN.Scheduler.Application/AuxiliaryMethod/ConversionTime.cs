using System;
using System.Collections.Generic;
using System.Text;

namespace HN.Scheduler.Application.AuxiliaryMethod
{
    public static class ConversionTime
    {
        public static List<int> ChangeInterval(string Date)
        {
            List<int> TimeSpanDate = new List<int>();
            foreach (var item in Date.Split(":"))
            {
                TimeSpanDate.Add(Convert.ToInt32(item));
            }
            return TimeSpanDate;
        }
    }
}
