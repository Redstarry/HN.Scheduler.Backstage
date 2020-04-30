using HN.Scheduler.Model.FarmerScheduler;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Text;

namespace HN.Scheduler.Application.AuxiliaryMethod
{
    public static class SqlStatement
    {
        private static string PeriodOfTimeQuery(string queryField, string startTime, string endTIme)
        {
            string message = "";
            if (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTIme))
            {
                message = $"and {queryField} between '{startTime}' and '{endTIme}'";
            }
            else if (!string.IsNullOrEmpty(startTime))
            {
                message = $"and {queryField} > '{startTime}'";
            }
            else if (!string.IsNullOrEmpty(endTIme))
            {
                message = $"and {queryField} < '{endTIme}'";
            }

            return message;
        }
        public static Sql SqlAssembly(SelectScheduler selectScheduler)
        {
            Sql selectSql = new Sql();
            selectSql.Append("select * from Tasks where IsExists=1 ");
            if (selectScheduler != null)
            {
                //任务名字不为空
                if (!string.IsNullOrEmpty(selectScheduler.Name))
                {
                    selectSql.Append("and Name like @0", "%" + selectScheduler.Name + "%");
                }
                //任务类型不为空
                if (!string.IsNullOrEmpty(selectScheduler.TaskType))
                {
                    if (selectScheduler.TaskType == "all")
                    {
                        selectSql.Append("and (TaskType = @0 or  TaskType = @1)", "OneOff", "TimedTask");
                    }
                    else
                    {
                        selectSql.Append("and TaskType like @0", "%" + selectScheduler.TaskType + "%");
                    }

                }
                //业务类型不为空
                if (!string.IsNullOrEmpty(selectScheduler.BusinessType))
                {
                    selectSql.Append("and BusinessType like @0", "%" + selectScheduler.BusinessType + "%");
                }

            }

            //完成状态不为空
            if (!string.IsNullOrEmpty(selectScheduler.ExecuteReuslt))
            {
                if (Convert.ToInt32(selectScheduler.ExecuteReuslt) == 0 || Convert.ToInt32(selectScheduler.ExecuteReuslt) == 1)
                {
                    selectSql.Append("and ExecuteReuslt like @0", selectScheduler.ExecuteReuslt);
                }
                else
                {
                    selectSql.Append("and (ExecuteReuslt like 0 or ExecuteReuslt like 1)");
                }

            }
            //创建时间
            selectSql.Append(PeriodOfTimeQuery("CreateTime", selectScheduler.CreatTimeStart, selectScheduler.CreatTimeEnd));
            //预定时间
            selectSql.Append(PeriodOfTimeQuery("PresetTime", selectScheduler.TaskPresetTimeStart, selectScheduler.TaskPresetTimeEnd));
            //执行时间
            selectSql.Append(PeriodOfTimeQuery("LastExecuteTime", selectScheduler.TaskLastExecuteTimeStart, selectScheduler.TaskLastExecuteTimeEnd));
            return selectSql;
        }
    }
}
