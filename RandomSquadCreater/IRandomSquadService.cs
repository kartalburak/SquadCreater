using RandomSquadCreater.Core;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;


namespace RandomSquatCreater
{

    [ServiceContract]
    public interface IRandomSquadService
    {
        [OperationContract]
        List<Rating> GetAllRating();
        [OperationContract]
        List<Score> GetAllScore();
        [OperationContract]
        List<Player> GetAllPlayer();
        [OperationContract]
        Rating GetRating(int id);
        [OperationContract]
        Score GetScore(int id);
        [OperationContract]
        Player GetPlayer(int id);
        [OperationContract]
        Player GetPlayerByName(string name);
        [OperationContract]
        bool DeleteRating(int id);
        [OperationContract]
        bool DeleteScore(int id);
        [OperationContract]
        bool DeletePlayer(int id);
        [OperationContract]
        bool UpdateRating(Rating rating);
        [OperationContract]
        bool UpdateScore(Score score);
        [OperationContract]
        bool UpdatePlayer(Player player);
        [OperationContract]
        bool AddRating(Rating rating);
        [OperationContract]
        bool AddScore(Score score);
        [OperationContract]
        bool AddPlayer(Player player);

        [OperationContract]
        bool Login(string userName, string password);
        [OperationContract]
        List<string> GetAllScoreJsonList();

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json,ResponseFormat = WebMessageFormat.Json)]
        List<string> GetMatchRoster();

        [OperationContract]
        List<Mails> GetInboxMailsByPlayerId(int id);
        [OperationContract]
        List<Mails> GetSentMailsByPlayerId(int id);
        [OperationContract]
        List<Mails> GetDraftMailsByPlayerId(int id);
        [OperationContract]
        List<Mails> GetTrashMailsByPlayerId(int id);

    }
}
