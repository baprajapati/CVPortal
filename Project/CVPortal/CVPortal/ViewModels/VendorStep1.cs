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

        [Required(ErrorMessage = "Please select country")]
        public int? Address1CountryId { get; set; }

        [Required(ErrorMessage = "Please select state")]
        public int? Address1StateId { get; set; }

        [Required(ErrorMessage = "Please select city")]
        public int? Address1CityId { get; set; }

        [Required(ErrorMessage = "Please enter address (PO/Supplies/Mfg/Trd.)")]
        public string Address2 { get; set; }

        [Required(ErrorMessage = "Please select country")]
        public int? Address2CountryId { get; set; }

        [Required(ErrorMessage = "Please select state")]
        public int? Address2StateId { get; set; }

        [Required(ErrorMessage = "Please select city")]
        public int? Address2CityId { get; set; }
        public bool IsMain { get; set; }
    }
}