using epj.Expander.Maui;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;


namespace MyConverterApp2;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseSentry(options =>
        {
            // The DSN is the only required setting.
            options.Dsn = "https://19b9457f069cf4d375d2901ae34f93bc@o4509686457827328.ingest.us.sentry.io/4509686459858944";
            // Use debug mode if you want to see what the SDK is doing.
            // Debug messages are written to stdout with Console.Writeline,
            // and are viewable in your IDE's debug console or with 'adb logcat', etc.
            // This option is not recommended when deploying your application.
            options.Debug = true;
            // Adds request URL and headers, IP and name for users, etc.
            options.SendDefaultPii = true;

            // Other Sentry options can be set here.
        })
			.UseExpander()
			.ConfigureSyncfusionCore()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("OpenSans-Bold.ttf", "OpenSansBold");
				fonts.AddFont("FontAwesomeRegular.otf", "FontAwesomeSolid");
			})
			.RegisterAppTypes();

#if DEBUG
		builder.Logging.AddDebug();
#endif
		// enable animations for Android & iOS
		Expander.EnableAnimations();

		return builder.Build();
	}
	public static MauiAppBuilder RegisterAppTypes(this MauiAppBuilder mauiAppBuilder)
	{
		// Services
		mauiAppBuilder.Services.AddSingleton<Services.IRateService>((serviceProvider) => new Services.RateService());
		mauiAppBuilder.Services.AddSingleton<Services.ILengthService>((serviceProvider) => new Services.LengthService());

		// ViewModels
		mauiAppBuilder.Services.AddTransient<ViewModels.MainViewModel>();

		// Views
		mauiAppBuilder.Services.AddTransient<MainPage>();

		return mauiAppBuilder;
	}
}
