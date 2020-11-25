using Android.App;
using Android.OS;

namespace BayonetTickets_Android
{
    [Activity(Label = "Forms", Theme = "@style/AppTheme")]
    public class Forms : MainActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.download_view);
        }

        public void ApplyButtonListeners()
        {

        }

        public override void OnBackPressed()
        {
            Finish();
        }
    }
}