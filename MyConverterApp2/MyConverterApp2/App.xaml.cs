namespace MyConverterApp2;

public partial class App : Application
{
	public App()
	{
		Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(Settings.SyncfusionLicenseKey);
		InitializeComponent();
		SentrySdk.CaptureMessage("✅ App launched successfully");

        // Optionally, add a breadcrumb too:
        SentrySdk.AddBreadcrumb("App constructor completed", category: "lifecycle");
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}