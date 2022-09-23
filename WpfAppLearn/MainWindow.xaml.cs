using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using WpfAppLearn.Models;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Win32;

namespace WpfAppLearn
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private const string API_KEY = "5cf4dd5be7a6ab85b34b33fd13c305b6";
        private AppDbContext _db = new AppDbContext();
        public MainWindow()
        {
            InitializeComponent();

            MainScreen.IsChecked = true;
            
            ObservableCollection<User> items = new ObservableCollection<User>();
            List<User> Users = _db.users.ToList();
            foreach (User el in Users)
                items.Add(el);
            
            ListLogin.ItemsSource = items;

            if (!File.Exists("user.xml"))
            {
                Hide();
                AuthWindow authWindow = new AuthWindow();
                authWindow.Show();
                Close();
            }

            XmlSerializer xml = new XmlSerializer(typeof(AuthUsers));
            using (FileStream file = new FileStream("user.xml", FileMode.Open))
            {
                AuthUsers auth = (AuthUsers)xml.Deserialize(file);
                UserLoginLabel.Content = auth.Login;
            }
        }

        private async void GetWeatherBtn_Click(object sender, RoutedEventArgs e)
        {
            string city = UserCityField.Text.Trim();
            if(city.Length < 2)
            {
                MessageBox.Show("Укажите верные данные");
                return;
            }

            try
            {
                string data = await GetWeather(city);
                var json = JObject.Parse(data);
                string temp = json["main"]["temp"].ToString();
                WeatherResult.Content = $"В городе {city} температура воздуха {temp} по Цельсию";
            }catch(HttpRequestException ex)
            {
                MessageBox.Show("Укажите верный город");
                WeatherResult.Content = "";
            }

        }

        private async Task<string> GetWeather(string city)
        {

            HttpClient client = new HttpClient();
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={API_KEY}&units=metric";
            return await client.GetStringAsync(url);

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            string objName = ((RadioButton)sender).Name;
            StackPanel[] panels = {MainScreenPanel, UsersPanel, ChangeScreenPanel, NotesScreenPanel };
            foreach (var el in panels)
                el.Visibility = Visibility.Hidden;
            switch (objName)
            {
                case ("MainScreen"): MainScreenPanel.Visibility = Visibility.Visible; break;
                case ("ListUsers"): UsersPanel.Visibility = Visibility.Visible; break;
                case ("Cabinet"): ChangeScreenPanel.Visibility = Visibility.Visible; break;
                case ("NotesScreen"): NotesScreenPanel.Visibility = Visibility.Visible; break;
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            string userLogin = UserLoginEnter.Text.Trim();
            if (string.IsNullOrEmpty(userLogin))
            {
                MessageBox.Show("Введите логин!!");
                return;
            }
            User user = (User)_db.users.Where(el => el.Login == userLogin).First() ?? new User();
            _db.users.Remove(user);
            _db.SaveChanges();

            MessageBox.Show("Вы удалили пользователя");
            UserLoginEnter.Text = "";
            DeleteUser.Content = "Готово";
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            File.Delete("user.xml");
            ShowAuthWindow();
        }
        private void ShowAuthWindow()
        {
            Hide();
            AuthWindow window = new AuthWindow();
            window.Show();
            Close();
        }

        private void ChangeUser_Click(object sender, RoutedEventArgs e)
        {
            string login = UserChangeLogin.Text.Trim();
            string email = UserChangeEmail.Text.Trim();
            if (login.Equals("") || !email.Contains("@"))
            {
                MessageBox.Show("Вы что-то ввели неверно");
                return;
            }
            int countUsers = _db.users.Count(el => el.Login == login);
            if(countUsers != 0 && !login.Equals(UserLoginLabel.Content))
            {
                MessageBox.Show("Данный логин уже занят");
                return ;
            }

            User user = _db.users.FirstOrDefault(el => el.Login == UserLoginLabel.Content.ToString());
            user.Email = email;
            user.Login = login;
            _db.SaveChanges();
            UserLoginLabel.Content = login;
            ChangeUser.Content = "Готово";

            AuthUsers auth = new AuthUsers(login, email);
            XmlSerializer xml = new XmlSerializer(typeof(AuthUsers));
            using (FileStream file = new FileStream("user.xml", FileMode.Create))
            {
                xml.Serialize(file, auth);

            }
        }

        private void MenuOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();

        }

        private void MenuNewFile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuSaveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.ShowDialog();

        }

        private void TimesNewRomanSetText_Click(object sender, RoutedEventArgs e)
        {
            UserNotes.FontFamily = new FontFamily("Times New Roman");
            VerdanaSetText.IsChecked = false;
            CalibriSetText.IsChecked = false;
        }

        private void VerdanaSetText_Click(object sender, RoutedEventArgs e)
        {
            UserNotes.FontFamily = new FontFamily("Verdana");
            TimesNewRomanSetText.IsChecked = false;
            CalibriSetText.IsChecked = false;

        }

        private void CalibriSetText_Click(object sender, RoutedEventArgs e)
        {
            UserNotes.FontFamily = new FontFamily("Calibri");
            TimesNewRomanSetText.IsChecked = false;
            VerdanaSetText.IsChecked = false;
        }
    }
}
