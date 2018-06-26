using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace BanCheckWindows
{
    public class BanCheck
    {
        private string PlayerPersonalURL;
        //private bool IsCustomURL;
        private Player toCheck;
        Player hasChecked;
        string steamAPIURL = "https://api.steampowered.com/ISteamUser/GetPlayerBans/v1/";
        public BanCheck()
        {

        }

        public BanCheck(string url,bool isMultiPlayers=false)
        {
            PlayerPersonalURL = url;
            string playerID = GetIDFromPersonalURL(PlayerPersonalURL);
            string apiRequestURL="";
            toCheck = new Player(playerID);
            if(!isMultiPlayers)
                apiRequestURL = App.GetPlayerBansAPIURL + "?key=" + App.SteamAPIKey + "&steamids=" + toCheck.SteamId + ",";

            ////TODO: 生成查询多个ID的URL
            string jsonFile = GetHttpResponse(apiRequestURL);
            hasChecked = GetSingleResult(toCheck, jsonFile);
        }

        public string GetIDFromPersonalURL(string url)
        {
            string idResult = "";

            if (url.Contains("id"))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(url + "?xml=1");
                string xpathID64 = "//profile/steamID64";
                XmlNode nodeID64 = doc.SelectSingleNode(xpathID64);
                idResult = nodeID64.InnerText;
            }
            else if (url.Contains("profiles"))
            {
                string[] spilted = url.Split('/');
                idResult = spilted[spilted.Length - 1];
            }
            
            return idResult;
        }

        public Player GetSingleResult(Player check,string json)
        {
            List<string> resultList = new List<string>();

            JObject jObj = JObject.Parse(json);
            check.SteamId = (string)jObj["players"][0]["SteamId"];
            check.CommunityBanned = Convert.ToBoolean((string)jObj["players"][0]["CommunityBanned"]);
            check.VACBanned = Convert.ToBoolean((string)jObj["players"][0]["VACBanned"]);
            check.NumberOfVACBanns = Convert.ToInt32((string)jObj["players"][0]["NumberOfVACBans"]);
            check.NumberOfGameBans = Convert.ToInt32((string)jObj["players"][0]["NumberOfGameBans"]);
            check.EconomyBan = (string)jObj["players"][0]["EconomyBan"];
            check.DaysSinceLastBan =Convert.ToInt32((string)jObj["players"][0]["DaysSinceLastBan"]);

            return check;
        }

        public string SetResultUI()
        {
            string uiString = "";

            uiString += "SteamId: " + hasChecked.SteamId + "\r\n";
            uiString += "社区封禁: " + hasChecked.CommunityBanned.ToString() + "\r\n";
            uiString += "VAC封禁: " + hasChecked.VACBanned.ToString() + "\r\n";
            uiString += "VAC封禁数: " + hasChecked.NumberOfVACBanns.ToString() + "\r\n";
            uiString += "上次封禁至今: " + hasChecked.DaysSinceLastBan.ToString() + " 天days\r\n";
            uiString += "游戏封禁: " + hasChecked.NumberOfGameBans.ToString() + "\r\n";
            uiString += "交易封禁: " + hasChecked.EconomyBan;



            return uiString;
        }
        public string GetHttpResponse(string url)
        {
            if (url == "")
                return "empty url";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = 5000;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

       
    }
    public class Player
    {
        public string SteamId { get; set; }
        public bool VACBanned { get; set; }
        public bool CommunityBanned { get; set; }
        public int NumberOfVACBanns { get; set; }
        public int DaysSinceLastBan { get; set; }
        public int NumberOfGameBans { get; set; }
        public string EconomyBan { get; set; }
        
        public Player(string id)
        {
            SteamId = id;
            VACBanned = false;
            CommunityBanned = false;
            NumberOfGameBans = 0;
            NumberOfVACBanns = 0;
            DaysSinceLastBan = 0;
            NumberOfVACBanns = 0;
            EconomyBan = "none";
        }

    }
}
