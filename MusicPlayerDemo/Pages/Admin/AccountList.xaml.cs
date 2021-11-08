using Microsoft.Toolkit.Uwp.UI.Controls;
using MusicPlayerDemo.Entities;
using MusicPlayerDemo.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace MusicPlayerDemo.Pages.Admin
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AccountList : Page
    {
        private AuthService accountService = new AuthService();
        private List<Account> accountList;
        public static Account accountDetails;
        public AccountList()
        {
            this.InitializeComponent();
            this.Loaded += AfterLoadingComponent;
        }

        public void AfterLoadingComponent(object sender, RoutedEventArgs e)
        {
            ControlLoadingEffect("start");
            LoadingAccountList();
        }

        public async void LoadingAccountList()
        {
            accountList = await accountService.GetAccountList();
            AccountListDataGrid.ItemsSource = accountList;
            ControlLoadingEffect("stop");
        }

        public void ControlLoadingEffect(string action)
        {
            switch (action)
            {
                case "start":
                    controlBar.Visibility = Visibility.Collapsed;
                    AccountListDataGrid.Visibility = Visibility.Collapsed;
                    LoadingEffect.Visibility = Visibility.Visible;
                    break;
                case "stop":
                    LoadingEffect.Visibility = Visibility.Collapsed;
                    controlBar.Visibility = Visibility.Visible;
                    AccountListDataGrid.Visibility = Visibility.Visible;
                    break;
            }
        }

        public string ShowAccountRole(int role)
        {
            return role == 99 ? "Admin" : "User";
        }

        public async void DataGrid_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            var accountRow = e.Row.DataContext as Account;
            Debug.WriteLine($"{accountRow.avatar} {accountRow.id}");
            ContentDialog dialog = new ContentDialog();
            if (await accountService.UpdateAccountInfo(accountRow.id, accountRow))
            {
                dialog.Title = "Notification";
                dialog.Content = "You have just updated the account successfull";
                dialog.CloseButtonText = "Continue";
                await dialog.ShowAsync();
            }
            else
            {
                dialog.Title = "Notification";
                dialog.Content = "You have just updated the account failed";
                dialog.CloseButtonText = "Try again";
                await dialog.ShowAsync();
            }
            LoadingAccountList();
        }

        public async void  DeleteAccount(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            ContentDialog dialog = new ContentDialog();
            dialog.Title = "Notification";
            dialog.Content = "Are you sure delete the account ?";
            dialog.CloseButtonText = "Close";
            dialog.PrimaryButtonText = "Confirm";
            var result = await dialog.ShowAsync();
            if(result == ContentDialogResult.Primary)
            {
                if (await accountService.DeleteAccount(Int32.Parse(button.Tag.ToString())))
                {
                    dialog.Title = "Notification";
                    dialog.Content = "You have just deleted the account successfull";
                    dialog.CloseButtonText = "Continue";
                    await dialog.ShowAsync();
                }else
                {
                    dialog.Title = "Notification";
                    dialog.Content = "You have just deleted the account failed";
                    dialog.CloseButtonText = "Try again";
                    await dialog.ShowAsync();
                }
                LoadingAccountList();
            }
        }

        public void mySearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            AccountListDataGrid.ItemsSource = new ObservableCollection<Account>(from account in accountList
                                                                                where IsIndexOfProperties(account, args.QueryText)
                                                                                select account);
        }

        public bool IsIndexOfProperties(Account account, string queryText)
        {
            if(account.firstName.IndexOf(queryText) != -1 || account.lastName.IndexOf(queryText) != -1
                || $"{account.firstName} {account.lastName}".IndexOf(queryText) != -1
                || account.email.IndexOf(queryText) != -1 
                || account.phone.IndexOf(queryText) != -1)
            {
                return true;
            }
            return false;
        }

        public void SelectFilterGender(object sender, SelectionChangedEventArgs e)
        {
            int genderSelected = 0;
            switch(FilterGenderBox.SelectedValue.ToString())
            {
                case "Male":
                    genderSelected = 1;
                    break;
                case "Female":
                    genderSelected = 0;
                    break;
            }
            if (FilterGenderBox.SelectedValue.ToString() == "All")
            {
                AccountListDataGrid.ItemsSource = accountList;
            }else
            {
                AccountListDataGrid.ItemsSource = new ObservableCollection<Account>(from item in accountList
                                                                                    where item.gender == genderSelected
                                                                                    select item);
            }
        }

        public void AccountListSorting(object sender, DataGridColumnEventArgs e)
        {
            switch (e.Column.Tag.ToString())
            {
                case "firstName":
                    if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                    {
                        AccountListDataGrid.ItemsSource = new ObservableCollection<Account>(from item in accountList
                                                                                      orderby item.firstName ascending
                                                                                      select item);
                        e.Column.SortDirection = DataGridSortDirection.Ascending;
                    }
                    else
                    {
                        AccountListDataGrid.ItemsSource = new ObservableCollection<Account>(from item in accountList
                                                                                         orderby item.firstName descending
                                                                                      select item);
                        e.Column.SortDirection = DataGridSortDirection.Descending;
                    }
                    break;
                case "lastName":
                    if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                    {
                        AccountListDataGrid.ItemsSource = new ObservableCollection<Account>(from item in accountList
                                                                                         orderby item.lastName ascending
                                                                                      select item);
                        e.Column.SortDirection = DataGridSortDirection.Ascending;
                    }
                    else
                    {
                        AccountListDataGrid.ItemsSource = new ObservableCollection<Account>(from item in accountList
                                                                                         orderby item.lastName descending
                                                                                      select item);
                        e.Column.SortDirection = DataGridSortDirection.Descending;
                    }
                    break;
            }

        }

        public async void ViewAccountDetails(object sender, RoutedEventArgs e)
        {
            var buttonData = sender as Button;
            var accountId = Int32.Parse(buttonData.Tag.ToString());
            accountDetails = await accountService.GetAccountInfo(accountId);
            Frame.Navigate(typeof(AccountDetails));
        }
    }
}
