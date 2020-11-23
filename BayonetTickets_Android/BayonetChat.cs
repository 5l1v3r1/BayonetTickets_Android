using Microsoft.AppCenter.Analytics;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Xml;

namespace BayonetTickets_Android
{
    class BayonetChat
    {
        static string API_URL;
        static string BOT_NAME;
        static string BOT_PASSWORD;
        static string ROOM_ID;
        static string AUTH_TOKEN;
        static string USER_ID;

        public static void QueryConfig()
        {
            Analytics.TrackEvent("Reading XML Config");
            XmlDocument doc = new XmlDocument();
            doc.Load(Android.App.Application.Context.Assets.Open("Config.xml"));
            API_URL = doc.SelectSingleNode("//configuration/RocketChat/API").InnerText;
            BOT_NAME = doc.SelectSingleNode("//configuration/RocketChat/User").InnerText;
            BOT_PASSWORD = doc.SelectSingleNode("//configuration/RocketChat/Password").InnerText;
            ROOM_ID = doc.SelectSingleNode("//configuration/RocketChat/Room").InnerText;
            LoginToAPI();
        }

        public static void LoginToAPI()
        {
            Analytics.TrackEvent("Logged into API");
            RestClient client = new RestClient(API_URL);
            client.Authenticator = new SimpleAuthenticator("user", BOT_NAME, "password", BOT_PASSWORD);
            var request = new RestRequest("login", Method.POST);
            var response = client.Execute(request);
            dynamic content = JsonConvert.DeserializeObject(response.Content);
            var data = content.data;
            string auth = data.authToken.ToString();
            string userId = data.userId.ToString();
            AUTH_TOKEN = auth;
            USER_ID = userId;
        }

        public static void PostMessageToChat(string ticket, string user)
        {
            RestClient client = new RestClient(API_URL);
            var ticketRequest = new RestRequest("chat.postMessage", Method.POST);
            ticketRequest.AddHeader("X-Auth-Token", AUTH_TOKEN);
            ticketRequest.AddHeader("X-User-Id", USER_ID);
            ticketRequest.AddHeader("Content-Type", "application/json");
            ticketRequest.AddJsonBody((new { text = ticket, roomId = ROOM_ID, alias = user }));
            client.Execute(ticketRequest);
        }
    }
}