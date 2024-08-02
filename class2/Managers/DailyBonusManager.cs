using LobbyServer.Models;
using LobbyServer.Services;
using System;
namespace LobbyServer.Managers

{
    public class DailyBonusManager
    {
        #region Singleton
        public static DailyBonusManager _instance;
        public static DailyBonusManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DailyBonusManager();
                return _instance;
            }
        }
        #endregion

        private int min_hours_to_get_bouns = 24;
        private int max_hours_to_get_bouns = 48;

        public DailyBonusManager() { }

        public void DailyBouns(User user)
        {//calculate and update daily bouns and update last_log_in to now

            user.diamonds = CalcDailyBonus(
                hours_from_last_log_in: Math.Abs(GetHoursFromLastLogIn(user.last_log_in))
                , curr_diamonds: user.diamonds
                );
            user.last_log_in = DateTime.UtcNow.ToString();
        }

        private int CalcDailyBonus(int hours_from_last_log_in, int curr_diamonds)
        {
            int first_day = 10,
                second_day = 20,
                third_day = 30,
                fourh_day = 40,
                fifth_day = 60,
                sixt_day = 80,
                seventh_day = 100;

            if (hours_from_last_log_in == -1) //Error. keep diamonds as is
                return curr_diamonds;

            if (hours_from_last_log_in > max_hours_to_get_bouns)//reset daily bonus to first day
                                                                //logged in in interval of more then 48 hours
                return first_day;
            else if (hours_from_last_log_in < min_hours_to_get_bouns) // keep diamonds as is.
                                                                      // logged in in interval of less then 24 hours
                return curr_diamonds;

    
            // if here - logged in with interval of more then 24 hours and less then 48
            switch (curr_diamonds)
            {
                //case x_day:
                // return x+1_day

                case 10:
                    return second_day;
                case 20:
                    return third_day;
                case 30:
                    return fourh_day;
                case 40:
                    return fifth_day;
                case 60:
                    return sixt_day;
                case 80:
                    return seventh_day;
                case 100:
                    return first_day;
                default: return first_day;
            }
        }

        private int GetHoursFromLastLogIn(string last_log_in)
        {
            try
            {
                DateTime lli = DateTime.Parse(last_log_in);
                DateTime now = DateTime.UtcNow;
                TimeSpan diff = lli - now;
                return (int)diff.TotalHours;
            }
            catch { return -1; }

        }
    }
}
