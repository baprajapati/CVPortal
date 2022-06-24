using System;
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
        public string VendorCode { get; set; }
        public string NewExistingVendor { get; set; }
    }
}