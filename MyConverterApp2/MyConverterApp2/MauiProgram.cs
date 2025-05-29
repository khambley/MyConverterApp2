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
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		// enable animations for Android & iOS
        Expander.EnableAnimations();

		return builder.Build();
	}
}
