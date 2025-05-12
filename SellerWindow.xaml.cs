using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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
    /// Interaction logic for SellerWindow.xaml
    /// </summary>
    public partial class SellerWindow : Window
    {
        ServerConnection connection;
        public SellerWindow()
        {
            InitializeComponent();
            connection = new ServerConnection("http://127.1.1.1:3000");
            
        }
        void OnFocus(object s, EventArgs e)
        {
            TextBox oneTextBox = s as TextBox;
            if(oneTextBox.Text == oneTextBox.Tag.ToString())
            {
                oneTextBox.Clear();
                oneTextBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
        void OffFocus(object s, EventArgs e)
        {
            TextBox oneTextBox = s as TextBox;
            if(oneTextBox.Text == "")
            {
                oneTextBox.Text = oneTextBox.Tag.ToString();
                oneTextBox.Foreground = new SolidColorBrush(Colors.LightSlateGray);
            }
        }
        async void AddCake(Object s, EventArgs e)
        {
            Cake oneCake = new Cake()
            {
                name = NameInput.Text,
                price = int.Parse(PriceInput.Text),
                stock = int.Parse(StockInput.Text),
                picture = PictureInput.Text,
                description = DescriptionInput.Text,
                allergenes = AllergenesInput.Text
            };
            await connection.PostCake(oneCake);
        }
        void Cancel(object s, EventArgs e)
        {
            MainWindow b = new MainWindow() { Top = this.Top, Left = this.Left, Visibility = Visibility.Visible };
            this.Close();
        }
    }
}
