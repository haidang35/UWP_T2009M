using MusicPlayerDemo.Entities;
using MusicPlayerDemo.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicPlayerDemo.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    
    public sealed partial class PlayList : Page
    {
        public delegate void PlaySong (string data);

        public PlaySong eventChoosePlaySong;
        public static string songIsPlaying;
        private Storyboard rotation = new Storyboard();

        public PlayList()
        {
            this.InitializeComponent();
            this.Loaded += GetSongListFromAPI;
        }

        public async void GetSongListFromAPI(object sender, RoutedEventArgs e)
        {
            var musicService = new MusicService();
            var songListData = await musicService.FindAll();
            LoadingEffect.Visibility = Visibility.Collapsed;
            songList.ItemsSource = songListData;
        }

        public void ChooseSongPlay(object sender, RoutedEventArgs e)
        {
            var songElementItem = sender as ToggleButton;
            if(songIsPlaying == songElementItem.Tag.ToString()
                && mediaPlayerElement.MediaPlayer.CurrentState == Windows.Media.Playback.MediaPlayerState.Playing)
            {
                mediaPlayerElement.MediaPlayer.Pause();
                songElementItem.Content = "Play";
            }
            else
            {
                DoubleAnimation animation = new DoubleAnimation();
                animation.From = 0.0;
                animation.To = 360.0;
                animation.BeginTime = TimeSpan.FromSeconds(8);
                animation.RepeatBehavior = RepeatBehavior.Forever;
                Storyboard.SetTarget(animation, img);
                Storyboard.SetTargetProperty(animation, "(UIElement.Projection).(PlaneProjection.Rotation" + "Z" + ")");
                rotation.Children.Clear();
                rotation.Children.Add(animation);
                rotation.Begin();
                songElementItem.Content = "Pause";
                eventChoosePlaySong?.Invoke(songElementItem.Tag.ToString());
                songIsPlaying = songElementItem.Tag.ToString();
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
    }

    
}
