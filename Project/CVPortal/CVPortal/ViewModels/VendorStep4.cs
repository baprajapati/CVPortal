using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CVPortal.ViewModels
{
    public class VendorStep4
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Please select type of vendor")]
        public string Type_of_Vend { get; set; }

        public HttpPostedFileBase AuditedFile { get; set; }
        public string AuditedFileName { get; set; }
        public HttpPostedFileBase MOAFile { get; set; }
        public string MOAFileName { get; set; }
        public bool IsMain { get; set; }
        public bool IsExistingUpdate { get; set; }
        public bool IsApprover { get; set; }
    }
}