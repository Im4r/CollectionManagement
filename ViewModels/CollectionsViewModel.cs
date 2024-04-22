using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CollectionManagement.Models;
using CommunityToolkit.Mvvm.Input;

namespace CollectionManagement.ViewModels
{
    public partial class CollectionsViewModel : ObservableObject
    {
        private static CollectionsViewModel _instance;
        public static CollectionsViewModel Instance => _instance ??= new CollectionsViewModel();
        private readonly string FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "collections.txt");
        public ObservableCollection<CollectionModel> Collections { get; set; } = [];

        [RelayCommand]
        public async Task AddCollection()
        {
            string result = await Application.Current.MainPage.DisplayPromptAsync("Podaj nazwe kolekcji","");
            if (result != null)
            {
                var newCollection = new CollectionModel { Id=NanoidDotNet.Nanoid.Generate(size: 5), Name=result};
                Collections.Add(newCollection);
                await SaveCollection();
            }
        }

        public CollectionsViewModel()
        {
            LoadCollections();
        }

        [ObservableProperty]
        public CollectionModel selectedCollection;

        [RelayCommand]
        public async Task GoToCollectionPage()
        {
            await Shell.Current.GoToAsync($"{nameof(CollectionPage)}?CollectionId={SelectedCollection.Id}&CollectionName={SelectedCollection.Name}");
        }

        public async Task SaveCollection()
        {
                var lines = Collections.Select(c => $"{c.Id}**{c.Name}").ToList();
                await File.WriteAllLinesAsync(FilePath, lines);
        }
        public async Task LoadCollections()
        {
            if (File.Exists(FilePath))
            {
                var lines = await File.ReadAllLinesAsync(FilePath);
                foreach (var line in lines)
                {
                    var parts = line.Split("**");
                    if (parts.Length == 2)
                    {
                        Collections.Add(new CollectionModel { Id = parts[0], Name = parts[1] });
                    }
                }
            }

        }

    }
}
