﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryWebApp.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }

        public ICollection<Restock> Restocks { get; set; }
        public ICollection<Sales> Sales { get; set; }

        public string UserId { get; set; }
    }
}