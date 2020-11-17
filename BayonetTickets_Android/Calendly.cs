using Android.App;
using Android.OS;
using Android.Views;
using Android.Webkit;

namespace BayonetTickets_Android
{
    [Activity(Label = "Calendly")]
    public class Calendly : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Window.RequestFeature(WindowFeatures.NoTitle);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.calendly_view);
            WebView webView = FindViewById<WebView>(Resource.Id.webView);
            webView.SetWebViewClient(new WebViewClient());
            webView.LoadUrl("https://calendly.com/bayonet-it");
            webView.Settings.JavaScriptEnabled = true;
            webView.Settings.BuiltInZoomControls = false;
            webView.ScrollBarStyle = ScrollbarStyles.InsideInset;
            webView.ScrollbarFadingEnabled = true;
        }

        public override void OnBackPressed()
        {
            WebView webView = FindViewById<WebView>(Resource.Id.webView);
            if (webView.CanGoBack())
                webView.GoBack();
            else
                Finish();
        }
    }
}