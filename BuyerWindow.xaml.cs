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
using System.Windows.Shapes;

namespace _2025._04._03._cukrászda
{
    /// <summary>
    /// Interaction logic for BuyerWindow.xaml
    /// </summary>
    public partial class BuyerWindow : Window
    {
        List<Cake> allCake = new List<Cake>();
        ServerConnection connection;
        public BuyerWindow(ServerConnection connection)
        {
            InitializeComponent();
            this.connection = connection;       // a localis változót a this szóval különböztetem meg a 
            Start();
        }
        async void Start()
        {
            //allCake.Add(new Cake("Házi francia krémes", 1000, "http://daubnercukraszda.hu/sites/default/files/imagecache/node350/hazi_francia_kremes_0.png") { id = "Kolbász"});
            allCake = await connection.GetCakes();      // ha async functiont hívok meg (return type-ja Task) akkor csak async function-ba hívhatom meg
            BuildItems();
            Cart.cart.CollectionChanged += CartChanged;
        }
        void BuildItems()
        {
            foreach(Cake item in allCake)
            {
                Grid oneCake = new Grid();
                oneCake.Height = 280;
                oneCake.Width = 120;
                oneCake.Margin = new Thickness(5);
                for(int i=0; i < 4;  i++)
                {
                    RowDefinition row = new RowDefinition();
                    if(i == 1)
                    {
                        row.Height = new GridLength(2,GridUnitType.Star);
                    }
                    oneCake.RowDefinitions.Add(row);
                }
                Label title = new Label() { Content = item.name };
                oneCake.Children.Add(title);
                Image picture = new Image();
                picture.Source = new BitmapImage(new Uri(item.picture));
                picture.Stretch = Stretch.UniformToFill;
                oneCake.Children.Add(picture);
                oneCake.Background = new SolidColorBrush(Colors.Bisque);
                Grid.SetRow(picture, 1);
                Label price = new Label() { Content = item.price + " Ft/db" };
                oneCake.Children.Add(price);
                Grid.SetRow(price, 2);
                Border BuyBorder = new Border() { CornerRadius = new CornerRadius(10), Background = new SolidColorBrush(Colors.Orange), Margin = new Thickness(10, 2, 10, 2)};
                oneCake.Children.Add(BuyBorder);
                Grid.SetRow(BuyBorder, 3);
                Label BuyLabel = new Label() { Content = "Kosárba" };
                BuyLabel.Tag = item.id;
                //BuyLabel.MouseLeftButtonUp += Buy;
                BuyBorder.Child = BuyLabel;

                if(item.stock > 0)
                {
                    BuyLabel.MouseLeftButtonUp += Buy;
                    BuyLabel.MouseEnter += HoverOver;
                    BuyLabel.MouseLeave += HoverLeave;
                }
                else
                {
                    BuyBorder.Background = new SolidColorBrush(Colors.AliceBlue);
                    BuyLabel.Content = "Elfogyott";
                }

                Items.Children.Add(oneCake);
            }
        }
        async void CartClick(object s, EventArgs e)
        {
            //MessageBox.Show("jó");
            if(Cart.cart.Count == 0)
            {
                MessageBox.Show("Nem vettél még semmit");
                return;
            }
            await connection.BuyCakes();
        }
        void CartChanged(object sender, EventArgs e)
        {
            if(Cart.cart.Count >0)
            {
                CartBorder.Background = new SolidColorBrush(Colors.Red);
                CartNumber.Content = Cart.cart.Count.ToString();
            }
            else
            {
                CartBorder.Background = new SolidColorBrush(Colors.White);
            }
        }
        void HoverOver(object s, EventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand; 
        }
        void HoverLeave(object s, EventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }
        void Buy(object sender, RoutedEventArgs e)
        {
            if(!(sender is Label))
            {
                return;
            }
            Label oneLabel = sender as Label;
            string id = oneLabel.Tag.ToString();
            //MessageBox.Show("Az elem ára: " + allCake.Where(cake => cake.id == id).First().price);  // az allCake-ből azokat a cake-eket választja, amik = id-val, de a First() a legelsőnek az árát választja ki.
            //  collectiont ad vissza, ezért kell a First(), mert valószínűleg több azonos id nem lesz.
            CakeWindow oneCakeWindow = new CakeWindow(allCake.Where(cake => cake.id == id).First());
            oneCakeWindow.Show();
        }
    }
}
