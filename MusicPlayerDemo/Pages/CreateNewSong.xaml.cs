using CloudinaryDotNet.Actions;
using MusicPlayerDemo.Entities;
using Newtonsoft.Json.Linq;
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
using MusicPlayerDemo.Services;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicPlayerDemo.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateNewSong : Windows.UI.Xaml.Controls.Page
    {
        public int songStatus;
        private CloudinaryService cloudinaryService = new CloudinaryService();
        private StorageFile fileUpload;
        public CreateNewSong()
        {
            this.InitializeComponent();
        }

        public async void UploadImage(object sender, RoutedEventArgs e)
        {
            await SetLocalMedia();
            if(fileUpload != null)
            {
                var result = cloudinaryService.UploadImage(@fileUpload.Path);
                thumbnailTxt.Text = result.Uri.ToString();
            }
        }

        public void ControlLoadingEffect(string action)
        {
           if(action == "start")
            {
                LoadingEffect.Visibility = Visibility.Visible;
            }else if(action == "stop")
            {
                LoadingEffect.Visibility = Visibility.Collapsed;
            }
        }

        public async void UploadFile(object sender, RoutedEventArgs e)
        {
            await SetLocalMedia();
            if(fileUpload != null)
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

        public async void SaveNewSong(object sender, RoutedEventArgs e)
        {
            var musicService = new MusicService();
            var songName = songNameTxt.Text;
            var singer = singerTxt.Text;
            var author = authorTxt.Text;
            var thumbnail = thumbnailTxt.Text;
            var link = linkTxt.Text;
            var message = messageTxt.Text;
            var description = descriptionTxt.Text;
            var newSong = new Song
            {
                name = songName,
                singer = singer,
                author = author,
                thumbnail = thumbnail,
                link = link,
                message = message,
                description = description,
                status = songStatus,
            };
            ContentDialog dialog = new ContentDialog();
            if(await musicService.Save(newSong))
            {
                dialog.Title = "Notification";
                dialog.Content = "Save new song successful !";
                dialog.CloseButtonText = "Cancel";
                await dialog.ShowAsync();
                ClearTextValue();
            } else
            {
                dialog.Title = "Notification";
                dialog.Content = "Save new song failed !";
                dialog.CloseButtonText = "Cancel";
                await dialog.ShowAsync();
            }
        }

        public void ClearTextValue()
        {
            songNameTxt.Text = "";
            singerTxt.Text = "";
            authorTxt.Text = "";
            thumbnailTxt.Text = "";
            linkTxt.Text = "";
            messageTxt.Text = "";
            descriptionTxt.Text = "";
        }

        public void SongStatusChecked(object sender, RoutedEventArgs e)
        {
            var statusRadio = sender as RadioButton;
            switch(statusRadio.Tag.ToString())
            {
                case "Active":
                    songStatus = (int) SongStatus.Active;
                    break;
                case "Deactive":
                    songStatus = (int)SongStatus.Deactive;
                    break;
            }
        }
    }
}
