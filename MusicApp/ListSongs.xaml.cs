using MusicApp.Models;
using System.Runtime.CompilerServices;
using Models;


namespace MusicApp;

public partial class ListSongs : ContentPage
{
    private PlayList selectedItem;
    private List<Song> songs = new List<Song>();
    private MainViewModel viewModel;
    private String currentTitle;


    public ListSongs(PlayList selectedItem, MainViewModel mainViewModel)
    {

        InitializeComponent();

        this.viewModel = mainViewModel;
        this.selectedItem = selectedItem;
        LoadSongsAsync();
        BindingContext = viewModel;

    }

    public void LoadSongsAsync()
    {

        for (int i = 0; i < selectedItem.Songs.Count; i++)
        {
            songs.Add(new Song()
            {
                Title = selectedItem.Songs[i].Title

            }); ;
        }

        listSongs.ItemsSource = songs;
    }

    private async void ItemSong(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            bottomMusic.IsVisible = true;


            var selectedSong = e.SelectedItem as Song;

            var title = selectedSong.Title;
            currentTitle = title;
            viewModel.SetMusic(title);
            
        }
       
    }

    private void Button_NextMusic(object sender, EventArgs e)
    {
       
        if (songs[songs.Count - 1].Title == currentTitle)
        {
            viewModel.SetMusic(songs[0].Title);
            currentTitle = songs[0].Title;

            return;
        }

        for (int i = 0; i < songs.Count; i++)
        {
            if (songs[i].Title == currentTitle)
            {
                viewModel.SetMusic(songs[i+1].Title);
                currentTitle = songs[i+1].Title;

                return;
            }
        }
    }

    private void Button_BackMusic(object sender, EventArgs e)
    {
        if (songs[0].Title == currentTitle)
        {
            viewModel.SetMusic(songs[songs.Count-1].Title);
            currentTitle = songs[songs.Count - 1].Title;

            return;
        }

        for (int i = 0; i < songs.Count; i++)
        {
            if (songs[i].Title == currentTitle)
            {
                viewModel.SetMusic(songs[i - 1].Title);
                currentTitle = songs[i - 1].Title;

                return;
            }
        }
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
    private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        viewModel.OnPanUpdated(sender, e, progressBar.Width);
    }

}