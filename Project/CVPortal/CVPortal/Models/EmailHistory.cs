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
    
    public partial class EmailHistory
    {
        public int Id { get; set; }
        public string EmailType { get; set; }
        public Nullable<int> EntityId { get; set; }
        public string MailTo { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Subject { get; set; }
        public string HtmlContent { get; set; }
        public bool IsMailSent { get; set; }
        public int CreatedById { get; set; }
        public System.DateTime CreatedByDate { get; set; }
    }
}