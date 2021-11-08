using MusicPlayerDemo.Entities;
using MusicPlayerDemo.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class Login : Page
    {
        private Storyboard rotation = new Storyboard();
        private int choosedGender;
        private AuthService accountService = new AuthService();
        private CloudinaryService cloudinaryService = new CloudinaryService();
        private StorageFile fileUpload;

        public Login()
        {
            this.InitializeComponent();
            this.Loaded += AfterLoadingData;
        }

        public void AfterLoadingData(object sender, RoutedEventArgs e)
        {
            AnimationLogo();
        }

        public void HandleChangeEmailLogin(object sender, RoutedEventArgs e)
        {
            MainPage.emailLogin = emailTxt.Text;
        }

        public void HandleChangePasswordLogin(object sender, RoutedEventArgs e)
        {
            MainPage.passwordLogin = passwordTxt.Password.ToString();
        }

        public void AnimationLogo()
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0.0;
            animation.To = 360.0;
            animation.BeginTime = TimeSpan.FromSeconds(2);
            animation.RepeatBehavior = RepeatBehavior.Forever;
            Storyboard.SetTarget(animation, musicPlayerLogo);
            Storyboard.SetTargetProperty(animation, "(UIElement.Projection).(PlaneProjection.Rotation" + "Z" + ")");
            rotation.Children.Clear();
            rotation.Children.Add(animation);
            rotation.Begin();
        }

        public void ShowLoginForm(object sender, RoutedEventArgs e)
        {
            ShowForm("login");
        }

        public void ShowRegisterForm(object sender, RoutedEventArgs e)
        {
            ShowForm("register");
        }

        public void ShowForm(string formType)
        {
            if(formType == "login")
            {
                loginForm.Visibility = Visibility.Visible;
                registerForm.Visibility = Visibility.Collapsed;
            }else if(formType == "register")
            {
                loginForm.Visibility = Visibility.Collapsed;
                registerForm.Visibility = Visibility.Visible;
            }
        }

        public void ControlLoadingEffect(string effect, string type)
        {
            if(type == "register")
            {
                switch (effect)
                {
                    case "start":
                        LoadingEffect.Visibility = Visibility.Visible;
                        progressBar.ShowError = false;
                        break;
                    case "stop":
                        LoadingEffect.Visibility = Visibility.Collapsed;
                        progressBar.ShowError = false;
                        break;
                    case "error":
                        LoadingEffect.Visibility = Visibility.Visible;
                        progressBar.ShowError = true;
                        break;
                }
            }
            else if(type == "login")
            {
                switch (effect)
                {
                    case "start":
                        LoadingEffectLogin.Visibility = Visibility.Visible;
                        progressBarLogin.ShowError = false;
                        break;
                    case "stop":
                        LoadingEffectLogin.Visibility = Visibility.Collapsed;
                        progressBarLogin.ShowError = false;
                        break;
                    case "error":
                        LoadingEffectLogin.Visibility = Visibility.Visible;
                        progressBarLogin.ShowError = true;
                        break;
                }
            }
           
        }

        public async void UploadAvatar(object sender, RoutedEventArgs e)
        {
            await SetLocalMedia();
            var result = cloudinaryService.UploadImage(@fileUpload.Path);
            avatarTxt.Text = result.SecureUrl.ToString();
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

        public async Task<bool> WriteTokenToFile()
        {
            try
            {
                Windows.Storage.StorageFolder storageFolder =
                Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile tokenFile = await storageFolder.CreateFileAsync("token.txt",
                Windows.Storage.CreationCollisionOption.ReplaceExisting);
                List<string> authDataList = new List<string>();
                authDataList.Add(AuthService.AuthData["access_token"].ToString());
                var authInfo = await accountService.GetMyInfo(AuthService.AuthData["access_token"].ToString());
                authDataList.Add(authInfo.role.ToString());
                AuthService.AuthData.Add("role", authInfo.role.ToString());
                MainPage.authRole = authInfo.role;
                Debug.WriteLine(MainPage.authRole);
                await Windows.Storage.FileIO.WriteLinesAsync(tokenFile, authDataList);
                return true;
            }catch(Exception ex)
            {
                ContentDialog dialog = new ContentDialog();
                ControlLoadingEffect("error", "login");
                dialog.Title = "Notification";
                dialog.Content = "Login failed";
                dialog.CloseButtonText = "Try again";
                await dialog.ShowAsync();
                return false;
            }
        }

        public async void OnLogin(object sender, RoutedEventArgs e)
        {
            ControlLoadingEffect("start", "login");
            var loginInfo = new
            {
                email = emailTxt.Text,
                password = passwordTxt.Password.ToString(),
            };
            ContentDialog dialog = new ContentDialog();
            try
            {
                if (await accountService.Login(loginInfo))
                {
                    ControlLoadingEffect("stop", "login");
                    await WriteTokenToFile();
                    Frame.Navigate(typeof(MainPage));
                }else
                {
                    ControlLoadingEffect("error", "login");
                    dialog.Title = "Notification";
                    dialog.Content = "Login failed";
                    dialog.CloseButtonText = "Try again";
                    await dialog.ShowAsync();
                }
            }catch(Exception ex)
            {
                ControlLoadingEffect("error", "login");
                dialog.Title = "Notification";
                dialog.Content = "Login failed";
                dialog.CloseButtonText = "Try again";
                await dialog.ShowAsync();
            }
           
        }

        public async void OnRegister(object sender, RoutedEventArgs e)
        {
             ControlLoadingEffect("start", "register");
            var newAccount = new Account()
            {
                firstName = firstNameTxt.Text,
                lastName = lastNameTxt.Text,
                birthday = birthdayPicker.SelectedDate.Value.DateTime.ToString("yyyy-MM-dd"),
                gender = choosedGender,
                email = emailRegisterTxt.Text,
                phone = phoneTxt.Text,
                password = passwordRegisterTxt.Password.ToString(),
                address = addressTxt.Text,
                avatar = avatarTxt.Text,
            };
            ContentDialog dialog = new ContentDialog();
            if(await accountService.Register(newAccount))
            {
                ControlLoadingEffect("stop", "register");
                dialog.Title = "Notification";
                dialog.Content = "Register successful !";
                dialog.PrimaryButtonText = "Login";
                var result = await dialog.ShowAsync();
                if(result == ContentDialogResult.Primary)
                {
                    ShowForm("login");
                }
            }else
            {
                ControlLoadingEffect("error", "register");
                dialog.Title = "Notification";
                dialog.Content = "Register failed !";
                dialog.PrimaryButtonText = "Try again";
                var result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    ShowForm("register");
                }
            }
            
        }

        public void OnChooseGender(object sender, RoutedEventArgs e)
        {
            var genderRadio = sender as RadioButton;
            switch(genderRadio.Tag.ToString())
            {
                case "Male":
                    choosedGender = 1;
                    break;
                case "Female":
                    choosedGender = 0;
                    break;
                default:
                    choosedGender = 2;
                    break;
            }
        }
        
        public void SkipLoginPage(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }

  
}
