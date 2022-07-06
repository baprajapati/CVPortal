using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CVPortal.ViewModels
{
    public class CustomerListModel
    {
        public int Id { get; set; }
        public string Cust_name { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string CustomerCode { get; set; }
        public string Owner { get; set; }
        public string NextApprover { get; set; }
        public string InitiatorDepartment { get; set; }
        public string HODDepartment { get; set; }
        public string LegalDepartment { get; set; }
        public string FinanceDepartment { get; set; }
        public string ITDepartment { get; set; }
        public bool Step4 { get; set; }
    }
}