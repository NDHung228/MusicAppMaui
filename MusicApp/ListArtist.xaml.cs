using AndroidX.Lifecycle;
using Models;
using MusicApp.Models;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace MusicApp;

public partial class ListArtist : ContentPage
{
    private MainViewModel viewModel;
    List<PlayList> listArtist = new List<PlayList>();

    public ListArtist(MainViewModel viewModel)
    {
        InitializeComponent();

        this.viewModel = viewModel;

        generateListArtist();
        ListAllArtist.ItemsSource = listArtist;

    }

    private async void HomePage(object sender, EventArgs e)
    {

        if (Navigation != null)
        {
            Navigation.PopAsync();

        }

    }

    private void Cick_Artist(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            PlayList selectedItem = e.SelectedItem as PlayList;
            ListSongs listSong = new ListSongs(selectedItem, viewModel);
            // Do something with the selected item, e.g. navigate to a new page
            Navigation.PushAsync(listSong);
        }
    }

    private void generateListArtist()
    {
        var items = GlobalVariables.ListAllMusic;

        List<string> nameOfArtists = new List<string>();

        foreach (var song in items)  // get all name of artist in list music   
        {
            if (!nameOfArtists.Contains(song.Artist))
            {
                nameOfArtists.Add(song.Artist);
            }
        }

        foreach (var artist in nameOfArtists)
        {
            // create a playlist for each artist
            var songs = items.Where(song => song.Artist == artist).ToList();
            var playlist = new PlayList { Name = artist, Picture = "musicpng.png", Songs = songs };

            // add the playlist to the listArtist only if it doesn't exist yet
            if (!listArtist.Any(pl => pl.Name == artist))
            {
                listArtist.Add(playlist);
            }
        }


    }

}
