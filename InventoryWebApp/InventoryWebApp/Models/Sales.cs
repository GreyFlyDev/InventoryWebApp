using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryWebApp.Models
{
    public class Sales
    {
        public int SalesId { get; set; }
        public int QuantitySold { get; set; }

        public int ProductId { get; set; }
        public string UserId { get; set; }
    }
}