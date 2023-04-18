using Models;
using MusicApp.Models;
using System.Collections.ObjectModel;

namespace MusicApp;

public partial class PlayLists : ContentPage
{

    List<PlayList> myList = new List<PlayList>();

    MainViewModel mainViewModel = new MainViewModel();

    private MainViewModel viewModel;
    private MainPage mainPage = new MainPage();


    public PlayLists(MainViewModel viewModel)
    {
        InitializeComponent();

        this.viewModel = viewModel;

        // Set the data source to the ListView
        myListView.ItemsSource = GlobalVariables.playlist;
    }



    private void myPlayList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            PlayList selectedItem = e.SelectedItem as PlayList;
            ListSongs listSong = new ListSongs(selectedItem, viewModel);
            // Do something with the selected item, e.g. navigate to a new page
            Navigation.PushAsync(listSong);
        }
    }

    private async void HomePage(object sender, EventArgs e)
    {

        if (Navigation != null)
        {
            Navigation.PopAsync();

        }
        else
        {
            await Shell.Current.Navigation.PushAsync(mainPage);
        }
    }

    private async void Button_ListArtist(object sender, EventArgs e)
    {
        ListArtist listArtist = new ListArtist(viewModel);
        await Navigation.PushAsync(listArtist);
    }



}

