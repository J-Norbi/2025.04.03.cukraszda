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
    /// Interaction logic for CakeWindow.xaml
    /// </summary>
    public partial class CakeWindow : Window
    {
        Cake oneCake;
        public CakeWindow(Cake oneCake)
        {
            InitializeComponent();
            this.oneCake = oneCake;
            Start();
        }
        void Start()
        {
            CakeName.Content = oneCake.name;
            CakePic.Source = new BitmapImage(new Uri(oneCake.picture));
            CakeDesk.Text = oneCake.description;
            CakeCount.Text = "1";
            CakeCount.IsEnabled = false;
        }
        private void More(object s, EventArgs e)
        {
            int number = int.Parse(CakeCount.Text);
            if(number < oneCake.stock)
            {
                number++;
                CakeCount.Text = number.ToString();
            }
        }
        private void Less(object s, EventArgs e)
        {
            int number = int.Parse(CakeCount.Text);
            if(number > 1)
            {
                number--;
                CakeCount.Text = number.ToString();
            }
        }
        private void Add(object s, EventArgs e)
        {
            oneCake.orderCount = int.Parse(CakeCount.Text);
            if(Cart.cart.Where(cake => cake.name == oneCake.name).Count() > 0)
            {
                Cart.cart.Where(cake => cake.name == oneCake.name).First().orderCount += int.Parse(CakeCount.Text);
            }
            else
            {
                Cart.cart.Add(oneCake);
            }
            oneCake.stock -= int.Parse(CakeCount.Text);
            this.Close();
        }
        private void Cancel(object s, EventArgs e)
        {
            this.Close();
        }
        
    }
}
