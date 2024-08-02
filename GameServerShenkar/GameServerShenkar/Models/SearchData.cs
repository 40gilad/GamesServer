using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerShenkar.Models
{
    public class SearchData
    {
        private string _userId;
        public string UserId { get => _userId; }

        private int _rating;
        public int Rating { get => _rating; }

        private bool _isReady;
        public bool IsReady { get => _isReady; set => _isReady = value; }

        public SearchData(string userId, int rating)
        {
            _userId = userId;
            _rating = rating;
            _isReady = false;
        }
    }
}
