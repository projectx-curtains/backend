﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curtains.Domain.Models
{
    public class SetModel : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; }
        public int? CurtainsId { get; set; }
        public int? PillowsId { get; set; }
        public int? BedspreadsId { get; set; }
        public decimal Price { get; set; }
        public virtual CurtainsModel Curtains { get; set; }
        public virtual PillowsModel Pillows { get; set; }
        public virtual BedspreadsModel Bedspreads { get; set; }
    }
}
