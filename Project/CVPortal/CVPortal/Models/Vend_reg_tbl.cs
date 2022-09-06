//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CVPortal.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Vend_reg_tbl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vend_reg_tbl()
        {
            this.VendorApprovals = new HashSet<VendorApproval>();
            this.VendorFiles = new HashSet<VendorFile>();
        }
    
        public int ID { get; set; }
        public bool IsNewVendor { get; set; }
        public string ExistingReason { get; set; }
        public string Org_Sts { get; set; }
        public string vend_name { get; set; }
        public string CEO_name { get; set; }
        public string Designation { get; set; }
        public string Contact_no { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string Address1Country { get; set; }
        public string Address1State { get; set; }
        public string Address1City { get; set; }
        public string Address1Pincode { get; set; }
        public string Address1StateCode { get; set; }
        public Nullable<bool> IsSameAsAddress1 { get; set; }
        public string Address2 { get; set; }
        public string Address2Country { get; set; }
        public string Address2State { get; set; }
        public string Address2City { get; set; }
        public string Address2Pincode { get; set; }
        public string Address2StateCode { get; set; }
        public string AC_contact_Desig { get; set; }
        public string AC_contact_name { get; set; }
        public string AC_contact_Phno { get; set; }
        public string AC_contact_Email { get; set; }
        public string Spy_contact_Desig { get; set; }
        public string Spy_contact_name { get; set; }
        public string Spy_contact_Phno { get; set; }
        public string Spy_contact_Email { get; set; }
        public string CIN_No { get; set; }
        public string PAN_No { get; set; }
        public string Type_vend_gst { get; set; }
        public string GST_Reg_no { get; set; }
        public string Item_Desc { get; set; }
        public string HSN_SAC_code { get; set; }
        public string MSME_no { get; set; }
        public Nullable<decimal> Annu_TurnOver { get; set; }
        public string Nature_of_service { get; set; }
        public Nullable<int> FinancialYear1 { get; set; }
        public Nullable<int> FinancialYear2 { get; set; }
        public Nullable<bool> IsITRFiled1 { get; set; }
        public Nullable<bool> IsITRFiled2 { get; set; }
        public string AcknowledgeNo1 { get; set; }
        public string AcknowledgeNo2 { get; set; }
        public string ITR_Field_dtl { get; set; }
        public string Benificiary_name { get; set; }
        public string Bank_name { get; set; }
        public string Branch_name_Add { get; set; }
        public string Account_no { get; set; }
        public Nullable<decimal> MICR_code { get; set; }
        public string IFSC_RTGS_code { get; set; }
        public Nullable<decimal> Date { get; set; }
        public string Type_of_Vend { get; set; }
        public string NextApproverRole { get; set; }
        public string NextApprover { get; set; }
        public string TermsCode { get; set; }
        public string BankCode { get; set; }
        public string BankBranch { get; set; }
        public string PaymentType { get; set; }
        public string TaxCode { get; set; }
        public string Company { get; set; }
        public string VendorType { get; set; }
        public string DocumentPfx { get; set; }
        public string Currency { get; set; }
        public string InitiatorApproval { get; set; }
        public string LegalDepartmentApproval { get; set; }
        public string FinanceDepartmentApproval { get; set; }
        public string ITDepartmentApproval { get; set; }
        public string ExistingReasonCode { get; set; }
        public string OrgCode { get; set; }
        public string NCode { get; set; }
        public string OTP { get; set; }
        public Nullable<int> VendorCode { get; set; }
        public Nullable<bool> Step1 { get; set; }
        public Nullable<bool> Step2 { get; set; }
        public Nullable<bool> Step3 { get; set; }
        public Nullable<bool> Step4 { get; set; }
        public bool IsFinalApproved { get; set; }
        public bool IsOpened { get; set; }
        public int CreatedById { get; set; }
        public System.DateTime CreatedByDate { get; set; }
        public bool LxPostingSts { get; set; }
        public string LxPostedDateTime { get; set; }
    
        public virtual tbl_Users tbl_Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VendorApproval> VendorApprovals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VendorFile> VendorFiles { get; set; }
    }
}
