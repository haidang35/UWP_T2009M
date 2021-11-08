using MusicPlayerDemo.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicPlayerDemo.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyMusic : Page
    {
        private AuthService accountService = new AuthService();
        public MyMusic()
        {
            this.InitializeComponent();
            this.Loaded += GetMySong;
        }

        public void ChooseSongPlay(object sender, RoutedEventArgs e)
        {
            var songElementItem = sender as ToggleButton;
            if (PlayList.songIsPlaying == songElementItem.Tag.ToString()
                && mediaPlayerElement.MediaPlayer.CurrentState == Windows.Media.Playback.MediaPlayerState.Playing)
            {
                mediaPlayerElement.MediaPlayer.Pause();
                songElementItem.Content = "Play";
            }
            else
            {
                songElementItem.Content = "Pause";
                PlayList.songIsPlaying = songElementItem.Tag.ToString();
                ActiveSongChoosed(songElementItem.Tag.ToString());
            }
        }
        
         public void ActiveSongChoosed(string data)
        {
            var link = new Uri(data);
            if (link != null)
            {
                mediaPlayerElement.Source = MediaSource.CreateFromUri(link);
                mediaPlayerElement.MediaPlayer.Play();
            }
        }

        public async void GetMySong(object sender, RoutedEventArgs e)
        {
            songList.ItemsSource = await accountService.GetMySong();
        }


    }
}
