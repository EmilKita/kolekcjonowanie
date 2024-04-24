using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace kolekcjonowanie.Views;
[QueryProperty(nameof(Items), "Items")]
[QueryProperty(nameof(Collection), "Collection")] 
[QueryProperty(nameof(MainPage), "MainPage")]
public partial class CollectionPage : ContentPage
{
    private MainPage _mainPage;
    private Models.Collection _collection;
    public string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "collections");

    public ObservableCollection<Models.Item> ItemsCollection { get; set; } = new ObservableCollection<Models.Item>();

    public string[] Items
    {
        set
        {
            foreach (string item in value)
            {
                ItemsCollection.Add(new Models.Item() { Name = item });
            }
        }
    }

    public MainPage MainPage
    {
        set
        {
            _mainPage = value;
        }
    }

    public Models.Collection Collection
    {
        set
        {
            _collection = value;
        }
    }

    public CollectionPage()
	{
		InitializeComponent();
        BindingContext = this;
    }

    private async void back_clicked(object sender, EventArgs e)
    {
        ItemsCollection.Clear();
        await Shell.Current.GoToAsync("////MainPage");
    }

    private void Add_item(object sender, EventArgs e)
    {
        string newItemName = editor.Text;
        ItemsCollection.Add(new Models.Item { Name = newItemName });

        int index = _mainPage.Collections.IndexOf(_collection);
        string[] itemsArray = ItemsCollection.Select(item => item.Name).ToArray();
        _mainPage.Collections[index].Items = itemsArray;

        string joinedItems = string.Join("|", itemsArray);
        File.WriteAllText(Path.Combine(path, $"{_collection.FileName}.txt"), joinedItems);
    }

    private async void itemsCollection_Select(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count != 0)
        {
            var item = (Models.Item)e.CurrentSelection[0];
            string[] itemsArray = ItemsCollection.Select(item => item.Name).ToArray();
            var navigationParameter = new Dictionary<string, object>
                {
                    { "Item", item.Name },
                    { "Items", itemsArray },
                    { "Collection", _collection },
                    { "MainPage", _mainPage }
                };
            await Shell.Current.GoToAsync("////EditItemPage", navigationParameter);
            ItemsCollection.Clear();
            itemsCollectionView.SelectedItem = null;
        }
    }
}
