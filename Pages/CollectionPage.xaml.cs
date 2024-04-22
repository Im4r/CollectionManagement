using CollectionManagement.ViewModels;

namespace CollectionManagement
{
    [QueryProperty(nameof(CollectionId), "CollectionId")]
    [QueryProperty(nameof(CollectionName), "CollectionName")]

    public partial class CollectionPage : ContentPage
    {
        private string _collectionId;
        private string _collectionName;

        public CollectionPage()
        {
            InitializeComponent();
            BindingContext = ItemsViewModel.Instance;
        }

        public string CollectionId
        {
            set
            {
                _collectionId = Uri.UnescapeDataString(value);
                var vm = BindingContext as ItemsViewModel;
                if (vm != null)
                {
                    vm.CollectionId = _collectionId;
                    OnPropertyChanged();
                }
            }
        }

        public string CollectionName
        {
            set
            {
                _collectionName = Uri.UnescapeDataString(value);
                Title = _collectionName;
                OnPropertyChanged();
            }
        }
    }
}
