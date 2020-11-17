using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BayonetTickets_Android
{
    class Listener
    {
        
        public static void ApplyButtonListeners()
        {
            MainActivity main = new MainActivity();
            Button submit = main.FindViewById<Button>(Resource.Id.submitButton);
            submit.Click += async delegate
            {
                await main.OnSubmitClickedAsync();
            };

            Button call = main.FindViewById<Button>(Resource.Id.callButton);
            call.Click += async delegate
            {
                string result = await Display.DisplayConfirmationNotice("IT Department");
                if (result.Equals("OK"))
                    await Dialer.OnCallButtonClick();
            };

            Button mechanic = main.FindViewById<Button>(Resource.Id.mechanicButton);
            mechanic.Click += async delegate
            {
                string result = await Display.DisplayConfirmationNotice("Mechanic Shop");
                if (result.Equals("OK"))
                    await Dialer.OnCallMechanicClick();
            };

            Button safety = main.FindViewById<Button>(Resource.Id.safetyButton);
            safety.Click += async delegate
            {
                string result = await Display.DisplayConfirmationNotice("Safety Coordinator");
                if (result.Equals("OK"))
                    await Dialer.OnCallSafetyButtonClick();
            };
        }

        public static void ApplyCheckBoxListeners()
        {
            MainActivity main = new MainActivity();
            CheckBox hudsonCheckBox = main.FindViewById<CheckBox>(Resource.Id.hudsonCheckBox);
            CheckBox tampaCheckBox = main.FindViewById<CheckBox>(Resource.Id.tampaCheckBox);
            CheckBox orlandoCheckBox = main.FindViewById<CheckBox>(Resource.Id.orlandoCheckBox);
            CheckBox androidCheckBox = main.FindViewById<CheckBox>(Resource.Id.androidCheckBox);
            CheckBox appleCheckBox = main.FindViewById<CheckBox>(Resource.Id.appleCheckBox);
            hudsonCheckBox.CheckedChange += OnHudsonCheckChanged;
            tampaCheckBox.CheckedChange += OnTampaCheckChanged;
            orlandoCheckBox.CheckedChange += OnOrlandoCheckChanged;
            androidCheckBox.CheckedChange += OnAndroidCheckChanged;
            appleCheckBox.CheckedChange += OnAppleCheckChanged;
        }

        private static void OnHudsonCheckChanged(object sender, CheckBox.CheckedChangeEventArgs e)
        {
            MainActivity main = new MainActivity();
            CheckBox tampaCheckBox = main.FindViewById<CheckBox>(Resource.Id.tampaCheckBox);
            CheckBox orlandoCheckBox = main.FindViewById<CheckBox>(Resource.Id.orlandoCheckBox);
            if (e.IsChecked)
            {
                tampaCheckBox.Checked = false;
                orlandoCheckBox.Checked = false;
            }
        }

        private static void OnOrlandoCheckChanged(object sender, CheckBox.CheckedChangeEventArgs e)
        {
            MainActivity main = new MainActivity();
            CheckBox hudsonCheckBox = main.FindViewById<CheckBox>(Resource.Id.hudsonCheckBox);
            CheckBox tampaCheckBox = main.FindViewById<CheckBox>(Resource.Id.tampaCheckBox);
            if (e.IsChecked)
            {
                tampaCheckBox.Checked = false;
                hudsonCheckBox.Checked = false;
            }
        }

        private static void OnTampaCheckChanged(object sender, CheckBox.CheckedChangeEventArgs e)
        {
            MainActivity main = new MainActivity();
            CheckBox hudsonCheckBox = main.FindViewById<CheckBox>(Resource.Id.hudsonCheckBox);
            CheckBox orlandoCheckBox = main.FindViewById<CheckBox>(Resource.Id.orlandoCheckBox);
            if (e.IsChecked)
            {
                hudsonCheckBox.Checked = false;
                orlandoCheckBox.Checked = false;
            }
        }

        private static void OnAndroidCheckChanged(object sender, CheckBox.CheckedChangeEventArgs e)
        {
            MainActivity main = new MainActivity();
            CheckBox appleCheckBox = main.FindViewById<CheckBox>(Resource.Id.appleCheckBox);
            if (e.IsChecked)
            {
                appleCheckBox.Checked = false;
            }
        }

        private static void OnAppleCheckChanged(object sender, CheckBox.CheckedChangeEventArgs e)
        {
            MainActivity main = new MainActivity();
            CheckBox androidCheckBox = main.FindViewById<CheckBox>(Resource.Id.androidCheckBox);
            if (e.IsChecked)
            {
                androidCheckBox.Checked = false;
            }
        }
    }
}