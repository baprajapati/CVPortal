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
    
    public partial class PayTypeMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PayTypeMaster()
        {
            this.VendorApprovals = new HashSet<VendorApproval>();
        }
    
        public int PayType_ID { get; set; }
        public string PayType_Code { get; set; }
        public string PayType_Desc { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VendorApproval> VendorApprovals { get; set; }
    }
}
