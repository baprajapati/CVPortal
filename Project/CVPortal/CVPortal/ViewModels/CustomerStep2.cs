using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CVPortal.ViewModels
{
    public class CustomerStep2
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter designation")]
        public string AC_contact_Desig { get; set; }

        [Required(ErrorMessage = "Please enter contact name")]
        public string AC_contact_name { get; set; }

        [Required(ErrorMessage = "Please enter contact no")]
        public string AC_contact_Phno { get; set; }

        public string AC_contact_Mob { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string AC_contact_Email { get; set; }

        public string CINNo_LLPNo { get; set; }
        public HttpPostedFileBase CINFile { get; set; }
        public string CINFileName { get; set; }

        [Required(ErrorMessage = "Please enter pan no")]
        public string PAN_No { get; set; }

        public HttpPostedFileBase PANFile { get; set; }
        public string PANFileName { get; set; }

        [Required(ErrorMessage = "Please select type of customer under GST")]
        public string Type_Cust_gst { get; set; }
        public string GST_Reg_no { get; set; }
        public HttpPostedFileBase GSTFile { get; set; }
        public string GSTFileName { get; set; }

        public bool IsMain { get; set; }
    }
}