using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MusicApp.Models;
using TagLib;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Plugin.Maui.Audio;



namespace MusicApp
{
    public class MainViewModel : ObservableObject
    {

        #region Fields
        private IAudioManager audioManager;
        private IAudioPlayer audioPlayer;
        public ObservableCollection<Song> Items { get; set; }

        private bool isLoadComplete = false;
        public bool IsPlaying { get; set; }

        private double _currentPosition;
        public double CurrentPosition
        {
            get
            {
                return this._currentPosition;
            }
            set
            {
                this._currentPosition = value;
                OnPropertyChanged();
            }
        }

        private string[] listMusic = GlobalVariables.listMusic;

        private string playButtonText = "▶";
        public string PlayButtonText
        {
            get
            {
                return this.playButtonText;
            }
            set
            {
                this.playButtonText = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoadComplete
        {
            get
            {
                return this.isLoadComplete;
            }
            set
            {
                this.isLoadComplete = value;
                OnPropertyChanged();
            }
        }

        private string title = "test";

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
                OnPropertyChanged();
            }
        }

        private string artist = "";

        public string Artist
        {
            get
            {
                return this.artist;
            }
            set
            {
                this.artist = value;
                OnPropertyChanged();
            }
        }

        private bool isRepeat;

        public bool IsRepeat
        {
            get
            {
                return this.isRepeat;
            }
            set
            {
                this.isRepeat = value;
                OnPropertyChanged();
            }
        }

        private string status = "";

        public string Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
                OnPropertyChanged();
            }
        }

        public Command PlayCommand { get; set; }
        public Command PauseCommand { get; set; }
        public Command StopCommand { get; set; }
        public object MessageBox { get; private set; }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        // Constructor
        #region MainViewModel
        [Obsolete]
        public MainViewModel()
        {
            this.audioManager = new AudioManager();
            generateMusic();

            IsPlaying = false;

            PlayCommand = new Command(OnPlayCommand);
            PauseCommand = new Command(OnPauseCommand);
            StopCommand = new Command(OnStopCommand);

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (audioPlayer != null && audioPlayer.IsPlaying)
                {
                    CurrentPosition = ((float)audioPlayer.CurrentPosition) / ((float)audioPlayer.Duration);
                }
                return true;
            });
        }


        public void OnPanUpdated(object sender, PanUpdatedEventArgs e, double width)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                case GestureStatus.Running:
                    // Calculate the new progress based on the position of the user's finger
                    double newProgress = e.TotalX / width;
                    // Set the new progress in your view model
                    CurrentPosition = newProgress;
                    break;
                case GestureStatus.Completed:

                    // Set the audio player's position to the new progress
                    audioPlayer.Seek(CurrentPosition * TimeSpan.FromSeconds(audioPlayer.Duration).TotalSeconds);

                    break;
            }
        }


        public async void generateMusic()
        {
            Items = new ObservableCollection<Song>();

            foreach (string music in listMusic)
            {
                var stream = await FileSystem.OpenAppPackageFileAsync(music);
                var tempFilePath = Path.ChangeExtension(Path.GetTempFileName(), "mp3");

                using (var tempFileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
                {
                    await stream.CopyToAsync(tempFileStream);
                }

                var file = TagLib.File.Create(tempFilePath);
                MediaFile mediaFile = new MediaFile(file);
                Items.Add(new Song
                {
                    Title = mediaFile.Title,
                    Artist = mediaFile.Artist,
                    Album = mediaFile.Album,
                    Picture = "musicpng.png",
                    MusicURL = music
                }); ;
            }
            GlobalVariables.ListAllMusic = Items;
        }

        public async void SetMusic(string queryMusic)
        {

            foreach (Song song in Items)
            {
                if (song.Title == queryMusic)
                {
                    audioPlayer = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(song.MusicURL.ToString()));
                    IsLoadComplete = true;
                    Title = song.Title;
                    Artist = song.Artist;

                    break;

                }
            }
        }


        public async void nextMuisc(String titleCurrent)
        {

            if (Items[Items.Count - 1].Title == titleCurrent)
            {
                SetMusic(Items[0].Title);
                Title = Items[0].Title;
                Artist = Items[0].Artist;
                return;
            }

            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Title == titleCurrent)
                {
                    SetMusic(Items[i + 1].Title);
                    Title = Items[i + 1].Title;
                    Artist = Items[i + 1].Artist;

                    return;
                }
            }
        }

        public void AudioPlayer_Completion(object sender, EventArgs e)
        {

            // Check if repeat is enabled
            if (IsRepeat)
            {
                // Set current position to 0 to replay the same song
                CurrentPosition = 0;
                audioPlayer.Loop = true;

            }
            else
            {
                audioPlayer.Loop = false;
                // Play next music

            }
        }

        public async void backMuisc(String titleCurrent)
        {

            if (Items[0].Title == titleCurrent)
            {
                SetMusic(Items[Items.Count - 1].Title);

                return;
            }

            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Title == titleCurrent)
                {
                    SetMusic(Items[i - 1].Title);


                    return;
                }
            }
        }

        public async Task<Song> findSong(string title)
        {

            foreach (Song song in Items)
            {
                if (song.Title == title)
                {
                    return song;
                }
            }
            return null;

        }

        #endregion
        #region OnPlayCommand
        void OnPlayCommand()
        {
            audioPlayer.Play();
            Status = "Play";
        }
        #endregion
        #region OnPauseCommand
        void OnPauseCommand()
        {

            if (audioPlayer.IsPlaying)
            {
                audioPlayer.Pause();
                PlayButtonText = "▶";
            }
            else
            {
                audioPlayer.Play();
                PlayButtonText = "||";
            }
        }
        #endregion
        #region OnStopCommand
        void OnStopCommand()
        {
            if (audioPlayer.IsPlaying)
            {
                audioPlayer.Stop();
                Status = "Stop";
            }
        }
        #endregion
    }
}