using MobileClient.ViewModel;

namespace MobileClient;

public partial class MainPage : ContentPage
{

	public MainPage(
#if ANDROID 
		MainViewModel viewModel 
#endif
		)
	{
		#if ANDROID
		InitializeComponent();

        BindingContext = viewModel;
#endif
    }
}

