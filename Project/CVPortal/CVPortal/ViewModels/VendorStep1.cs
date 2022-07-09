using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CVPortal.ViewModels
{
    public class VendorStep1
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select organization status")]
        public string Org_Sts { get; set; }

        [Required(ErrorMessage = "Please enter vendor name")]
        public string vend_name { get; set; }

        [Required(ErrorMessage = "Please enter CEO name")]
        public string CEO_name { get; set; }

        [Required(ErrorMessage = "Please enter designation")]
        public string Designation { get; set; }

        [Required(ErrorMessage = "Please enter contact no")]
        public string Contact_no { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter address (Registered / Corporate Office)")]
        public string Address1 { get; set; }

        [Required(ErrorMessage = "Country required")]
        public string Address1Country { get; set; }

        [Required(ErrorMessage = "State required")]
        public string Address1State { get; set; }

        [Required(ErrorMessage = "State code required")]
        public string Address1StateCode { get; set; }

        [Required(ErrorMessage = "City required")]
        public string Address1City { get; set; }

        [Required(ErrorMessage = "Please enter pincode")]
        public string Address1Pincode { get; set; }

        public bool IsSameAsAddress1 { get; set; }

        [Required(ErrorMessage = "Please enter address (PO/Supplies/Mfg/Trd.)")]
        public string Address2 { get; set; }

        [Required(ErrorMessage = "Country required")]
        public string Address2Country { get; set; }

        [Required(ErrorMessage = "State code required")]
        public string Address2StateCode { get; set; }

        [Required(ErrorMessage = "State required")]
        public string Address2State { get; set; }

        [Required(ErrorMessage = "City required")]
        public string Address2City { get; set; }

        [Required(ErrorMessage = "Please enter pincode")]
        public string Address2Pincode { get; set; }

        public bool IsMain { get; set; }
        public bool IsExistingUpdate { get; set; }
        public string Status { get; set; }
    }
}