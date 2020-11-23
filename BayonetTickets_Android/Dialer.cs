using Microsoft.AppCenter.Analytics;
using System;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Essentials;

namespace BayonetTickets_Android
{
    class Dialer
    {
        public static void OpenDialer(string number)
        {
            MainActivity main = new MainActivity();
            main.DisplayFailureNotice("fail");
            try
            {
                PhoneDialer.Open(number);
            }
            catch (ArgumentNullException anEx)
            {
                main.DisplayFailureNotice(anEx.Message);
            }
            catch (FeatureNotSupportedException ex)
            {
                main.DisplayFailureNotice(ex.Message);
            }
            catch (Exception ex)
            {
                main.DisplayFailureNotice(ex.Message);
            }
        }

        public static string GetPhoneNumber(string type)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Android.App.Application.Context.Assets.Open("Config.xml"));
            return doc.SelectSingleNode("//configuration/Phone/" + type).InnerText;
        }

        public static async Task OnCallButtonClick()
        {
            Analytics.TrackEvent("Call IT Button Pressed");
            string number = GetPhoneNumber("IT");
            OpenDialer(number);
        }

        public static async Task OnCallMechanicClick()
        {
            Analytics.TrackEvent("Call Mechanic Button Pressed");
            string number = GetPhoneNumber("Mechanic");
            OpenDialer(number);
        }

        public static async Task OnCallSafetyButtonClick()
        {
            Analytics.TrackEvent("Call Safety Button Pressed");
            string number = GetPhoneNumber("HR");
            OpenDialer(number);
        }
    }
}