using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Maui.Controls;

namespace kolekcjonowanie.Views
{
    public partial class MainPage : ContentPage
    {
        public string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "collections");
        public ObservableCollection<Models.Collection> Collections { get; set; } = new ObservableCollection<Models.Collection>();

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            readCollections();
            Directory.CreateDirectory(path);
            Trace.WriteLine(path);
        }

        public void readCollections()
        {
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    string content = File.ReadAllText(file);
                    string[] items = content.Split('|');
                    Collections.Add(new Models.Collection { FileName = Path.GetFileName(file).Substring(0, Path.GetFileName(file).Length - 4), Items = items });
                }
            }
        }

        private async void collectioniew_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count != 0)
            {
                var collection = (Models.Collection)e.CurrentSelection[0];
                var navigationParameter = new Dictionary<string, object>
                {
                    { "Items",  collection.Items },
                    { "Collection", collection },
                    { "MainPage", this } 
                };
                await Shell.Current.GoToAsync("////CollectionPage", navigationParameter);
                collectionView.SelectedItem = null;
            }
        }

        private void addCollection(object sender, EventArgs e)
        {
            File.WriteAllText(Path.Combine(path, $"{editor.Text}.txt"), "");
            Collections.Add(new Models.Collection { FileName = editor.Text, Items = Array.Empty<string>() });
        }
    }

}
