using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CVPortal.ViewModels
{
    public class VendorStep2
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter designation")]
        public string AC_contact_Desig { get; set; }

        [Required(ErrorMessage = "Please enter contact name")]
        public string AC_contact_name { get; set; }

        [Required(ErrorMessage = "Please enter contact no")]
        public string AC_contact_Phno { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string AC_contact_Email { get; set; }

        [Required(ErrorMessage = "Please enter deignation")]
        public string Spy_contact_Desig { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        public string Spy_contact_name { get; set; }

        [Required(ErrorMessage = "Please enter contact no")]
        public string Spy_contact_Phno { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Spy_contact_Email { get; set; }

        public string CIN_No { get; set; }
        public HttpPostedFileBase CINFile { get; set; }
        public string CINFileName { get; set; }

        [Required(ErrorMessage = "Please enter pan no")]
        public string PAN_No { get; set; }

        public HttpPostedFileBase PANFile { get; set; }

        [Required(ErrorMessage = "Please upload pan file")]
        public string PANFileName { get; set; }

        [Required(ErrorMessage = "Please select type of vendor under GST")]
        public string Type_vend_gst { get; set; }
        public string GST_Reg_no { get; set; }
        public HttpPostedFileBase GSTFile { get; set; }
        public string GSTFileName { get; set; }

        [Required(ErrorMessage = "Please enter item description")]
        public string Item_Desc { get; set; }

        [Required(ErrorMessage = "Please enter HSN/SAC code")]
        public string HSN_SAC_code { get; set; }

        [Required(ErrorMessage = "Please enter MSME no")]
        public string MSME_no { get; set; }

        public HttpPostedFileBase MSMEFile { get; set; }

        [Required(ErrorMessage = "Please upload MSME file")]
        public string MSMEFileName { get; set; }

        [Required(ErrorMessage = "Please enter annual turnover")]
        public decimal? Annu_TurnOver { get; set; }

        [Required(ErrorMessage = "Please select nature of service")]
        public string Nature_of_service { get; set; }

        [Required(ErrorMessage = "Please enter financial year")]
        public int? FinancialYear1 { get; set; }

        [Required(ErrorMessage = "Please enter financial year")]
        public int? FinancialYear2 { get; set; }

        [Required(ErrorMessage = "Please select yes/no")]
        public bool? IsITRFiled1 { get; set; }

        [Required(ErrorMessage = "Please select yes/no")]
        public bool? IsITRFiled2 { get; set; }

        [Required(ErrorMessage = "Please enter acknowledge no")]
        public string AcknowledgeNo1 { get; set; }

        [Required(ErrorMessage = "Please enter acknowledge no")]
        public string AcknowledgeNo2 { get; set; }

        public string ITR_Field_dtl { get; set; }
        public bool IsMain { get; set; }
    }
}