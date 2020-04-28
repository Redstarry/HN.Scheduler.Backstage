using HN.Scheduler.Repository.Petapoco;
using System;
using System.Collections.Generic;
using System.Text;
using PetaPoco;

namespace HN.Scheduler.Repository
{
    public class FarmerSchedulerRepository:IFarmerSchedulerRepository
    {
        private  Database _Database;
        public FarmerSchedulerRepository()
        {

            _Database = new Database( DatabaseConfigure.DBConnec, DatabaseConfigure.DBProvider, null);
        }
    }
}
