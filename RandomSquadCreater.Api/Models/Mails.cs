//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RandomSquadCreater.Api.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Mails
    {
        public int MailId { get; set; }
        public Nullable<int> PlayerId { get; set; }
        public string From { get; set; }
        public string Password { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string Message { get; set; }
        public Nullable<int> Type { get; set; }
    }
}
