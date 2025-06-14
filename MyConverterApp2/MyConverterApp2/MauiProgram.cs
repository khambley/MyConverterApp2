using epj.Expander.Maui;
using Microsoft.Extensions.Logging;


namespace MyConverterApp2;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseExpander()
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
