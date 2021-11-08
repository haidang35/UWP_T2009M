using MusicPlayerDemo.Entities;
using MusicPlayerDemo.Pages;
using MusicPlayerDemo.Pages.Admin;
using MusicPlayerDemo.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MusicPlayerDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static List<Windows.Storage.StorageFile> listFile = new List<Windows.Storage.StorageFile>();
        public static string songChoosed;
        public static MediaPlayer mediaPlayer = new MediaPlayer();
        public PlayList playList = new PlayList();
        public static string emailLogin;
        public static string passwordLogin;
        private AuthService accountService = new AuthService();
        public static int authRole;
        public MainPage()
        {
            this.InitializeComponent();
        }

        async Task<string> CheckAuth()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.GetFileAsync("token.txt");
            var lines = await FileIO.ReadLinesAsync(file) ?? null;
            if(lines != null && lines.Count > 1)
            {
                authRole = Int32.Parse(lines.ElementAt(1));
            }
            if (authRole == 99)
            {
                return "Admin";
            }else if(authRole != 0)
            {
                return "User";
            }
            authRole = 0;
            return "Guest";
          
        }

        public async void LoadedPage(object sender, RoutedEventArgs e)
        {
            if(await CheckAuth() == "Admin")
            {
                ShowMenuWithRole("Admin");
            } else if(await CheckAuth() == "Guest")
            {
                ShowMenuWithRole("Guest");
            }
            else if(await CheckAuth() == "User")
            {
                ShowMenuWithRole("User");
            }

            contentFrame.Navigate(typeof(PlayList));
        }

        public void ShowMenuWithRole(string role)
        {
            switch(role)
            {
                case "Admin":
                    navView.MenuItems.Add(new NavigationViewItemSeparator());
                    navView.MenuItems.Add(new NavigationViewItem
                    {
                        Content = "Account List",
                        Icon = new SymbolIcon((Symbol)0xF1AD),
                        Tag = "accountList"
                    });
                    navView.MenuItems.Add(new NavigationViewItem
                    {
                        Content = "Song List",
                        Icon = new SymbolIcon((Symbol)0xF1AD),
                        Tag = "songList"
                    });
                    CreateMySong.Visibility = Visibility.Visible;
                    MyMusic.Visibility = Visibility.Visible;
                    GetMyProfile.Visibility = Visibility.Visible;
                    Logout.Visibility = Visibility.Visible;
                    Login.Visibility = Visibility.Collapsed;
                    break;
                case "Guest":
                    /*  navView.MenuItems.Remove(CreateMySong);
                       navView.MenuItems.Remove(MyMusic);
                       navView.MenuItems.Remove(GetMyProfile);*/
                    break;
                case "User":
                    Logout.Visibility = Visibility.Visible;
                    Login.Visibility = Visibility.Collapsed;
                    CreateMySong.Visibility = Visibility.Visible;
                    MyMusic.Visibility = Visibility.Visible;
                    GetMyProfile.Visibility = Visibility.Visible;
                    break;
            }
        }

        public void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if(args.IsSettingsSelected == true)
            {
                NavigateContent(sender.Tag.ToString(), args.RecommendedNavigationTransitionInfo);
            } else if(args.SelectedItemContainer != null)
            {
                var navItemTag = args.SelectedItemContainer.Tag.ToString();
                NavigateContent(navItemTag, args.RecommendedNavigationTransitionInfo);
            }
        }

        public void NavigateContent(string navItemTag, NavigationTransitionInfo transition)
        {
            if(navItemTag == "settings")
            {
                contentFrame.Navigate(typeof(Setting));
            } else
            {
                switch(navItemTag)
                {
                    case "myMusic":
                        contentFrame.Navigate(typeof(MyMusic));
                        break;
                    case "recentPlays":
                        contentFrame.Navigate(typeof(RecentPlay));
                        break;
                    case "nowPlaying":
                        contentFrame.Navigate(typeof(NowPlaying));
                        break;
                    case "playList":
                        contentFrame.Navigate(typeof(PlayList));
                        break;
                    case "createNewSong":
                        contentFrame.Navigate(typeof(CreateNewSong));
                        break;
                    case "createMySong":
                        contentFrame.Navigate(typeof(CreateMySong));
                        break;
                    case "register":
                        contentFrame.Navigate(typeof(RegisterAccount));
                        break;
                    case "getMyProfile":
                        contentFrame.Navigate(typeof(Profile));
                        break;
                    case "login":
                        Frame.Navigate(typeof(Login));
                        break;
                    case "accountList":
                        contentFrame.Navigate(typeof(AccountList));
                        break;
                    case "songList":
                        contentFrame.Navigate(typeof(SongList));
                        break;
                    case "logout":
                        LogoutAccount();
                        break;
                }
            }
        }

        public async void LogoutAccount()
        {
            Logout.Visibility = Visibility.Collapsed;
            Windows.Storage.StorageFolder storageFolder =
            Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile = await storageFolder.CreateFileAsync("token.txt",
            Windows.Storage.CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(sampleFile, "");
            Frame.Navigate(typeof(Login));
        }


        public void NavigationView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            TryGoBack();
        }

        public bool TryGoBack()
        {
            if(!contentFrame.CanGoBack)
            {
                return false;
            }
            if(navView.IsPaneOpen && navView.DisplayMode == NavigationViewDisplayMode.Compact
              || navView.DisplayMode == NavigationViewDisplayMode.Minimal  
              )
            {
                return false;
            }
            contentFrame.GoBack();
            return true;
        }

      /*  public async void AddMediaPlay()
        {
            await SetLocalMedia();
        }

        async public System.Threading.Tasks.Task SetLocalMedia()
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            openPicker.FileTypeFilter.Add(".wmv");
            openPicker.FileTypeFilter.Add(".mp4");
            openPicker.FileTypeFilter.Add(".wma");
            openPicker.FileTypeFilter.Add(".mp3");
            var file = await openPicker.PickSingleFileAsync();
            listFile.Add(file);
            if (file != null)
            {
                mediaPlayerElement.Source = MediaSource.CreateFromStorageFile(file);
                mediaPlayerElement.MediaPlayer.Play();
            }
        }*/

       
    }
}
