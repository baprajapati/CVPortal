using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CVPortal.ViewModels
{
    public class VendorCustomerLoginViewModel
    {
        public int? Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string OTP { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}