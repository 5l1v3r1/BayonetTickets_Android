using Microsoft.AppCenter.Analytics;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace BayonetTickets_Android
{
    class BayonetChat
    {
        const string API_URL = "https://bayonetchat.com/api/v1/";
        const string BOT_NAME = "bayonet.tickets";
        const string BOT_PASSWORD = "bayonet-9";
        const string ROOM_ID = "nZucG9euMFuyoq9e7";
        public static RestClient client = new RestClient(API_URL);
        static string AUTH_TOKEN;
        static string USER_ID;

        public static void LoginToAPI()
        {
            Analytics.TrackEvent("Logged into API");
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
            var ticketRequest = new RestRequest("chat.postMessage", Method.POST);
            ticketRequest.AddHeader("X-Auth-Token", AUTH_TOKEN);
            ticketRequest.AddHeader("X-User-Id", USER_ID);
            ticketRequest.AddHeader("Content-Type", "application/json");

            ticketRequest.AddJsonBody((new { text = ticket, roomId = ROOM_ID, alias = user }));

            client.Execute(ticketRequest);
        }
    }
}