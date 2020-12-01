using Android.App;
using Microsoft.AppCenter.Analytics;
using Plugin.Messaging;
using Plugin.Permissions;
using System.Threading.Tasks;
using System.Xml;
using PermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;

namespace BayonetTickets_Android
{
    class Dialer
    {
        public static async Task OpenDialerAsync(string number)
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync<PhonePermission>();
            if (status != PermissionStatus.Granted)
                status = await CrossPermissions.Current.RequestPermissionAsync<PhonePermission>();
            if (status == PermissionStatus.Granted)
            {
                var phoneDialer = CrossMessaging.Current.PhoneDialer;
                if (phoneDialer.CanMakePhoneCall)
                    phoneDialer.MakePhoneCall(number);
            }
        }

        public static string GetPhoneNumber(string type)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Application.Context.Assets.Open("Config.xml"));
            return doc.SelectSingleNode("//configuration/Phone/" + type).InnerText;
        }

        public static async Task OnCallButtonClick()
        {
            Analytics.TrackEvent("Call IT Button Pressed");
            string number = GetPhoneNumber("IT");
            OpenDialerAsync(number);
        }

        public static async Task OnCallMechanicClick()
        {
            Analytics.TrackEvent("Call Mechanic Button Pressed");
            string number = GetPhoneNumber("Mechanic");
            OpenDialerAsync(number);
        }

        public static async Task OnCallSafetyButtonClick()
        {
            Analytics.TrackEvent("Call Safety Button Pressed");
            string number = GetPhoneNumber("HR");
            OpenDialerAsync(number);
        }
    }
}