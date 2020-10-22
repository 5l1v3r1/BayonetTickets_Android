using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Telephony;
using Android.Widget;
using Newtonsoft.Json;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using AlertDialog = Android.App.AlertDialog;
using Button = Android.Widget.Button;
using CheckBox = Android.Widget.CheckBox;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;

namespace BayonetTickets_Android
{
    [Activity(Label = "Bayonet Tickets", Theme= "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    { 
        static string AUTH_TOKEN;
        static string USER_ID;
        const string API_URL = "https://bayonetchat.com/api/v1/";
        const string BOT_NAME = "bayonet.tickets";
        const string BOT_PASSWORD = "bayonet-9";
        const string ROOM_ID = "nZucG9euMFuyoq9e7";
        public static RestClient client = new RestClient(API_URL);
        Page page = new Page();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            AppCenter.Start("576b46f7-5eb3-4a49-88fa-309341fb2054", typeof(Distribute), typeof(Analytics), typeof(Crashes));
            Distribute.SetEnabledAsync(true);
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Button submit = FindViewById<Button>(Resource.Id.submitButton);
            submit.Click += async delegate
            {
                 await OnSubmitClickedAsync();
            };

            //login to API on startup
            Task.Run(() => LoginToAPI());
        }

        void ClearForm()
        {
            CheckBox hudsonCheckBox = FindViewById<CheckBox>(Resource.Id.hudsonCheckBox);
            CheckBox tampaCheckBox = FindViewById<CheckBox>(Resource.Id.tampaCheckBox);
            CheckBox orlandoCheckBox = FindViewById<CheckBox>(Resource.Id.orlandoCheckBox);

            CheckBox androidCheckBox = FindViewById<CheckBox>(Resource.Id.androidCheckBox);
            CheckBox appleCheckBox = FindViewById<CheckBox>(Resource.Id.appleCheckBox);

            EditText empName = FindViewById<EditText>(Resource.Id.nameText);
            EditText issueBox = FindViewById<EditText>(Resource.Id.issueText);

            hudsonCheckBox.Checked = false;
            tampaCheckBox.Checked = false;
            orlandoCheckBox.Checked = false;
            androidCheckBox.Checked = false;
            appleCheckBox.Checked = false;
            empName.Text = "";
            issueBox.Text = "";
        }

        void LoginToAPI()
        {
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

        void PostMessageToChat(string ticket, string user)
        {
            var ticketRequest = new RestRequest("chat.postMessage", Method.POST);
            ticketRequest.AddHeader("X-Auth-Token", AUTH_TOKEN);
            ticketRequest.AddHeader("X-User-Id", USER_ID);
            ticketRequest.AddHeader("Content-Type", "application/json");

            ticketRequest.AddJsonBody((new { text = ticket, roomId = ROOM_ID, alias = user }));

            client.Execute(ticketRequest);

            
        }

        public Task<bool> DisplayNotification()
        {
            var tcs = new TaskCompletionSource<bool>();
            string message = "Your ticket has been submitted." + "\n" + "Please select ok and schedule a date and time to meet with the IT Department.";
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


        void DisplayFailureNotice(string reason)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Not Enough Information");
            alert.SetMessage(reason);
            Dialog dialog = alert.Create();
            dialog.Show();
        }

        string DetermineLocation(CheckBox hudson, CheckBox tampa, CheckBox orlando)
        {
            if (hudson.Checked)
                return "Hudson";
            if (tampa.Checked)
                return "Tampa";
            if (orlando.Checked)
                return "Orlando";
            return "NULL";
        }

        string DetermineDevice(CheckBox android, CheckBox apple)
        {
            if (apple.Checked)
                return "Apple";
            if (android.Checked)
                return "Android";
            return "NULL";
        }

        private async Task<string> GetPhoneNumberAsync()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync<PhonePermission>();
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Phone))
                    {
                        await page.DisplayAlert("Need Phone Number", "We need your phone number", "OK");
                    }
                    status = await CrossPermissions.Current.RequestPermissionAsync<PhonePermission>();
                }

                if (status == PermissionStatus.Granted)
                {
                    TelephonyManager mTelephonyMgr;
                    mTelephonyMgr = (TelephonyManager)GetSystemService(Context.TelephonyService);
                    string number = mTelephonyMgr.Line1Number.ToString();
                    return number;
                }
                else if (status != PermissionStatus.Unknown)
                {
                    return "Permission Denied";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Idk what happened, this should be unreachable code";
        }

        async Task OnSubmitClickedAsync()
        {
            CheckBox hudsonCheckBox = FindViewById<CheckBox>(Resource.Id.hudsonCheckBox);
            CheckBox tampaCheckBox = FindViewById<CheckBox>(Resource.Id.tampaCheckBox);
            CheckBox orlandoCheckBox = FindViewById<CheckBox>(Resource.Id.orlandoCheckBox);

            CheckBox androidCheckBox = FindViewById<CheckBox>(Resource.Id.androidCheckBox);
            CheckBox appleCheckBox = FindViewById<CheckBox>(Resource.Id.appleCheckBox);

            EditText empName = FindViewById<EditText>(Resource.Id.nameText);
            EditText issueBox = FindViewById<EditText>(Resource.Id.issueText);

            //no location
            if (!hudsonCheckBox.Checked && !tampaCheckBox.Checked && !orlandoCheckBox.Checked)
            {
                DisplayFailureNotice("No location checked.");
                return;
            }

            //no device type
            if (!appleCheckBox.Checked && !androidCheckBox.Checked)
            {
                DisplayFailureNotice("No device type checked.");
                return;
            }

            //no name
            string name = empName.Text;
            if(name.Equals(""))
            {
                DisplayFailureNotice("No employee name specified.");
                return;
            }

            string issue = issueBox.Text;

            //no issue
            if(issue.Equals(""))
            {
                DisplayFailureNotice("No issue entered.");
                return;
            }

            string location = DetermineLocation(hudsonCheckBox, tampaCheckBox, orlandoCheckBox);
            string device = DetermineDevice(androidCheckBox, appleCheckBox);
            string number = await GetPhoneNumberAsync();
            string ticket = "";

            ticket += "Name: " + name + "\n";
            ticket += "Number: " + number + "\n";
            ticket += "Location: " + location + "\n";
            ticket += "Device: " + device + "\n";
            ticket += "Issue: " + issue + "\n";

            await Task.Run(() => PostMessageToChat(ticket, name));
            await DisplayNotification();
            Task.Run(() => ClearForm());
            GoToActivity(typeof(Calendly));
        }

        public void GoToActivity(Type myActivity)
        {
            StartActivity(myActivity);
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}
