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
    public sealed partial class RegisterAccount : Page
    {
        public int gender;
        public RegisterAccount()
        {
            this.InitializeComponent();
        }

        public async void AuthRegister(object sender, RoutedEventArgs e)
        {
            var firstName = firstNameTxt.Text;
            var lastName = lastNameTxt.Text;
            var email = emailTxt.Text;
            var phone = phoneTxt.Text;
            var birthday = birthdayTxt.SelectedDate.Value.DateTime.ToString("yyyy-MM-dd");
            var password = passwordTxt.Text;
            var avatar = avatarTxt.Text;
            var address = addressTxt.Text;
            var newAccount = new Account
            {
                firstName = firstName,
                lastName = lastName,
                email = email,
                phone = phone,
                birthday = birthday,
                password = password,
                avatar = avatar,
                address = address,
                gender = gender,
            };
            Debug.WriteLine($"{gender} {birthday}");
            var authService = new AuthService();
            ContentDialog dialog = new ContentDialog();
            if(await authService.Register(newAccount))
            {
                dialog.Title = "Notification";
                dialog.Content = "Register Successful !";
                dialog.CloseButtonText = "Continue";
                await dialog.ShowAsync();
            } else
            {
                dialog.Title = "Notification";
                dialog.Content = "Register Failed !";
                dialog.CloseButtonText = "Try again";
                await dialog.ShowAsync();
            }
        }

        public void ChooseGenderChecked(object sender, RoutedEventArgs e)
        {
            var genderCheckedRadio = sender as RadioButton;
            switch(genderCheckedRadio.Tag.ToString())
            {
                case "Male":
                    gender = (int) AccountGender.Male;
                    break;
                case "Female":
                    gender = (int) AccountGender.Female;
                    break;
                case "Other":
                    gender = (int) AccountGender.Other;
                    break;
            }
        }
    }
}
