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
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        
        public AuthWindow()
        {
            InitializeComponent();
            
        }

        private void AuthBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = UserLoginField.Text.Trim();
            string password = UserPasswordField.Password.Trim();
            if (login.Equals("") || password.Length < 3)
            {
                MessageBox.Show("Вы ввели что-то неверно");
                return;
            }

            User authUser = null;
            using(AppDbContext db = new AppDbContext())
            {
                authUser = db.users.Where(user => user.Login == login && user.Password == Hash(password)).FirstOrDefault();
            }
            if (authUser == null)
                MessageBox.Show("Такой пользователь не найден");
            else
            {
                UserLoginField.Text = "";
                UserPasswordField.Password = "";
                AuthBtn.Content = "Готово";
            }

        }


        private string Hash(string password)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
