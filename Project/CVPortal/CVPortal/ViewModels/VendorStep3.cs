using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CVPortal.ViewModels
{
    public class VendorStep3
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter benificiary name")]
        public string Benificiary_name { get; set; }

        [Required(ErrorMessage = "Please enter bank name")]
        public string Bank_name { get; set; }

        public HttpPostedFileBase BankFile { get; set; }
        [Required(ErrorMessage = "Please upload bank related file")]
        public string BankFileName { get; set; }

        [Required(ErrorMessage = "Please enter branch name/address")]
        public string Branch_name_Add { get; set; }

        [Required(ErrorMessage = "Please enter account no")]
        public string Account_no { get; set; }

        public decimal? MICR_code { get; set; }

        [Required(ErrorMessage = "Please enter IFSC/RTGS code")]
        public string IFSC_RTGS_code { get; set; }

        [Required(ErrorMessage = "Please enter date")]
        public decimal? Date { get; set; }
        public bool IsMain { get; set; }
        public bool IsExistingUpdate { get; set; }
    }
}