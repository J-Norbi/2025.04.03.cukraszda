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
        List<Cake> allCake = new List<Cake>();
        public SellerWindow()
        {
            InitializeComponent();
            connection = new ServerConnection("http://127.1.1.1:3000");
            Load();
        }
        async void Load()
        {
            allCake = await connection.GetCakes();
            foreach (Cake oneCake in allCake)
            {
                Grid oneGrid = new Grid();
                cakeList.Children.Add(oneGrid);
                for (int i = 0; i < 5; i++) { oneGrid.ColumnDefinitions.Add(new ColumnDefinition()); }
                oneGrid.ColumnDefinitions[0].Width = new GridLength(2, GridUnitType.Star);      // süti neve oszlop
                oneGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);      // süti ára oszlop
                oneGrid.ColumnDefinitions[2].Width = new GridLength(20);                        // csökkentés button
                oneGrid.ColumnDefinitions[3].Width = new GridLength(20);                        // mennyiség mező
                oneGrid.ColumnDefinitions[4].Width = new GridLength(20);                        // növekvő button
                Label firstLabel = new Label() { Content = oneCake.name };
                Label secondLabel = new Label() { Content = oneCake.price + " Ft", HorizontalAlignment = HorizontalAlignment.Right };
                Button minusButton = new Button() { Content = "-", FontSize = 15 };
                TextBox input = new TextBox() { Text = "0", FontSize = 15, Margin = new Thickness(0) };
                Button plusButton = new Button() { Content = "+", FontSize = 15};
                Grid.SetColumn(firstLabel, 0);
                Grid.SetColumn(secondLabel, 1);
                Grid.SetColumn(minusButton, 2);
                Grid.SetColumn(input, 3);
                Grid.SetColumn(plusButton, 4);
                minusButton.Click += minusEvent;
                plusButton.Click += plusEvent;
                minusButton.Tag = input;
                plusButton.Tag = input;
                input.TextChanged += TextChanged;
                input.Tag = 0;
                oneGrid.Children.Add(firstLabel);
                oneGrid.Children.Add(secondLabel);
                oneGrid.Children.Add(minusButton);
                oneGrid.Children.Add(input);
                oneGrid.Children.Add(plusButton);
                
            }
        }
        void TextChanged(object s, EventArgs e)
        {
            TextBox oneSomething = s as TextBox;
            int number = 0;
            if(!int.TryParse(oneSomething.Tag.ToString(), out number))
                MessageBox.Show($"Hibás tag érték: {oneSomething.Tag}");
            int newNumber = 0;
            if(int.TryParse(oneSomething.Text, out newNumber))
                oneSomething.Tag = newNumber;
            else
                oneSomething.Text = number.ToString();
        }
        void plusEvent(object s, EventArgs e)
        {
            FrameworkElement oneButton = s as FrameworkElement;     // azért FrameworkElement, hogy ha nem tudom, hogy Button, Label, vagy más ilyen FrameworkElement-et használok.
            TextBox oneTextbox = oneButton.Tag as TextBox;
            int number = 0;
            if (int.TryParse(oneTextbox.Text, out number))          // a szövegből megpróbál egy int-et létrehozni, amit out után beletesz egy változóba (most number)
            {
                number++;
                oneTextbox.Text = number.ToString();
            }
            else
            {
                MessageBox.Show("Hibás érték a textboxban!");
            }
        }
        void minusEvent(object s, EventArgs e)
        {
            FrameworkElement oneButton = s as FrameworkElement;
            TextBox oneTextbox = oneButton.Tag as TextBox;
            int number = 0;
            if (int.TryParse(oneTextbox.Text, out number))    
            {
                number--;
                if(number >= 0)
                {
                    oneTextbox.Text = number.ToString();
                }
            }
            else
            {
                MessageBox.Show("Hibás érték a textboxban!");
            }
        }
        void OnFocus(object s, EventArgs e)
        {
            TextBox oneTextBox = s as TextBox;
            if (oneTextBox.Text == oneTextBox.Tag.ToString())
            {
                oneTextBox.Clear();
                oneTextBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
        void OffFocus(object s, EventArgs e)
        {
            TextBox oneTextBox = s as TextBox;
            if (oneTextBox.Text == "")
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
        void Save(object s, EventArgs e)
        {

        }
    }
}
