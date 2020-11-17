using Microsoft.AppCenter.Analytics;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace BayonetTickets_Android
{
    class Dialer
    {
        public static void OpenDialer(string number)
        {
            MainActivity main = new MainActivity();
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

        public static async Task OnCallButtonClick()
        {
            Analytics.TrackEvent("Call IT Button Pressed");
            OpenDialer("7279335322");
        }

        public static async Task OnCallMechanicClick()
        {
            Analytics.TrackEvent("Call Mechanic Button Pressed");
            OpenDialer(null);
        }

        public static async Task OnCallSafetyButtonClick()
        {
            Analytics.TrackEvent("Call Safety Button Pressed");
            OpenDialer("3529427031");
        }
    }
}