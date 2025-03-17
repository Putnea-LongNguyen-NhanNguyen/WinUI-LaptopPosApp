﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.Model
{
    public class Product
    {
        public required string ID { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public Manufacturer? Manufacturer { get; set; }
        public Category? Category { get; set; }
    }
}
