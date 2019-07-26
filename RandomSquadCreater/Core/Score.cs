using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace RandomSquadCreater.Core
{

    [DataContract]
    [Table("Score")]
    public class Score
    {
        [Key]
        [DataMember]
        public int ScoreId { get; set; }
        [DataMember]
        public int PlayerId { get; set; }
        [DataMember]
        public DateTime EventDate { get; set; }
        [DataMember]
        public float AvaragePoint { get; set; }


    }
}
