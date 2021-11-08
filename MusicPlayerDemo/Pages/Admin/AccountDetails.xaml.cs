using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicPlayerDemo.Pages.Admin
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AccountDetails : Page
    {
        public AccountDetails()
        {
            this.InitializeComponent();
            this.Loaded += AfterLoadingComponent;
        }

        public void AfterLoadingComponent(object sender, RoutedEventArgs e)
        {
            GetAccountInfo();
        }

        public void GetAccountInfo()
        {
            var account = AccountList.accountDetails;
            if (account != null)
            {
                idTxt.Text = account.id.ToString();
                roleTxt.Text = account.GetUserRole();
                firstNameTxt.Text = account.firstName;
                lastNameTxt.Text = account.lastName;
                birthdayTxt.Text = account.birthday;
                genderTxt.Text = account.GetUserGender();
                emailTxt.Text = account.email;
                phoneTxt.Text = account.phone;
                addressTxt.Text = account.address;
                introduceTxt.Text = account.introduction ?? "No information yet";
                statusTxt.Text = account.GetStatus();
                avatarImage.Source = new BitmapImage(new Uri(account.avatar));
            }
            LoadingEffect.Visibility = Visibility.Collapsed;
            ProfileContent.Visibility = Visibility.Visible;
        }
    }
}
