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
using NorthEntitiesApp.Models;
namespace NorthEntitiesWPFAPP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly NorthWindContext db = new();
        public MainWindow()
        {
            InitializeComponent();
            Dgrid.Visibility = Visibility.Collapsed;
            Style style = this.FindResource("BtnStyle") as Style;
            foreach (Button btn in Buttons.Children)
            {
                btn.Style = style;
            }
        }

        private void Btn1_OnClick(object sender, RoutedEventArgs e)
        {
            var query = from prd in db.Products
                        select new { prd.ProductName, prd.QuantityPerUnit };
            FillGrid(query);
        }

        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            var query = from prd in db.Products
                        where prd.Discontinued == false
                        select new{prd.ProductId,prd.ProductName };
            FillGrid(query);
        }
        private void FillGrid(IEnumerable<Object> query)
        {
            Dgrid.ItemsSource = query.ToList();
            Dgrid.Visibility = Visibility.Visible;
        }

        private void Btn3_Click(object sender, RoutedEventArgs e)
        {
            var query = from prd in db.Products
                        where prd.Discontinued == true
                        select new { prd.ProductId, prd.ProductName };
            FillGrid(query);
        }

        private void Btn4_Click(object sender, RoutedEventArgs e)
        {
            var maxPrice = db.Products.Max(x => x.UnitPrice).Value;
            var miPrice = db.Products.Min(x => x.UnitPrice).Value;

            var query = from prd in db.Products
                        where prd.UnitPrice == miPrice || prd.UnitPrice==maxPrice
                     select new {prd.ProductId, prd.ProductName,prd.UnitPrice};



            FillGrid(query);
        }

        private void Btn9_Click(object sender, RoutedEventArgs e)
        {
            var query = from prd in db.Products
                        group prd by prd.Discontinued into discGroup
                        select new
                        {
                         
                            Count = discGroup.Count(),
                            discontinuedd = discGroup.Key.ToString()
                        };
            FillGrid(query);


        }

        private void btn5_click(object sender, RoutedEventArgs e)
        {
            var query= from prd in db.Products
                       where prd.Discontinued==false && prd.UnitPrice<20
                       select new { prd.ProductId,prd.ProductName, prd.UnitPrice};
                 FillGrid(query);
        }

        private void btn6_click(object sender, RoutedEventArgs e)
        {
            var query= from prd in db.Products
                       where prd.UnitPrice>=15 && prd.UnitPrice<=25
                       select new { prd.ProductId, prd.ProductName, prd.UnitPrice };
            FillGrid(query);
        }

        private void Btn7_click(object sender, RoutedEventArgs e)
        {
            var query= from prd in db.Products
                       where prd.UnitPrice>(db.Products.Average(x=>x.UnitPrice))
                       orderby prd.UnitPrice descending
                       select new { prd.ProductId, prd.ProductName, prd.UnitPrice };
            FillGrid(query); 
        }

        private void Btn8_click(object sender, RoutedEventArgs e)
        {
            var query = db.Products.OrderByDescending(x => x.UnitPrice).Select(x=> new { x.ProductName, x.UnitPrice }).Take(10);
            FillGrid(query);
        }

        private void Btn10_click(object sender, RoutedEventArgs e)
        {
            var query = db.Products.Where(x => x.UnitsInStock < x.UnitsOnOrder).Select(x => new { x.ProductName, x.UnitsOnOrder, x.UnitsInStock }).OrderBy(x=>x.UnitsInStock).ThenBy(x=>x.UnitsOnOrder);
            FillGrid(query);
        }
    }
}
