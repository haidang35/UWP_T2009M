using Demo023.Entity;
using MySql.Data.MySqlClient;
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
using MySQL.Data;
using Demo023.Models;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Demo023
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private int genderDefault = 1;

        public MainPage()
        {
            this.InitializeComponent();
            StudentModel stm = new StudentModel();
            Debug.WriteLine("Get all");
            stm.findAll();
            Debug.WriteLine("Find one");
            stm.findById(1);
            Debug.WriteLine("Update");
            var student = new Student
            {
                RollNumber = "B222",
                FullName = "Nguyen Van Nam",
                Dob = DateTime.Parse("1999-05-15"),
                Email = "nguyna@gmail.com",
                Phone = "035556556560",
                Address = "abcd",
                Gender = 1,
            };
            stm.Update(1, student);
            Debug.WriteLine("After updating");
            stm.findById(1);
            Debug.WriteLine("Deleting ....");
            stm.Delete(1);


        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Save(object sender, RoutedEventArgs e)
        {
            var fullName = txtFullName.Text;
            var rollNumber = txtRollNumber.Text;
            var phone = txtPhone.Text;
            var email = txtEmail.Text;
            var birthday = txtBirthday.SelectedDate;
            var address = txtAddress.Text;
            var gender = genderDefault;
            var student = new Student
            {
               RollNumber = rollNumber,
               FullName = fullName,
               Phone = phone,
               Email = email,
               Dob = birthday.Value.Date,
               Address = address,
               Gender =  gender,

            };
            var studentModel = new StudentModel();
            studentModel.Save(student);
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {

        }

        private void Check_Gender(object sender, RoutedEventArgs e)
        {
            RadioButton checkGender = sender as RadioButton;
            switch(checkGender.Tag.ToString())
            {
                case "Male":
                    genderDefault = 1;
                    break;
                case "Female":
                    genderDefault = 0;
                    break;
            }
        }
    }
}
