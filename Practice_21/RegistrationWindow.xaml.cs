using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Shapes;

namespace Practice_21
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        PP21Entitie db = new PP21Entitie();
        public RegistrationWindow()
        {
            InitializeComponent();
            db.Auth.Load();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddUser(object sender, RoutedEventArgs e)
        {
            try
            {
                Auth authTemp = new Auth();
                authTemp.Login = loginBox.Text;
                authTemp.Password = passBox.Password;
                if (cb.SelectedIndex == 0)
                    authTemp.AccessLevel = true;
                else
                    authTemp.AccessLevel = false;
                db.Auth.Add(authTemp);
                db.SaveChanges();
                Close();
            }
            catch
            {
                MessageBox.Show("Введены некорректные данные");
            }
        }

        private void PassChanged(object sender, RoutedEventArgs e)
        {
            if (passBox.Password.Length < 4) regButton.IsEnabled = false;
            else regButton.IsEnabled = true;
        }
    }
}
