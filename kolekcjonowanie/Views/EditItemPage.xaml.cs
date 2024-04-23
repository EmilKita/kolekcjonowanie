using System.Diagnostics;

namespace kolekcjonowanie.Views;

[QueryProperty(nameof(Item), "Item")]
[QueryProperty(nameof(Collection), "Collection")]
[QueryProperty(nameof(MainPage), "MainPage")]
[QueryProperty(nameof(Items), "Items")]

public partial class EditItemPage : ContentPage
{
    private MainPage _mainPage;
    private Models.Collection _collection;
    private string _item;
    private string[] _items;

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

    public string Item
    {
        set
        {
            _item = value;
        }
    }
    public string[] Items
    {
        set
        {
            _items = value;
        }
    }

    public EditItemPage()
	{
		InitializeComponent();
	}

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        int index = Array.IndexOf(_items, _item);
        _items = _items.Where((source, i) => i != index).ToArray();

        int collectionIndex = _mainPage.Collections.IndexOf(_collection);
        _mainPage.Collections[collectionIndex].Items = _items;

        string joinedItems = string.Join("|", _items);
        File.WriteAllText(Path.Combine(_mainPage.path, $"{_collection.FileName}.txt"), joinedItems);

        var navigationParameter = new Dictionary<string, object>
                {
                    { "Items",  _items },
                    { "Collection", _collection },
                    { "MainPage", _mainPage }
                };
        await Shell.Current.GoToAsync("////CollectionPage", navigationParameter);
    }

    private async void EditButton_Clicked(object sender, EventArgs e)
    {
        string newName = editor.Text;
        int index = Array.IndexOf(_items, _item);
        _items[index] = newName;


        int collectionIndex = _mainPage.Collections.IndexOf(_collection);
        _mainPage.Collections[collectionIndex].Items = _items;


        string joinedItems = string.Join("|", _items);
        File.WriteAllText(Path.Combine(_mainPage.path, $"{_collection.FileName}.txt"), joinedItems);

        var navigationParameter = new Dictionary<string, object>
        {
            { "Items",  _items },
            { "Collection", _collection },
            { "MainPage", _mainPage }
        };
        await Shell.Current.GoToAsync("////CollectionPage", navigationParameter);
    }

}