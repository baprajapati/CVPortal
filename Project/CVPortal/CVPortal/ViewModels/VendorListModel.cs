﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CVPortal.ViewModels
{
    public class VendorListModel
    {
        public int Id { get; set; }
        public string vend_name { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string RejectedReason { get; set; }
        public string VendorCode { get; set; }
        public string NewExistingVendor { get; set; }
        public string Owner { get; set; }
        public string Documents { get; set; }
        public string NextApprover { get; set; }
        public string PreviousApprover { get; set; }
        public string LegalDepartment { get; set; }
        public string FinanceDepartment { get; set; }
        public string ITDepartment { get; set; }
        public bool Step4 { get; set; }
        public bool IsEnableAccess { get; set; }
    }
}