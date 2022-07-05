using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CVPortal.ViewModels
{
    public class CustomerStep1
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select organization status")]
        public string Org_Sts { get; set; }

        [Required(ErrorMessage = "Please enter customer name")]
        public string Cust_name { get; set; }

        [Required(ErrorMessage = "Please enter CEO name")]
        public string CEO_name { get; set; }

        [Required(ErrorMessage = "Please enter designation")]
        public string CEO_Designation { get; set; }

        [Required(ErrorMessage = "Please enter contact no")]
        public string CEO_Contact_no { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter address")]
        public string Dlr_Address { get; set; }

        [Required(ErrorMessage = "Country required")]
        public string Dlr_Add_Country { get; set; }

        [Required(ErrorMessage = "State required")]
        public string Dlr_Add_State { get; set; }

        [Required(ErrorMessage = "City required")]
        public string Dlr_Add_City { get; set; }

        [Required(ErrorMessage = "Please enter pincode")]
        public string Dlr_Add_Pincode { get; set; }

        [Required(ErrorMessage = "Please enter address (PO/Supplies/Mfg/Trd.)")]
        public string Supp_Address { get; set; }

        [Required(ErrorMessage = "Country required")]
        public string Supp_Add_Country { get; set; }

        [Required(ErrorMessage = "State required")]
        public string Supp_Add_State { get; set; }

        [Required(ErrorMessage = "City required")]
        public string Supp_Add_City { get; set; }

        [Required(ErrorMessage = "Please enter pincode")]
        public string Supp_Add_Pincode { get; set; }

        [Required(ErrorMessage = "Please enter contact no")]
        public string Contact_no { get; set; }
        public bool IsMain { get; set; }
        public string Status { get; set; }
    }
}