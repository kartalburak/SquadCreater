using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Security.Principal;

namespace RandomSquadCreater.Core
{

    [DataContract]
    [Table("Mails")]
    public class Mails
    {

        [Key]
        [DataMember]
        public int MailId { get; set; }


        [DataMember]
        public int PlayerId { get; set; }

        [DataMember]
        public string From { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string To { get; set; }


        [DataMember]
        public string Subject { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }


        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public int Type { get; set; }


    }

    enum MailType{
        Inbox=1,
        Sent=2,
        Draft=3,
        Trash=4

    }





}