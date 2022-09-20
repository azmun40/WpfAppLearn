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

namespace WpfAppLearn
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private const string API_KEY = "5cf4dd5be7a6ab85b34b33fd13c305b6";
        public MainWindow()
        {
            InitializeComponent();
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
    }
}
