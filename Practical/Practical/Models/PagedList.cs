﻿using System;
using System.Collections.Generic;

namespace Practical.Models
{
    public class PagedList<T>
    {
        public List<T> Content { get; set; }

        public int CurrentPage { get; set; }
        
        public int PageSize { get; set; }
        
        public int TotalRecords { get; set; }

        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalRecords / PageSize); }
        }
    }
}
