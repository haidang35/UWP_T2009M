using Microsoft.Toolkit.Uwp.UI.Controls;
using MusicPlayerDemo.Entities;
using MusicPlayerDemo.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class SongList : Page
    {
        private AuthService accountService = new AuthService();
        private List<Song> songList;
        public SongList()
        {
            this.InitializeComponent();
            Loaded += GetSongListData;
        }

        public void GetSongListData(object sender, RoutedEventArgs e)
        {
            ControlLoadingEffect("start");
            LoadSongListData();
        }

        public async void LoadSongListData()
        {
            songList = await accountService.GetSongList();
            SongListDataGrid.ItemsSource = songList;
            ControlLoadingEffect("stop");
        }

        public void ControlLoadingEffect(string action)
        {
            switch(action)
            {
                case "start":
                    controlBar.Visibility = Visibility.Collapsed;
                    SongListDataGrid.Visibility = Visibility.Collapsed;
                    LoadingEffect.Visibility = Visibility.Visible;
                    break;
                case "stop":
                    LoadingEffect.Visibility = Visibility.Collapsed;
                    controlBar.Visibility = Visibility.Visible;
                    SongListDataGrid.Visibility = Visibility.Visible;
                    break;
            }
        }

        public async void DataGrid_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            var songRow = e.Row.DataContext as Song;
            ContentDialog dialog = new ContentDialog();
            if (await accountService.UpdateSongInfo(songRow.id, songRow))
            {
                dialog.Title = "Notification";
                dialog.Content = "You have just updated the song successfull";
                dialog.CloseButtonText = "Continue";
                await dialog.ShowAsync();
            }
            else
            {
                dialog.Title = "Notification";
                dialog.Content = "You have just updated the song failed";
                dialog.CloseButtonText = "Try again";
                await dialog.ShowAsync();
            }
            LoadSongListData();
        }

        public async void DeleteSong(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            ContentDialog dialog = new ContentDialog();
            dialog.Title = "Notification";
            dialog.Content = "Are you sure delete the song ?";
            dialog.CloseButtonText = "Close";
            dialog.PrimaryButtonText = "Confirm";
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                if (await accountService.DeleteSong(Int32.Parse(button.Tag.ToString())))
                {
                    dialog.Title = "Notification";
                    dialog.Content = "You have just deleted the song successfull";
                    dialog.CloseButtonText = "Continue";
                    await dialog.ShowAsync();
                }
                else
                {
                    dialog.Title = "Notification";
                    dialog.Content = "You have just deleted the song failed";
                    dialog.CloseButtonText = "Try again";
                    await dialog.ShowAsync();
                }
                LoadSongListData();
            }
        }

        public void mySearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            SongListDataGrid.ItemsSource = new ObservableCollection<Song>(from song in songList
                                                                                where IsIndexOfProperties(song, args.QueryText)
                                                                                select song);
        }

        public bool IsIndexOfProperties(Song song, string queryText)
        {
            if (song.name.IndexOf(queryText) != -1 || song.author.IndexOf(queryText) != -1
                || song.singer.IndexOf(queryText) != -1
                || song.account_id.ToString().IndexOf(queryText) != -1)
            {
                return true;
            }
            return false;
        }

        public void SongListSorting(object sender, DataGridColumnEventArgs e)
        {
            switch(e.Column.Tag.ToString())
            {
                case "name":
                    if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                    {
                        SongListDataGrid.ItemsSource = new ObservableCollection<Song>(from item in songList
                                                                                      orderby item.name ascending
                                                                                      select item);
                        e.Column.SortDirection = DataGridSortDirection.Ascending;
                    }
                    else
                    {
                        SongListDataGrid.ItemsSource = new ObservableCollection<Song>(from item in songList
                                                                                      orderby item.name descending
                                                                                      select item);
                        e.Column.SortDirection = DataGridSortDirection.Descending;
                    }
                    break;
                case "author":
                    if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                    {
                        SongListDataGrid.ItemsSource = new ObservableCollection<Song>(from item in songList
                                                                                      orderby item.author ascending
                                                                                      select item);
                        e.Column.SortDirection = DataGridSortDirection.Ascending;
                    }
                    else
                    {
                        SongListDataGrid.ItemsSource = new ObservableCollection<Song>(from item in songList
                                                                                      orderby item.author descending
                                                                                      select item);
                        e.Column.SortDirection = DataGridSortDirection.Descending;
                    }
                    break;
                case "singer":
                    if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                    {
                        SongListDataGrid.ItemsSource = new ObservableCollection<Song>(from item in songList
                                                                                      orderby item.singer ascending
                                                                                      select item);
                        e.Column.SortDirection = DataGridSortDirection.Ascending;
                    }
                    else
                    {
                        SongListDataGrid.ItemsSource = new ObservableCollection<Song>(from item in songList
                                                                                      orderby item.singer descending
                                                                                      select item);
                        e.Column.SortDirection = DataGridSortDirection.Descending;
                    }
                    break;
            }
           
        }

        
    }
}
