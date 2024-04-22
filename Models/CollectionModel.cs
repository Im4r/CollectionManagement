using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionManagement.Models
{
    public partial class CollectionModel : ObservableObject
    {
        [ObservableProperty]
        public string id;

        [ObservableProperty]
        public string name;

    }
}
