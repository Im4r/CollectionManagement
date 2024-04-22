using CollectionManagement.ViewModels;
using CommunityToolkit.Maui.Views;

namespace CollectionManagement.Views;

public partial class NewItemPopup : Popup
{
	public NewItemPopup()
	{
		InitializeComponent();
		BindingContext = ItemsViewModel.Instance;
	}
}