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
    /// Логика взаимодействия для SearchWin.xaml
    /// </summary>
    public partial class SearchWin : Window
    {
        PP21Entitie db = new PP21Entitie();
        public SearchWin()
        {
            InitializeComponent();
            db.Building.Load();
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            var res = db.Building.SqlQuery($"SELECT * FROM Building WHERE BuildingName = N'{tbSField.Text}'");
                //SqlQuery($"SELECT * FROM Building WHERE BuildingName = '{tbSField.Text}'");
            searchGrid.ItemsSource = res.ToList();
        }
    }
}
