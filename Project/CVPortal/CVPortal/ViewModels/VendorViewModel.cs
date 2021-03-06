using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CVPortal.ViewModels
{
    public class VendorViewModel
    {
        public bool IsNewVendor { get; set; } = true;

        [Required(ErrorMessage = "Please enter vendor name")]
        public string vend_name { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        public string VendorCode { get; set; }

        public string ExistingReason { get; set; }
    }
}