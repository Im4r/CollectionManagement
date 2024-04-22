using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CollectionManagement.Models
{
    public partial class ItemModel : ObservableObject
    {
        [ObservableProperty]
        public string id;

        [ObservableProperty]
        public string collectionid;

        [ObservableProperty]
        public string customValue;

        [ObservableProperty]
        public string customValueName;

        [ObservableProperty]
        public string name;

        [ObservableProperty]
        public double price;

        [ObservableProperty]
        private string status;

        [ObservableProperty]
        private string imagePath;
        public double Opacity
        {
            get
            {
                return (status.Equals("sprzedane", StringComparison.OrdinalIgnoreCase) ||
                        status.Equals("sprzedany", StringComparison.OrdinalIgnoreCase) ||
                        status.Equals("Sprzedany", StringComparison.OrdinalIgnoreCase) ||
                        status.Equals("Sprzedane", StringComparison.OrdinalIgnoreCase)) ? 0.5 : 1.0;
            }
        }

        [ObservableProperty]
        public int rating;

        [ObservableProperty]
        public string description;

    }
}
