using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Telephony;
using Android.Widget;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using CheckBox = Android.Widget.CheckBox;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using PermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;

namespace BayonetTickets_Android
{
    [Activity(Label = "Bayonet Tickets", Theme= "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    { 
        Page page = new Page();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            AppCenter.Start("576b46f7-5eb3-4a49-88fa-309341fb2054", typeof(Analytics), typeof(Crashes));
            Analytics.TrackEvent("Application Launched");
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            //setup button events
            Task.Run(() => Listener.ApplyButtonListeners());

            //setup checkbox listeners
            Task.Run(() => Listener.ApplyCheckBoxListeners());

            //login to API on startup
            Task.Run(() => BayonetChat.LoginToAPI());    
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
            return "Unknown Error";
        }

        public async Task OnSubmitClickedAsync()
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
                await Display.DisplayFailureNotice("No location checked.");
                return;
            }

            //no device type
            if (!appleCheckBox.Checked && !androidCheckBox.Checked)
            {
                await Display.DisplayFailureNotice("No device type checked.");
                return;
            }
            
            //no name
            string name = empName.Text;
            if(name.Equals(""))
            {
                await Display.DisplayFailureNotice("No employee name specified.");
                return;
            }

            string issue = issueBox.Text;

            //no issue
            if(issue.Equals(""))
            {
                await Display.DisplayFailureNotice("No issue entered.");
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

            await Task.Run(() => BayonetChat.PostMessageToChat(ticket, name));
            await Display.DisplayNotification();

            Analytics.TrackEvent("Ticket Submitted");
            
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
