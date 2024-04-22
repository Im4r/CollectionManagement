using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CollectionManagement.Models;
using CommunityToolkit.Mvvm.Input;
using CollectionManagement.Views;
using CommunityToolkit.Maui.Views;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Collections.Specialized;
using System.ComponentModel;
using Microsoft.VisualBasic;

namespace CollectionManagement.ViewModels
{
    public partial class ItemsViewModel : ObservableObject
    {
        private ObservableCollection<ItemModel> _allItems = new ObservableCollection<ItemModel>();
        private ObservableCollection<ItemModel> _sortedItems;
        private readonly string FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "items.txt");
        public ObservableCollection<ItemModel> Items
        {
            get => _sortedItems;
            set => SetProperty(ref _sortedItems, value);
        }

        private static ItemsViewModel _instance;
        public static ItemsViewModel Instance => _instance ??= new ItemsViewModel();

        public string CollectionId;

        private NewItemPopup? currentPopup;
        private EditItemPopup? editPopup;
        private ItemModel? _currentEditingItem;

        [ObservableProperty]
        private string appDataPath;

        [ObservableProperty]
        public string name;

        [ObservableProperty]
        public string price;

        [ObservableProperty]
        public string status;

        [ObservableProperty]
        public string rating;

        [ObservableProperty]
        public string description;

        [ObservableProperty]
        public string customValue;

        [ObservableProperty]
        public string customValueName;

        [ObservableProperty]
        public string imagePath;
        public ItemsViewModel()
        {
            GetAppDataPath();
            _allItems.CollectionChanged += HandleCollectionChanged;
            Items = new ObservableCollection<ItemModel>(_allItems.OrderBy(item => item.Status == "sprzedane"));
            LoadItemsData();
        }
        private void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
                foreach (INotifyPropertyChanged item in e.OldItems)
                    item.PropertyChanged -= HandleItemPropertyChanged;

            if (e.NewItems != null)
                foreach (INotifyPropertyChanged item in e.NewItems)
                    item.PropertyChanged += HandleItemPropertyChanged;

            ReorderItems();
        }
        private void HandleItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ItemModel.Status))
                ReorderItems();
        }

        private void ReorderItems()
        {
            var sorted = _allItems.OrderBy(item => item.Status == "sprzedane").ToList();
            _allItems.Clear();
            foreach (var item in sorted)
                _allItems.Add(item);

            OnPropertyChanged(nameof(Items));
        }
        private void GetAppDataPath()
        {
            appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Debug.WriteLine("Path : " + appDataPath);
        }

        [RelayCommand]
        public void AddElement()
        {
            currentPopup = new NewItemPopup();
            Shell.Current.CurrentPage.ShowPopup(currentPopup);
        }

        [RelayCommand]
        public async void DeleteElement(ItemModel Item)
        {
            if(Item != null)
            {
                Items.Remove(Item);
                await SaveData();
            }
        }

        [RelayCommand]
        public async Task AddItem()
        {
            if (currentPopup == null)
                return;

            if (Name != null && Price != null && Status != null && Rating != null && Description != null && ImagePath != null)
            {
                ItemModel newItem = new ItemModel
                {
                    Id = NanoidDotNet.Nanoid.Generate(size: 5),
                    Collectionid = CollectionId,
                    Name = Name,
                    Price = double.Parse(Price),
                    Status = Status,
                    Rating = int.Parse(Rating),
                    Description = Description,
                    CustomValue = CustomValue,
                    CustomValueName = CustomValueName,
                    ImagePath = ImagePath
                };

                Items.Add(newItem);
                currentPopup.Close();

                Name = "";
                Price = "";
                Rating = "";
                Status = "";
                Description = "";
                CustomValueName = "";
                customValue = "";
                await SaveData();
            }
        }

    [RelayCommand]
        public async Task PickImage()
        {
            var fileResult = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Wybierz zdjecie",
                FileTypes = FilePickerFileType.Images
            });

            if (fileResult != null)
            {
                ImagePath = fileResult.FullPath;
            }
        }


        [RelayCommand]
        public void EditItem(ItemModel item)
        {
            _currentEditingItem = item;
            editPopup = new EditItemPopup(item);
            Shell.Current.CurrentPage.ShowPopup(editPopup);
        }

        [RelayCommand]
        public async Task UpdateItem()
        {
            if (_currentEditingItem != null && editPopup != null)
            {
                _currentEditingItem.Name = Name;
                _currentEditingItem.Description = Description;
                _currentEditingItem.Price = double.Parse(Price);
                _currentEditingItem.Status = Status;
                _currentEditingItem.Rating = int.Parse(Rating);
                _currentEditingItem.CustomValue = CustomValue;
                _currentEditingItem.CustomValueName = CustomValueName;
                _currentEditingItem.ImagePath = ImagePath;

                editPopup.Close();
                editPopup = null;
                _currentEditingItem = null;


                Name = "";
                Status = "";
                Price = "";
                Rating = "";
                Description = "";
                ImagePath = "";
                CustomValueName = "";
                CustomValue = "";

               await SaveData();
            }
        }

        [RelayCommand]
        public void ClosePopup()
        {
            if (currentPopup != null)
            {
                currentPopup.Close();
                currentPopup = null;
            }
        }
        [RelayCommand]
        public void CloseEditPopup()
        {
            if (editPopup != null)
            {
                editPopup.Close();
                editPopup = null;
                _currentEditingItem = null;
            }
        }

        public async Task SaveData()
        {
                var lines = Items.Select(c => $"{c.Collectionid}**{c.Id}**{c.Name}**{c.Status}**{c.Price}**{c.Rating}**{c.Description}**{c.CustomValueName}**{c.CustomValue}**{c.ImagePath}").ToList();
                await File.WriteAllLinesAsync(FilePath, lines);
        }

        public async Task LoadItemsData()
        {
            if (File.Exists(FilePath))
            {
                var lines = await File.ReadAllLinesAsync(FilePath);
                foreach (var line in lines)
                {
                    var parts = line.Split("**");
                    if (parts.Length == 10)
                    {
                        Items.Add(new ItemModel { Collectionid = parts[0], Id = parts[1], Name = parts[2], Status = parts[3], Price = double.Parse(parts[4]),Rating = int.Parse(parts[5]), Description = parts[6], CustomValueName = parts[7], CustomValue = parts[8], ImagePath = parts[9]  });
                    }
                }
            }

        }

    }
}
