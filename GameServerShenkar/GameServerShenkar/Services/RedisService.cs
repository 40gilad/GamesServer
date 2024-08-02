using GameServerShenkar.Managers;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerShenkar.Services
{
    internal class RedisService
    {
        #region Get/Set Dictionaries

        private static Dictionary<string, string> RedisGetDictionary(string _Key)
        {
            Dictionary<string, string> _ret = new Dictionary<string, string>();
            try
            {
                var _cacheConnection = RedisManager.connection.GetDatabase();
                HashEntry[] _data = _cacheConnection.HashGetAll(_Key);
                foreach (HashEntry hash in _data)
                    _ret.Add(hash.Name, hash.Value);
            }
            catch (Exception e)
            { }
            return _ret;
        }

        private static void RedisSetDictionary(string _Key, Dictionary<string, string> _UserDetails)
        {
            try
            {
                var _cacheConnection = RedisManager.connection.GetDatabase();
                HashEntry[] _entry = new HashEntry[_UserDetails.Count];

                int _count = 0;
                foreach (string s in _UserDetails.Keys)
                {
                    RedisKey _key = new RedisKey(s);
                    RedisValue _name = new RedisValue(s);
                    RedisValue _value = new RedisValue(_UserDetails[s]);
                    _entry[_count++] = new HashEntry(_name, _value);

                }
                _cacheConnection.HashSet(_Key, _entry);
            }
            catch (Exception e) { }
        }


        #endregion

        #region Get/Set Strings

        private static void RedisSet(string key, string value)
        {
            var cacheConnection = RedisManager.connection.GetDatabase();
            cacheConnection.StringSet(key, value);
        }
        private static string RedisGet(string key)
        {
            var cacheConnection = RedisManager.connection.GetDatabase();
            return cacheConnection.StringGet(key);
        }
        #endregion

        #region Remove Keys

        private static void RemoveData(string key)
        {
            var cacheConnection = RedisManager.connection.GetDatabase();
            cacheConnection.KeyDelete(key);
        }

        #endregion

        #region Player
        public static Dictionary<string, string> GetPlayer(string email)
        {
            return RedisGetDictionary(email + "#Players");
        }
        public static void SetPlayer(string email, Dictionary<string, string> playerData)
        {
            RedisSetDictionary(email + "#Players", playerData);
        }

        public static void SetPlayerRating(string userId, int rating)
        {
            RedisSet(userId + "#Rating", rating.ToString());
        }

        public static int GetPlayerRating(string userId)
        {
            try
            {
                string rating = RedisGet(userId + "#Rating");
                int ratingInt = Convert.ToInt32(rating);

                return ratingInt;
            }
            catch (Exception e) { }
            return 0;
        }

        #endregion

        #region MatchId

        public static string GetMatchId()
        {return RedisGet("MatchId");}

        public static void SetMatchId(string MatchId)
        {RedisSet("MatchId", MatchId);}

        #endregion
    }
}
