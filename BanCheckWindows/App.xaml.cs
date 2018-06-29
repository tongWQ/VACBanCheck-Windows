using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
//using System.Threading.Tasks;
using System.Windows;

namespace BanCheckWindows
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 

    public partial class App : Application
    {
        public static string SteamAPIKey = "DCCF6B72328C01D1EE708DA272F01327";
        public const string GetPlayerBansAPIURL = "https://api.steampowered.com/ISteamUser/GetPlayerBans/v1/";
        public const string GetPlayerSummariesURL = "https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/";
    }
}
