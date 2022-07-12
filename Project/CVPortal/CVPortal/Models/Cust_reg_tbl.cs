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
    
    public partial class Cust_reg_tbl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cust_reg_tbl()
        {
            this.CustomerApprovals = new HashSet<CustomerApproval>();
            this.CustomerFiles = new HashSet<CustomerFile>();
        }
    
        public int ID { get; set; }
        public Nullable<decimal> Cust_CodeVehicles { get; set; }
        public Nullable<decimal> Cust_CodeSpares { get; set; }
        public Nullable<decimal> Cust_CodeSecurity { get; set; }
        public string Org_Sts { get; set; }
        public string Cust_name { get; set; }
        public string CEO_name { get; set; }
        public string CEO_Designation { get; set; }
        public string CEO_Contact_no { get; set; }
        public string Email { get; set; }
        public string Dlr_Address { get; set; }
        public string Dlr_Add_Country { get; set; }
        public string Dlr_Add_State { get; set; }
        public string Dlr_Add_City { get; set; }
        public string Dlr_Add_Pincode { get; set; }
        public Nullable<bool> IsSameAsDlr_Address { get; set; }
        public string Supp_Address { get; set; }
        public string Supp_Add_Country { get; set; }
        public string Supp_Add_State { get; set; }
        public string Supp_Add_City { get; set; }
        public string Supp_Add_Pincode { get; set; }
        public string Contact_no { get; set; }
        public string AC_contact_Desig { get; set; }
        public string AC_contact_name { get; set; }
        public string AC_contact_Phno { get; set; }
        public string AC_contact_Mob { get; set; }
        public string AC_contact_Email { get; set; }
        public string CINNo_LLPNo { get; set; }
        public string PAN_No { get; set; }
        public string Type_Cust_gst { get; set; }
        public string GST_Reg_no { get; set; }
        public Nullable<decimal> Seucirty_Deposit { get; set; }
        public string DDNo_UTRNo { get; set; }
        public string Benificiary_name { get; set; }
        public string Bank_name { get; set; }
        public string Branch_name_Add { get; set; }
        public string Account_no { get; set; }
        public Nullable<decimal> MICR_code { get; set; }
        public string IFSC_RTGS_code { get; set; }
        public string Swift_Code { get; set; }
        public string ITR_ReturnSts { get; set; }
        public Nullable<bool> ITR_ReturnStsTurnover { get; set; }
        public Nullable<bool> ITR_ReturnTDSDeduct { get; set; }
        public string DealerType { get; set; }
        public Nullable<decimal> Date { get; set; }
        public string NextApprover { get; set; }
        public string OTP { get; set; }
        public string InitiatorApproval { get; set; }
        public string LegalDepartmentApproval { get; set; }
        public string FinanceDepartmentApproval { get; set; }
        public string ITDepartmentApproval { get; set; }
        public string Company { get; set; }
        public string CustomerType { get; set; }
        public string PaymentType { get; set; }
        public string TermsCode { get; set; }
        public string CurrencyCode { get; set; }
        public string AgentSales { get; set; }
        public string TaxCode { get; set; }
        public string DocumentSequencePrefix { get; set; }
        public Nullable<int> CustomerCode { get; set; }
        public Nullable<bool> Step1 { get; set; }
        public Nullable<bool> Step2 { get; set; }
        public Nullable<bool> Step3 { get; set; }
        public Nullable<bool> Step4 { get; set; }
        public bool IsFinalApproved { get; set; }
        public int CreatedById { get; set; }
        public System.DateTime CreatedByDate { get; set; }
    
        public virtual tbl_Users tbl_Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerApproval> CustomerApprovals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerFile> CustomerFiles { get; set; }
    }
}
