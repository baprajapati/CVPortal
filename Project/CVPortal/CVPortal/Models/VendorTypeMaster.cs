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
    
    public partial class VendorTypeMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VendorTypeMaster()
        {
            this.VendorApprovals = new HashSet<VendorApproval>();
        }
    
        public int VT_ID { get; set; }
        public string Company { get; set; }
        public string VendorType { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VendorApproval> VendorApprovals { get; set; }
    }
}
