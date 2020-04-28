using System;
using System.Collections.Generic;
using System.Text;

namespace HN.Scheduler.Repository.Petapoco
{
    public static class DatabaseConfigure
    {
        public static string DBConnec { get; private set; } = "server = .;database = TaskInfo;uid = sa; pwd = 123";
        public static string DBProvider { get; private set; } = "System.Data.SqlClient";
    }
}
