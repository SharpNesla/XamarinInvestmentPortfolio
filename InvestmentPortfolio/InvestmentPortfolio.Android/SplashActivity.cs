using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.AppCompat.App;
using InvestmentPortfolio.Droid;
using System.Threading.Tasks;

[Activity(Theme = "@style/MyTheme.Splash",
    MainLauncher = true,
    Icon = "@mipmap/icon", 
    NoHistory = true)]
public class SplashActivity : AppCompatActivity
{
    public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
    {
        base.OnCreate(savedInstanceState, persistentState);
    }

    // Launches the startup task
    protected override void OnResume()
    {
        base.OnResume();
        Task startupWork = new Task(() => { SimulateStartup(); });
        startupWork.Start();
    }
    // Simulates background work that happens behind the splash screen
    async void SimulateStartup()
    {
        await Task.Delay(700); // Simulate a bit of startup work.
        StartActivity(new Intent(Application.Context, typeof(MainActivity)));
    }
}