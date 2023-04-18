using CommunityToolkit.Maui.Views;
using Models;
using MusicApp.Models;

using Acr.UserDialogs;

using CommunityToolkit.Maui.Alerts;
using static System.Net.Mime.MediaTypeNames;

namespace MusicApp;


public partial class MyPage : ContentPage
{
    private async void ShowCreatePopup()
    {
        var createPopup = new CreatePopup(this);

    }
}
public partial class CreatePopup : Popup
{

    private Page _parentPage;

    public CreatePopup(Page parentPage)
    {
        InitializeComponent();
        _parentPage = parentPage;
    }


    MainViewModel viewModel;
    String titleAddPlayList;


    public CreatePopup(MainViewModel viewModel, String title)
    {
        this.viewModel = viewModel;
        titleAddPlayList = title;
        InitializeComponent();

        PlaylistsItem.ItemsSource = GlobalVariables.playlist;

    }

    public CreatePopup()
    {

    }



    private async void CreatePlaylistButton_Clicked(object sender, EventArgs e)
    {
        // Get the name of the new playlist from the Entry control
        string playlistName = NamePlayList.Text;

        List<Song> songs = new List<Song>();
        Song song = await viewModel.findSong(titleAddPlayList);

        songs.Add(song);
        GlobalVariables.playlist.Add(new PlayList() { Name = playlistName, Picture = "musicpng.png", Songs = songs });

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        double fontSize = 14;
        string text = "Tạo một danh sách nhạc " +playlistName + " thành công với bài " +titleAddPlayList;
        var toast = Toast.Make(text, CommunityToolkit.Maui.Core.ToastDuration.Long, fontSize);
        await toast.Show(cancellationTokenSource.Token);
        Close();
    }

    private async void addMusicToPlayList(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {
            PlayList selectPlaylist = e.CurrentSelection[0] as PlayList;
            Song song = await viewModel.findSong(titleAddPlayList);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            double fontSize = 14;

            string text = "Thêm nhạc " + titleAddPlayList + " vào danh sách nhạc " + selectPlaylist.Name;
            for (int i = 0; i < selectPlaylist.Songs.Count; i++)
            {
                
                if (titleAddPlayList.Equals(selectPlaylist.Songs[i].Title))
                {
                    text = "Nhạc " + titleAddPlayList + " đã có trong danh sách nhạc " + selectPlaylist.Name;
                    var toast1 = Toast.Make(text, CommunityToolkit.Maui.Core.ToastDuration.Short, fontSize);
                    await toast1.Show(cancellationTokenSource.Token);
                    Close();

                    return;
                }
            }
           
            var toast = Toast.Make(text, CommunityToolkit.Maui.Core.ToastDuration.Short, fontSize);
            await toast.Show(cancellationTokenSource.Token);
           
            selectPlaylist.Songs.Add(song);

            Close();
        }

    }

}