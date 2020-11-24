using Android.App;
using Android.OS;

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