using CollectionManagement.Models;
using CommunityToolkit.Maui.Views;
using CollectionManagement.ViewModels;

namespace CollectionManagement.Views
{
    public partial class EditItemPopup : Popup
    {
        public EditItemPopup(ItemModel item)
        {
            InitializeComponent();
            BindingContext = ItemsViewModel.Instance;

            var vm = BindingContext as ItemsViewModel;
            if (vm != null)
            {
                vm.Name = item.Name;
                vm.Price = item.Price.ToString();
                vm.Rating = item.Rating.ToString();
                vm.Status = item.Status;
                vm.Description = item.Description;
                vm.CustomValue = item.CustomValue;
                vm.CustomValueName = item.CustomValueName;
                vm.ImagePath = item.ImagePath;
            }
        }
    }
}
