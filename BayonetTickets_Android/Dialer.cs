using Microsoft.AppCenter.Analytics;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace BayonetTickets_Android
{
    class Dialer
    {
        public static Display disp = new Display();
        public static async Task OnCallButtonClick()
        {
            Analytics.TrackEvent("Call IT Button Pressed");
            try
            {
                PhoneDialer.Open("7279335322");
            }
            catch (ArgumentNullException anEx)
            {
                disp.DisplayFailureNotice(anEx.Message);
            }
            catch (FeatureNotSupportedException ex)
            {
                disp.DisplayFailureNotice(ex.Message);
            }
            catch (Exception ex)
            {
                disp.DisplayFailureNotice(ex.Message);
            }
        }

        public static async Task OnCallMechanicClick()
        {
            Analytics.TrackEvent("Call Mechanic Button Pressed");
            try
            {
                PhoneDialer.Open("");
            }
            catch (ArgumentNullException anEx)
            {
                disp.DisplayFailureNotice(anEx.Message);
            }
            catch (FeatureNotSupportedException ex)
            {
                disp.DisplayFailureNotice(ex.Message);
            }
            catch (Exception ex)
            {
                disp.DisplayFailureNotice(ex.Message);
            }
        }

        public static async Task OnCallSafetyButtonClick()
        {
            Analytics.TrackEvent("Call Safety Button Pressed");
            try
            {
                PhoneDialer.Open("3529427031");
            }
            catch (ArgumentNullException anEx)
            {
                disp.DisplayFailureNotice(anEx.Message);
            }
            catch (FeatureNotSupportedException ex)
            {
                disp.DisplayFailureNotice(ex.Message);
            }
            catch (Exception ex)
            {
                disp.DisplayFailureNotice(ex.Message);
            }
        }
    }
}