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
    [Activity(Label = "Forms")]
    public class Forms : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.download_view);
        }

        public override void OnBackPressed()
        {
            Finish();
        }
    }
}