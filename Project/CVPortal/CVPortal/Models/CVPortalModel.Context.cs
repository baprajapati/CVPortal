﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CVPortalEntities : DbContext
    {
        public CVPortalEntities()
            : base("name=CVPortalEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CurrencyCodeMaster> CurrencyCodeMasters { get; set; }
        public virtual DbSet<ExceptionLog> ExceptionLogs { get; set; }
        public virtual DbSet<GSTType_Master> GSTType_Master { get; set; }
        public virtual DbSet<LoginLog> LoginLogs { get; set; }
        public virtual DbSet<Lx_AVM> Lx_AVM { get; set; }
        public virtual DbSet<Lx_CustomerMaster> Lx_CustomerMaster { get; set; }
        public virtual DbSet<LX_TaxCode> LX_TaxCode { get; set; }
        public virtual DbSet<Orginzation_StatusMaster> Orginzation_StatusMaster { get; set; }
        public virtual DbSet<ParameterConfiguration> ParameterConfigurations { get; set; }
        public virtual DbSet<PaymentTermsMaster> PaymentTermsMasters { get; set; }
        public virtual DbSet<PayTypeMaster> PayTypeMasters { get; set; }
        public virtual DbSet<tbl_Users> tbl_Users { get; set; }
        public virtual DbSet<VendorApproval> VendorApprovals { get; set; }
        public virtual DbSet<VendorFile> VendorFiles { get; set; }
        public virtual DbSet<VendorTypeMaster> VendorTypeMasters { get; set; }
        public virtual DbSet<webpages_Membership> webpages_Membership { get; set; }
        public virtual DbSet<webpages_OAuthMembership> webpages_OAuthMembership { get; set; }
        public virtual DbSet<webpages_Roles> webpages_Roles { get; set; }
        public virtual DbSet<DepartmentMaster> DepartmentMasters { get; set; }
        public virtual DbSet<Vend_reg_tbl> Vend_reg_tbl { get; set; }
    }
}
