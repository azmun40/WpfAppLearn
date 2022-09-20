using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfAppLearn.Models;

namespace WpfAppLearn
{
    /// <summary>
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private AppDbContext _db;
        public RegisterWindow()
        {
            InitializeComponent();
             _db = new AppDbContext();
        }

        private void RegBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = UserLoginField.Text.Trim();
            string email = UserEmailField.Text.Trim();
            string password = UserPasswordField.Password.Trim();
            if(login.Equals("") || !email.Contains("@") || password.Length < 3)
            {
                MessageBox.Show("Вы ввели что-то неверно");
                return;
            }

            User user = new User(login, email, Hash(password));
            _db.users.Add(user);
            _db.SaveChanges();
            UserLoginField.Text = "";
            UserEmailField.Text = "";
            UserPasswordField.Password = "";
            RegBtn.Content = "Готово";
 
        }

        private string Hash(string password)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            using(SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }


}
