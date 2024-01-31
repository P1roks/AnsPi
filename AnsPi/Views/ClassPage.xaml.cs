using AnsPi.ViewModels;

namespace AnsPi.Views;

public partial class ClassPage : ContentPage
{
	public ClassPage()
	{
		InitializeComponent();

		// Force user to choose a class when the program loads
		Loaded += (s, e) =>
		{
			if(BindingContext is ClassViewModel viewModel)
			{
				if (viewModel.ChangeClassCommand.CanExecute(null))
				{
					viewModel.ChangeClassCommand.Execute(null);
				}
			}
		};
	}

}