namespace LobbyServer.Managers
{
    public class UserNameManager
    {

        #region Singleton
        public static UserNameManager _instance;
        public static UserNameManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UserNameManager();
                return _instance;
            }
        }
        #endregion

        private List<string> user_name_list;

        private void CreateUserNameList()
        {

            user_name_list = new List<string>()
                {
                // The Beatles
                "JohnLennon", // Singer, Songwriter, Rhythm Guitarist
                "PaulMcCartney", // Bass Guitarist, Singer, Songwriter
                "GeorgeHarrison", // Lead Guitarist, Singer, Songwriter
                "RingoStarr", // Drummer, Singer

                // Pink Floyd
                "DavidGilmour", // Lead Guitarist, Singer, Songwriter
                "RogerWaters", // Bass Guitarist, Singer, Songwriter
                "NickMason", // Drummer, Percussionist
                "SydBarrett", // Founder 

                // Deep Purple
                "RitchieBlackmore", // Lead Guitarist, Songwriter
                "JonLord", // Keyboardist, Songwriter
                "IanPaice", // Drummer
                "IanGillan", // Vocalist
                "RogerGlover", // Bass Guitarist

                // Led Zeppelin
                "JimmyPage", // Lead Guitarist, Producer
                "RobertPlant", // Vocalist
                "JohnBonham", // Drummer
                "JohnPaulJones", // Bass Guitarist, Keyboardist, Mandolinist

                // The Rolling Stones
                "MickJagger", // Vocalist, Songwriter
                "KeithRichards", // Guitarist, Songwriter
                "CharlieWatts", // Drummer (deceased)
                "RonnieWood", // Guitarist

                // The Who
                "PeteTownshend", // Guitarist, Songwriter, Vocalist
                "RogerDaltrey", // Lead Vocalist
                "JohnEntwistle", // Bass Guitarist (deceased)
                "KeithMoon", // Drummer (deceased)

                // Queen
                "FreddieMercury", // Lead Vocalist, Songwriter, Pianist
                "BrianMay", // Guitarist, Songwriter
                "JohnDeacon", // Bass Guitarist (retired)
                "RogerTaylor", // Drummer, Singer, Songwriter

                // Singles
                "JimiHendrix", // Lead Guitarist, Vocalist, Songwriter
                "LesClypool",
                "FrankZappa",

                // The Doors
                "JimMorrison", // Lead Vocalist
                "RayManzarek", // Keyboardist, Singer
                "JohnDensmore", // Drummer
                "RobbyKrieger", // Guitarist

                // Israelis
                "RamiFortis",
                "ShalomHanoch",
                "ArikEinstein",
                "TheZabariBrothers",
                "RavidPlotnik",
                "MattiCaspi",
                "ShiranAsaf",
                "Tuna",
                "HadagNahash",

                // Guns N' Roses
                "AxlRose", // Vocalist
                "Slash", // Lead Guitarist
                "DuffMcKagan", // Bass Guitarist
                "IzzyStradlin", // Rhythm Guitarist (former)
                "StevenAdler", // Drummer (former)

                // David Bowie
                "DavidBowie", // Singer, Songwriter, Musician, Actor

                // King Crimson
                "RobertFripp", // Guitarist, Songwriter, Bandleader
                "AdrianBelew", // Singer, Guitarist, Songwriter (current)
                "TonyLevin", // Bass Guitarist (current)
                "MelCollins", // Saxophonist, Flautist (former)
                "BillBruford", // Drummer (former)

                // Primus
                "LesClaypool", // Bass Guitarist, Vocalist
                "LarryLalonde", // Guitarist
                "TimAlexander", // Drummer (former)
                "BryanMantia", // Drummer (current)

                // Famous Rappers
                "KendrickLamar", // Rapper, Songwriter, Record Producer
                "KanyeWest", // Rapper, Singer, Songwriter, Record Producer, Fashion Designer
                "Eminem" // Rapper, Songwriter, Record Producer
            };
        }
        public string GenrateUserName()
        {
            if(user_name_list == null)
                CreateUserNameList();

            Random rand = new Random();
            int indx = rand.Next(0, user_name_list.Count);
            return user_name_list[indx];
        }

    }
}
