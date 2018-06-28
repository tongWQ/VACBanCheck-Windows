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
        private Player hasChecked;
        public Player[] Players;
        private int playerCount;
      //  string steamAPIURL = "https://api.steampowered.com/ISteamUser/GetPlayerBans/v1/";
        public BanCheck()
        {

        }

        /// <summary>
        /// 调用该构造函数后就生成了已查询过的Players（Player[]）
        /// </summary>
        /// <param name="url">以英文逗号,分隔的多个个人主页链接字符串</param>
        public BanCheck(string url)
        {
            PlayerPersonalURL = url;
           // string playerID = GetIDFromPersonalURL(PlayerPersonalURL);
            string apiRequestURL="";
           // toCheck = new Player(playerID);
            apiRequestURL = App.GetPlayerBansAPIURL + "?key=" + App.SteamAPIKey + "&steamids="+SpiltURLtoAPIParam(url);
            string jsonFile = GetHttpResponse(apiRequestURL);
            Players = GetMultiResults(playerCount,jsonFile);
            
        }

        /// <summary>
        /// 从个人主页链接中提取ID并生成API所需参数
        /// </summary>
        /// <param name="total">以英文逗号,分隔的多个个人主页链接字符串</param>
        /// <returns>格式化的API参数steamids</returns>
        public string SpiltURLtoAPIParam(string total)
        {
            if (string.IsNullOrEmpty(total))
                return null;
            string apiParam = "";
            string[] spilted = total.Split(',');

            playerCount = spilted.Length;

            for (int i = 0; i < spilted.Length; i++)
            {
                apiParam += GetIDFromPersonalURL(spilted[i]) + ",";
            }
            return apiParam;
        }

        /// <summary>
        /// 从单个URL中提取Steam ID，暂不支持个性化主页链接
        /// </summary>
        /// <param name="url">单个URL</param>
        /// <returns>提取到的steamid</returns>
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

        /// <summary>
        /// Abandoned
        /// </summary>
        /// <param name="check"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public Player GetSingleResult(Player check,string json)
        {
           // List<string> resultList = new List<string>();

            JObject jObj = JObject.Parse(json);
            check.SteamId = (string)jObj["players"][0]["SteamId"];
            check.CommunityBanned = Convert.ToBoolean((string)jObj["players"][0]["CommunityBanned"]);
            check.VACBanned = Convert.ToBoolean((string)jObj["players"][0]["VACBanned"]);
            check.NumberOfVACBans = Convert.ToInt32((string)jObj["players"][0]["NumberOfVACBans"]);
            check.NumberOfGameBans = Convert.ToInt32((string)jObj["players"][0]["NumberOfGameBans"]);
            check.EconomyBan = (string)jObj["players"][0]["EconomyBan"];
            check.DaysSinceLastBan =Convert.ToInt32((string)jObj["players"][0]["DaysSinceLastBan"]);

            return check;
        }

        /// <summary>
        /// 将json文件内容转化为Player对象
        /// </summary>
        /// <param name="count">链接数</param>
        /// <param name="json">收到的json文件</param>
        /// <returns></returns>
        public Player[] GetMultiResults(int count, string json)
        {
            if (string.IsNullOrEmpty(json)||count<1)
                return null;

            Player[] results = new Player[count];
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = new Player();
            }
            JObject jObj = JObject.Parse(json);
            for (int i = 0; i < count; i++)
            {
                results[i].SteamId = (string)jObj["players"][i]["SteamId"];
                results[i].CommunityBanned = Convert.ToBoolean((string)jObj["players"][i]["CommunityBanned"]);
                results[i].VACBanned = Convert.ToBoolean((string)jObj["players"][i]["VACBanned"]);
                results[i].NumberOfVACBans = Convert.ToInt32((string)jObj["players"][i]["NumberOfVACBans"]);
                results[i].NumberOfGameBans = Convert.ToInt32((string)jObj["players"][i]["NumberOfGameBans"]);
                results[i].EconomyBan = (string)jObj["players"][i]["EconomyBan"];
                results[i].DaysSinceLastBan = Convert.ToInt32((string)jObj["players"][i]["DaysSinceLastBan"]);
            }


            return results;
        }

        /// <summary>
        /// Abandoned
        /// </summary>
        /// <param name="toSet"></param>
        /// <returns></returns>
        public string SetResultUI(Player toSet)
        {
            string uiString = "";

            uiString += "SteamId: " + toSet.SteamId + "\r\n";
            uiString += "社区封禁: " + toSet.CommunityBanned.ToString() + "\r\n";
            uiString += "VAC封禁: " + toSet.VACBanned.ToString() + "\r\n";
            uiString += "VAC封禁数: " + toSet.NumberOfVACBans.ToString() + "\r\n";
            uiString += "上次封禁至今: " + toSet.DaysSinceLastBan.ToString() + " 天\r\n";
            uiString += "游戏封禁: " + toSet.NumberOfGameBans.ToString() + "\r\n";
            uiString += "交易封禁: " + toSet.EconomyBan;
            
            return uiString;
        }

        /// <summary>
        /// 在UI显示结果，通过index切换显示的结果
        /// </summary>
        /// <param name="toSetIndex">players数组索引</param>
        /// <returns>供UI显示的已格式化的字符串</returns>
        public string SetResultUI(int toSetIndex)
        {
            if (toSetIndex > Players.Length - 1 || toSetIndex < 0)
                return "";

            string uiString = "";

            uiString += "SteamId: " + Players[toSetIndex].SteamId + "\r\n";
            uiString += "社区封禁: " + Players[toSetIndex].CommunityBanned.ToString() + "\r\n";
            uiString += "VAC封禁: " + Players[toSetIndex].VACBanned.ToString() + "\r\n";
            uiString += "VAC封禁数: " + Players[toSetIndex].NumberOfVACBans.ToString() + "\r\n";
            uiString += "上次封禁至今: " + Players[toSetIndex].DaysSinceLastBan.ToString() + " 天\r\n";
            uiString += "游戏封禁: " + Players[toSetIndex].NumberOfGameBans.ToString() + "\r\n";
            uiString += "交易封禁: " + Players[toSetIndex].EconomyBan;

            return uiString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">Steam API的url，不是个人主页url</param>
        /// <returns></returns>
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
        public int NumberOfVACBans { get; set; }
        public int DaysSinceLastBan { get; set; }
        public int NumberOfGameBans { get; set; }
        public string EconomyBan { get; set; }
        
        /// <summary>
        /// Abandoned
        /// </summary>
        /// <param name="id"></param>
        public Player(string id)
        {
            SteamId = id;
            VACBanned = false;
            CommunityBanned = false;
            NumberOfGameBans = 0;
            NumberOfVACBans = 0;
            DaysSinceLastBan = 0;
            NumberOfVACBans = 0;
            EconomyBan = "none";
        }
        public Player()
        {
            SteamId = "";
            VACBanned = false;
            CommunityBanned = false;
            NumberOfGameBans = 0;
            NumberOfVACBans = 0;
            DaysSinceLastBan = 0;
            NumberOfVACBans = 0;
            EconomyBan = "none";
        }
    }
}
