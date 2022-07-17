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

        public string Swift_Code { get; set; }

        [Required(ErrorMessage = "Please enter ITS return status")]
        public string ITR_ReturnSts { get; set; }

        [Required(ErrorMessage = "Please select ITR return turnover status")]
        public bool? ITR_ReturnStsTurnover { get; set; }

        [Required(ErrorMessage = "Please select ITR return TDS deduct status")]
        public bool? ITR_ReturnTDSDeduct { get; set; }

        public HttpPostedFileBase WealthCapitalCertificateFile { get; set; }
        public string WealthCapitalCertificateFileName { get; set; }

        public HttpPostedFileBase SolvancyCertificateFile { get; set; }
        public string SolvancyCertificateFileName { get; set; }

        public HttpPostedFileBase InvestmentDeclarationFile { get; set; }
        public string InvestmentDeclarationFileName { get; set; }

        public bool IsMain { get; set; }
        public bool IsApprover { get; set; }
        public bool IsCreatedUser { get; set; }
    }
}