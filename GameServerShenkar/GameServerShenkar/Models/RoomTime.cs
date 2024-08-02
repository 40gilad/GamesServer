using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerShenkar.Models
{
    public class RoomTime
    {
        private float _maxTurnTime; //10 seconds
        private float _timeOutTime; //10 seconds
        private DateTime _startDate;
        private DateTime _startRoomDate;

        public RoomTime(float maxTurnTime, float timeOutTime)
        {
            _maxTurnTime = maxTurnTime;
            _timeOutTime = timeOutTime;
            _startRoomDate = DateTime.UtcNow;
        }

        public void ResetTimer()
        {
            _startDate = DateTime.UtcNow;
        }

        public bool IsCurrentTimeActive()
        {
            if (_startDate != null)
            {
                TimeSpan diff = DateTime.UtcNow - _startDate;
                if (diff.TotalSeconds < _maxTurnTime)
                    return true;
            }
            return false;
        }

        public bool IsRoomTimeOut()
        {
            if (_startDate != null)
            {
                TimeSpan diff = DateTime.UtcNow - _startRoomDate;
                if (diff.TotalSeconds < _timeOutTime)
                    return true;
            }
            return false;
        }
    }
}
