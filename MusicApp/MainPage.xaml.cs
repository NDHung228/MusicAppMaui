

using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using Models;
using MusicApp.Models;
using Plugin.Maui.Audio;
using System.Collections.ObjectModel;

namespace MusicApp;

public partial class MainPage : ContentPage
{
    MainViewModel viewModel = new();

    
    public MainPage()
    {
        InitializeComponent();

        BindingContext = viewModel;
    }

    private void MyButton_Clicked(object sender, EventArgs e)
    {
        bottomMusic.IsVisible = true;

        var button = sender as ImageButton;
        var selectedItem = button.BindingContext as Song;
        var title = selectedItem.Title;

        viewModel.SetMusic(title);
    }

    private void Button_NextMusic(object sender, EventArgs e)
    {
        viewModel.nextMuisc(viewModel.Title);
    }

    private void Button_BackMusic(object sender, EventArgs e)
    {
        viewModel.backMuisc(viewModel.Title);
    }

    private void Button_RepeatMusic(object sender, EventArgs e)
    {
        if (viewModel.IsRepeat)
        {
           repeatButton.Source = "repeatoff.svg";
            viewModel.IsRepeat = false;
        }
        else
        {
           repeatButton.Source = "repeaton.svg";
            viewModel.IsRepeat = true;
        }

        viewModel.AudioPlayer_Completion(sender, e);
    }


    public void findSong(string title)
    {
        viewModel.SetMusic(title);
    }


    private void OnImageButtonClicked(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        var selectedItem = button.BindingContext as Song;
        var title = selectedItem.Title;


        this.ShowPopup(new CreatePopup(viewModel, title));
    }


    private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        viewModel.OnPanUpdated(sender, e, progressBar.Width);
    }

    private async void filePlayLists(object sender, EventArgs e)
    {
        PlayLists playlist = new PlayLists(viewModel);
        if (Navigation != null)
        {
            await Navigation.PushAsync(playlist);
        }
        else
        {
            await Shell.Current.Navigation.PushAsync(playlist);
        }
    }

    private async void Button_ListArtist(object sender, EventArgs e)
    {
        ListArtist listArtist = new ListArtist(viewModel);
        await Navigation.PushAsync(listArtist);
    }

    
}
