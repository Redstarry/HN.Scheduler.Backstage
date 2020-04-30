using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using HN.Scheduler.Model.FarmerScheduler;

namespace HN.Scheduler.Application.MapperData
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateScheduler,DataBaseScheduler>();
            CreateMap<DataBaseScheduler,CreateScheduler>();
        }
    }
}
