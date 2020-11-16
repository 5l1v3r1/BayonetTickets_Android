using Android.App;
using System.Threading.Tasks;

namespace BayonetTickets_Android
{
    class Display : MainActivity
    {
        public Display()
        {
            //default constructor, no real use
        }

        public Task<bool> DisplayNotification()
        {
            var tcs = new TaskCompletionSource<bool>();
            string message = "Your ticket has been submitted." + "\n\n" + "Please press ok and schedule a date and time to meet with the IT Department.";
            AlertDialog.Builder alert = new AlertDialog.Builder(this).SetPositiveButton("OK", (sender, args) =>
            {
                tcs.SetResult(true);
            })
            .SetTitle("Ticket Submitted")
            .SetMessage(message);

            Dialog dialog = alert.Create();
            dialog.Show();

            return tcs.Task;
        }

        public void DisplayFailureNotice(string reason)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Not Enough Information");
            alert.SetMessage(reason);
            Dialog dialog = alert.Create();
            dialog.Show();
        }
    }
}