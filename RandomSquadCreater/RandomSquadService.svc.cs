using RandomSquadCreater;
using RandomSquadCreater.Core;
using System;
using System.Collections.Generic;
using System.Linq;



namespace RandomSquatCreater
{

    public class RandomSquadService : IRandomSquadService
    {


        UnitOfWork _context = new UnitOfWork(new StaticWebContext());
        //Repository<Player> repo = new Repository<Player>(new StaticWebContext());
        //Repository<Rating> repoRating = new Repository<Rating>(new StaticWebContext());
        //Repository<Score> repoScore = new Repository<Score>(new StaticWebContext());

        public bool AddPlayer(Player player)
        {
            try
            {
                //repo.Add(player);

                _context.Repository<Player>().Add(player);
                _context.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public bool AddRating(Rating rating)
        {
            try
            {
                //repoRating.Add(rating);
                _context.Repository<Rating>().Add(rating);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool AddScore(Score score)
        {
            try
            {
                //repoScore.Add(score);
                _context.Repository<Score>().Add(score);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool DeletePlayer(int id)
        {
            try
            {
                //repo.Delete(id);
                _context.Repository<Player>().Delete(id);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool DeleteRating(int id)
        {
            try
            {
                //repoRating.Delete(id);
                _context.Repository<Rating>().Delete(id);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool DeleteScore(int id)
        {
            try
            {
                //repoScore.Delete(id);
                _context.Repository<Score>().Delete(id);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<Player> GetAllPlayer()
        {
            try
            {
                return _context.Repository<Player>().GetAll().ToList();

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<Rating> GetAllRating()
        {
            try
            {
                return _context.Repository<Rating>().GetAll().ToList();

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<Score> GetAllScore()
        {
            try
            {
                return _context.Repository<Score>().GetAll().ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<string> GetAllScoreJsonList()
        {
            try
            {
                return AppGlobal.ConvertListToJson(_context.Repository<Score>().GetAll().ToList());

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<Mails> GetDraftMailsByPlayerId(int id)
        {
            throw new NotImplementedException();
        }

        public List<Mails> GetInboxMailsByPlayerId(int id)
        {
            throw new NotImplementedException();
        }

        public List<string> GetMatchRoster()
        {
            List<string> result = new List<string>();
            List<Player> matchRoster = new List<Player>();
            List<Player> players = _context.Repository<Player>().GetAll().Where(x => x.PlayerIsComing == true).ToList();

            if (players.Where(x => x.PlayerPosition == "Forward").Count() == 2)
            {
                if (players.Where(x => x.PlayerPosition == "MidField").Count() + players.Where(x => x.PlayerPosition == "Deffence").Count() == 10)
                {
                    if (players.Where(x => x.PlayerPosition == "GoalKeeper").Count() == 2)
                    {
                        matchRoster.AddRange(players.Where(x => x.PlayerPosition == "GoalKeeper").ToList());
                    }
                    else
                    {
                        matchRoster.AddRange(players.Where(x => x.PlayerPosition == "GoalKeeper").ToList());
                        result.Add("Kaleciniz bulunmamaktadır. ");
                    }

                    matchRoster.AddRange(players.Where(x => x.PlayerPosition == "MidField").ToList());
                    matchRoster.AddRange(players.Where(x => x.PlayerPosition == "Deffence").ToList());
                }
                else
                {
                    result.Add("Orta Saha ve Defans : Yeterli sayıda oyuncu yok. ");
                }
                matchRoster.AddRange(players.Where(x => x.PlayerPosition == "Forward").ToList());
            }
            else
            {
                result.Add("Forvetiniz bulunmamaktadır.");
            }



            List<string> matchPlayerRoster = AppGlobal.ConvertListToJson(matchRoster);

            if (matchPlayerRoster.Count==14)
            {
                return matchPlayerRoster;
            }
            else
            {
                return result;
            }

            

        }

        public Player GetPlayer(int id)
        {
            try
            {
                return _context.Repository<Player>().GetById(id);

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public Player GetPlayerByName(string name)
        {
            try
            {
                return _context.Repository<Player>().GetAll()
                    .Where(x => x.PlayerName + " " + x.PlayerSurname == name).FirstOrDefault();

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public Rating GetRating(int id)
        {
            try
            {
                return _context.Repository<Rating>().GetById(id);

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public Score GetScore(int id)
        {
            try
            {
                return _context.Repository<Score>().GetById(id);

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<Mails> GetSentMailsByPlayerId(int id)
        {
            throw new NotImplementedException();
        }

        public List<Mails> GetTrashMailsByPlayerId(int id)
        {
            throw new NotImplementedException();
        }

        public bool Login(string userName, string password)
        {
            return new Player().Login(userName, password);
        }

        public bool UpdatePlayer(Player player)
        {
            try
            {
                //repo.Update(player);
                _context.Repository<Player>().Update(player);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool UpdateRating(Rating rating)
        {
            try
            {
                //repoRating.Update(rating);
                _context.Repository<Rating>().Update(rating);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool UpdateScore(Score score)
        {
            try
            {
                //repoScore.Update(score);
                _context.Repository<Score>().Update(score);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
