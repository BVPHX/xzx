using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
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

namespace Practice_21
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public PP21Entitie db = new PP21Entitie();

        public MainWindow()
        {
            AuthWindow auth = new AuthWindow();
            if (auth.ShowDialog() == false) Close();
            InitializeComponent();
            db.Regions.Load();
            db.Branches.Load();
            db.Building.Load();
            db.Organizations.Load();
            regionsGrid.ItemsSource = db.Regions.Local.ToBindingList();
            branchesGrid.ItemsSource = db.Branches.Local.ToBindingList();
            buildingsGrid.ItemsSource = db.Building.Local.ToBindingList();
            OrganizationsGrid.ItemsSource = db.Organizations.Local.ToBindingList();
            var temp = db.Building
                .Join(db.Organizations,
                b => b.OrganizationCode,
                o => o.OrganizationCode,
                (b, o) => new
                {
                    BuildingName = b.BuildingName,
                    OrganizationName = o.OrganizationName,
                    ComissingYear = b.ComissioningYear,
                    OrganizationAddress = o.OrganizationAddress
                }
                );
            string[] letters = { "Л", "М", "Т" };


            int x = DateTime.Now.AddYears(1).Year;

            task2Grid.ItemsSource = temp.Where(p => p.ComissingYear.Year == x).ToList();
            task3Grid.ItemsSource = temp.Where(o => letters.Contains(o.OrganizationAddress.Substring(o.OrganizationAddress.IndexOf(".") + 2, 1))).ToList();
            PR22Task();
        }

        private void SaveButton(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();
        }

        //private void TabSelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (regionsGrid.SelectedItem == null) ti1Delete.IsEnabled = false;
        //    else ti1Delete.IsEnabled = true;

        //    var temp = from product in db.Products
        //               join composite in db.ProductComposition on product.ProductCode equals composite.ProductCode
        //               join detail in db.Details on composite.DetailCode equals detail.DetailCode into g
        //               select new { Name = product.ProductName, Price = g.Sum(d => d.DetailPrice * composite.DetailAmount), ProductPrice = g.Sum(d => d.DetailPrice * composite.DetailAmount) + product.AssemblePrice };


        //    var test = db.Products
        //        .Join(db.ProductComposition,
        //        p => p.ProductCode,
        //        c => c.ProductCode,
        //        (p, c) => new
        //        {
        //            Name = p.ProductName,
        //            DetailCode = c.DetailCode,
        //            Amount = c.DetailAmount,
        //            Assemble = p.AssemblePrice
        //        }
        //        )
        //        .GroupJoin(db.Details,
        //        p => p.DetailCode,
        //        d => d.DetailCode,
        //        (p, details) => new
        //        {
        //            Name = p.Name,
        //            Price = details.Sum(d => d.DetailPrice * p.Amount),
        //            ProductPrice = details.Sum(d => d.DetailPrice * p.Amount) + p.Assemble
        //        });

        //    priceGrid.ItemsSource = temp.ToList();
        //}

        private void BranchAdd(object sender, RoutedEventArgs e)
        {
            Branches entry = new Branches();
            entry.BranchName = tbBName.Text;
            db.Branches.Add(entry);
            db.SaveChanges();
            branchesGrid.Items.Refresh();
        }

        private void DeleteBranch(object sender, RoutedEventArgs e)
        {
            if (branchesGrid.SelectedIndex != -1)
            {
                db.Database.ExecuteSqlCommand($"DELETE FROM Branches WHERE BranchCode = {branchesGrid.SelectedIndex + 1}");
            }
            else
            {
                MessageBox.Show("Не выбрана запись");
            }
            db.SaveChanges();
            branchesGrid.Items.Refresh();
        }

        private void BranchesControl(object sender, TextChangedEventArgs e)
        {
            if (tbBName.Text == null && tbBName.Text == String.Empty) brAdd.IsEnabled = false;
        }

        private void BranchChange(object sender, RoutedEventArgs e)
        {
            db.Database.ExecuteSqlCommand($"UPDATE Branches SET BranchName = N'{tbBName.Text}' WHERE BranchCode = {branchesGrid.SelectedIndex + 1}");
            db.SaveChanges();
            branchesGrid.Items.Refresh();
        }

        private void RegionsControl(object sender, TextChangedEventArgs e)
        {
            if (tRName.Text == null && tRName.Text == String.Empty) rAdd.IsEnabled = false;
        }

        private void AddRegion(object sender, RoutedEventArgs e)
        {
            Regions entry = new Regions();
            entry.RegionName = tRName.Text;
            db.Regions.Add(entry);
            db.SaveChanges();
            regionsGrid.Items.Refresh();
        }
        private void DeleteRegion(object sender, RoutedEventArgs e)
        {
            if (regionsGrid.SelectedIndex != -1)
            {
                db.Database.ExecuteSqlCommand($"DELETE FROM Regions WHERE RegionCode = {regionsGrid.SelectedIndex + 1}");
            }
            else
            {
                MessageBox.Show("Не выбрана запись");
            }
            db.SaveChanges();
            regionsGrid.Items.Refresh();
        }

        private void RegionChange(object sender, RoutedEventArgs e)
        {
            db.Database.ExecuteSqlCommand($"UPDATE Regions SET RegionName = N'{tRName.Text}' WHERE BranchCode = {regionsGrid.SelectedIndex + 1}");
            db.SaveChanges();
            regionsGrid.Items.Refresh();
        }

        private void AddOrg(object sender, RoutedEventArgs e)
        {
            Organizations entry = new Organizations();
            entry.OrganizationName = tOName.Text;
            entry.OrganizationAddress = tOAddress.Text;
            entry.OrganizationPhone = tOPhone.Text;
            db.Organizations.Add(entry);
            db.SaveChanges();
            OrganizationsGrid.Items.Refresh();
        }

        private void DeleteOrg(object sender, RoutedEventArgs e)
        {
            if (OrganizationsGrid.SelectedIndex != -1)
            {
                db.Database.ExecuteSqlCommand($"DELETE FROM Organizations WHERE OrganizationCode = {OrganizationsGrid.SelectedIndex + 1}");
            }
            else
            {
                MessageBox.Show("Не выбрана запись");
            }
            db.SaveChanges();
            OrganizationsGrid.Items.Refresh();
        }

        private void OrgChange(object sender, RoutedEventArgs e)
        {
            db.Database.ExecuteSqlCommand($"UPDATE Organizations SET OrganizationName = N'{tOName.Text}', OrganizationAddress = {tOAddress.Text}, OrganizationPhone = N'{tOPhone.Text}' WHERE OrganizationCode = {OrganizationsGrid.SelectedIndex + 1}");
            db.SaveChanges();
            OrganizationsGrid.Items.Refresh();
        }
        private void OrgControl(object sender, TextChangedEventArgs e)
        {
            if (tOName.Text == null && tOName.Text == String.Empty && tOPhone.Text == null && tOPhone.Text == String.Empty && tOAddress.Text == null && tOAddress.Text == String.Empty) oAdd.IsEnabled = false;
        }


        private void AddBuilding(object sender, RoutedEventArgs e)
        {
            try
            {
                db.Database.ExecuteSqlCommand($"INSERT INTO Buildings VALUES (N'{tBuName.Text}', {Convert.ToInt32(tBuRegCode.Text)}, {Convert.ToInt32(tBuBrCode.Text)}, {Convert.ToInt32(tBuOrgCode.Text)}, {Convert.ToInt32(tBuF1.Text)}, {Convert.ToInt32(tBuF2.Text)}, { Convert.ToInt32(tBuF3.Text)}, { Convert.ToInt32(tBuF4.Text)}, {(DateTime)dpBuAccept.SelectedDate})");
                db.SaveChanges();
                buildingsGrid.Items.Refresh();
            }
            catch
            {
                MessageBox.Show("Данные не введены либо введены некорректно ");
            }
        }

        private void DeleteBuilding(object sender, RoutedEventArgs e)
        {
            Building row = (Building)buildingsGrid.Items[buildingsGrid.SelectedIndex];
            if (buildingsGrid.SelectedIndex != -1)
            {
                db.Database.ExecuteSqlCommand($"DELETE FROM Building WHERE BuildingName = {row.BuildingName + 1}");
            }
            else
            {
                MessageBox.Show("Не выбрана запись");
            }
            db.SaveChanges();
            buildingsGrid.Items.Refresh();
        }

        private void ChangeBuilding(object sender, RoutedEventArgs e)
        {
            Building row = (Building)buildingsGrid.Items[buildingsGrid.SelectedIndex];
            if (buildingsGrid.SelectedIndex != -1)
            {
                db.Database.ExecuteSqlCommand($"UPDATE Building SET BuildingName = N'{tBuName.Text}', RegionCode = N'{tBuRegCode.Text}', BranchCode = N'{tBuBrCode.Text}', OrganizationCode = N'{tBuOrgCode}', Funding1Quarter = N'{tBuF1.Text}', Funding2Quarter = N'{tBuF2.Text}', Funding3Quarter = N'{tBuF3.Text}', Funding4Quarter = N'{tBuF4.Text}', ComissioningYear = N'{(DateTime)dpBuAccept.SelectedDate}' WHERE BuildingName = N'{row.BuildingName}'");
                db.SaveChanges();
                buildingsGrid.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Не выбрана запись");
            }
        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            SearchWin search = new SearchWin();
            search.ShowDialog();
        }
        private void PR22Task()
        {
            var task22 = db.Building
               .Join(db.Branches,
               bu => bu.BranchCode,
               br => br.BranchCode,
               (bu, br) => new
               {
                   BranchName = br.BranchName,
                   FundingValue = bu.Funding1Quarter + bu.Funding2Quarter + bu.Funding3Quarter + bu.Funding4Quarter,
               }
               );
            task22Grid.ItemsSource = task22.ToList();
            var task23 = db.Building
                .Join(db.Regions,
                b => b.RegionCode,
                r => r.RegionCode,
                (b, r) => new
                {
                    RegionName = r.RegionName,
                    FundingValue = b.Funding1Quarter + b.Funding2Quarter + b.Funding3Quarter + b.Funding4Quarter
                }
                );
            task23Grid.ItemsSource = task23.ToList();
            var task24 = db.Building
                .Join(db.Branches,
                b => b.BranchCode,
                br => br.BranchCode,
                (b, br) => new
                {
                    BranchName = br.BranchName,
                    FundingValue = (b.Funding1Quarter + b.Funding2Quarter + b.Funding3Quarter + b.Funding4Quarter)/ 4
                }
                );
            task24Grid.ItemsSource = task24.OrderBy(b => b.BranchName).ToList();
            var task25 = db.Building
                .Join(db.Branches,
                b => b.BranchCode,
                br => br.BranchCode,
                (b, br) => new
                {
                    BranchName = br.BranchName,
                    FundingValue = b.Funding1Quarter + b.Funding2Quarter + b.Funding3Quarter + b.Funding4Quarter
                }
                );
            task25Grid.ItemsSource = task25.Where(a => a.FundingValue == task25.Max(b => b.FundingValue)).ToList();

                



        }
    }
}
