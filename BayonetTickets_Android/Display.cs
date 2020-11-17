using Android.App;
using System.Threading.Tasks;

namespace BayonetTickets_Android
{
    class Display
    {
        public static Task<bool> DisplayNotification()
        {
            MainActivity main = new MainActivity();
            var tcs = new TaskCompletionSource<bool>();
            string message = "Your ticket has been submitted." + "\n\n" + "Please press ok and schedule a date and time to meet with the IT Department.";
            AlertDialog.Builder alert = new AlertDialog.Builder(main).SetPositiveButton("OK", (sender, args) =>
            {
                tcs.SetResult(true);
            })
            .SetTitle("Ticket Submitted")
            .SetMessage(message);

            Dialog dialog = alert.Create();
            dialog.Show();

            return tcs.Task;
        }

        public static Task<string> DisplayConfirmationNotice(string reason)
        {
            MainActivity main = new MainActivity();
            var tcs = new TaskCompletionSource<string>();
            AlertDialog.Builder alert = new AlertDialog.Builder(main);
            alert.SetTitle("Open Phone Dialer");
            alert.SetMessage("You are trying to call the " + reason + ".\nIf this is correct, press Call.\nIf not, press Cancel.");
            alert.SetPositiveButton("Call", (senderAlert, args) => {
                tcs.SetResult("OK");
            });
            alert.SetNegativeButton("Cancel", (senderAlert, args) => {
                tcs.SetResult("Cancel");
            });
            Dialog dialog = alert.Create();
            dialog.Show();

            return tcs.Task;
        }

        public static Task<string> DisplayFailureNotice(string reason)
        {
            MainActivity main = new MainActivity();
            var tcs = new TaskCompletionSource<string>();
            AlertDialog.Builder alert = new AlertDialog.Builder(main);
            alert.SetTitle("Not Enough Information");
            alert.SetMessage(reason);
            alert.SetPositiveButton("OK", (senderAlert, args) => {
                tcs.SetResult("OK");
            });
            Dialog dialog = alert.Create();
            dialog.Show();

            return tcs.Task;
        }
    }
}