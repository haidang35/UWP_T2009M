using MusicPlayerDemo.Entities;
using MusicPlayerDemo.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class CreateMySong : Page
    {
        private AuthService accountService = new AuthService();
        private StorageFile fileUpload;
        private CloudinaryService cloudinaryService = new CloudinaryService();

        public CreateMySong()
        {
            this.InitializeComponent();
        }

        public async void SaveNewSong(object sender, RoutedEventArgs e)
        {
            var songName = songNameTxt.Text;
            var author = authorTxt.Text;
            var thumbnail = thumbnailTxt.Text;
            var link = linkTxt.Text;
            var singer = singerTxt.Text;
            var newSong = new Song
            {
                name = songName,
                author = author,
                thumbnail = thumbnail,
                link = link,
                singer = singer,
                description = descTxt.Text,
            };
            ContentDialog dialog = new ContentDialog();
            if(await accountService.CreateMySong(newSong))
            {
                dialog.Title = "Notification";
                dialog.Content = "You have just created new your song successful";
                dialog.CloseButtonText = "Close";
                var result = await dialog.ShowAsync();
                if (result == ContentDialogResult.None)
                {
                    Frame.Navigate(typeof(MyMusic));
                }
            }
            else
            {
                dialog.Title = "Notification";
                dialog.Content = "You have just created new your song failed";
                dialog.CloseButtonText = "Close";
            }
           
        }

        public async void UploadImage(object sender, RoutedEventArgs e)
        {
            await SetLocalMedia();
            if (fileUpload != null)
            {
                var result = cloudinaryService.UploadImage(@fileUpload.Path);
                thumbnailTxt.Text = result.Uri.ToString();
            }
        }

        public async void UploadFile(object sender, RoutedEventArgs e)
        {
            await SetLocalMedia();
            if (fileUpload != null)
            {
                var result = cloudinaryService.UploadFile(@fileUpload.Path);
                linkTxt.Text = result;
            }
        }

        async public System.Threading.Tasks.Task SetLocalMedia()
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".wmv");
            openPicker.FileTypeFilter.Add(".mp4");
            openPicker.FileTypeFilter.Add(".wma");
            openPicker.FileTypeFilter.Add(".mp3");
            fileUpload = await openPicker.PickSingleFileAsync();
        }
    }
}
