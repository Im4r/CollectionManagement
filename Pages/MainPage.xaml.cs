using CollectionManagement.ViewModels;

namespace CollectionManagement
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new CollectionsViewModel();
        }
    }

}
