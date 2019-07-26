using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace RandomSquadCreater.Core
{
    [DataContract]
    [Table("Rating")]
    public class Rating
    {
        [DataMember]
        public int RatingId { get; set; }
        [DataMember]
        public int Rated { get; set; } //puan veren
        [DataMember]
        public int Scored { get; set; } // puan alan
        [DataMember]
        public int Point { get; set; }
        [DataMember]
        public DateTime EventDate { get; set; }

    }
}
