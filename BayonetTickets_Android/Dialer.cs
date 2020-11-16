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