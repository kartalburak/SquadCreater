using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;


namespace RandomSquadCreater.Core
{
    [DataContract]
    [Table("Player")]
    public class Player
    {
        [Key]
        [DataMember]
        public int PlayerId { get; set; }
        [DataMember]
        public string PlayerName { get; set; }
        [DataMember]
        public string PlayerSurname { get; set; }
        [DataMember]
        [Required]
        public string PlayerEmail { get; set; }
        [DataMember]
        public int PlayerPower { get; set; }
        [DataMember]
        public string PlayerImageUrl { get; set; }
        [DataMember]
        public string PlayerPosition { get; set; }
        [DataMember]
        public string PlayerPassword { get; set; }
        [DataMember]
        public string PlayerUserName { get; set; }
        [DataMember]
        public bool PlayerIsComing { get; set; }

        [DataMember]
        public DateTime PlayerVoteDate { get; set; }

        [DataMember]
        public bool PlayerIsOnline { get; set; }

        [DataMember]
        public bool PlayerIsAdmin { get; set; }

        [DataMember]
        public string PlayerConnectionId { get; set; }

        [DataMember]
        public DateTime PlayerLastLoginTime { get; set; }


        public bool Login(string userName, string password)
        {
            Repository<Player> repo = new Repository<Player>(new StaticWebContext());
            Player player = repo.GetAll().Where(x => x.PlayerUserName == userName && x.PlayerPassword == password).FirstOrDefault();

            return player != null ? true : false;

        }


    }



}
