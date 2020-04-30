﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HN.Scheduler.Model
{
    public class Page
    {
        private const int MaxPageSize = 20;
        public int PageNumber { get; set; } = 1;
        private int _PageSize = 5;

        public int PageSize
        {
            get => _PageSize;
            set => _PageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
