﻿using System;

namespace VPMDataManager.Library.Models
{
    public class SaleReportModel
    {
        public DateTime SaleDate { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string EmailAddress { get; set; }
    }
}
