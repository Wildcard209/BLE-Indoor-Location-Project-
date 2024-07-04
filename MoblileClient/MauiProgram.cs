using Microsoft.Extensions.Configuration;
using MobileClient.ViewModel;
using System.Reflection.Metadata;

namespace MobileClient;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
#if ANDROID
		builder.Services.AddSingleton<MainPage>();

		builder.Services.AddSingleton<MainViewModel>();
#endif

        return builder.Build();
	}
}
