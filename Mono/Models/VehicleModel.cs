﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mono.Models
{
    public class VehicleModel
    {
        public long id { get; set; }
        public string name { get; set; }
        public string abrv { get; set; }

        public VehicleMake vehicleMake { get; set; }

    }
}
