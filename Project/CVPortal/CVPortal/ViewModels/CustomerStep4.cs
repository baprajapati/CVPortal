using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CVPortal.ViewModels
{
    public class CustomerStep4
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter swift code")]
        public string Swift_Code { get; set; }

        [Required(ErrorMessage = "Please enter ITS return status")]
        public string ITR_ReturnSts { get; set; }

        [Required(ErrorMessage = "Please select ITR return turnover status")]
        public bool? ITR_ReturnStsTurnover { get; set; }

        [Required(ErrorMessage = "Please select ITR return TDS deduct status")]
        public bool? ITR_ReturnTDSDeduct { get; set; }

        public bool IsMain { get; set; }
        public bool IsApprover { get; set; }
        public bool IsCreatedUser { get; set; }
    }
}