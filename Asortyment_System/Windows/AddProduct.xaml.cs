using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Asortyment_System.Controllers;
using Microsoft.Win32;
using static System.Net.Mime.MediaTypeNames;

namespace Asortyment_System.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        private AsortymentDBContext dbContext;
        public bool IsClosed { get; set; } = true;
        public AddProduct()
        {
            InitializeComponent();
            IsClosed = false;
            this.dbContext = new AsortymentDBContext();
            this.EAN.TextChanged += new TextChangedEventHandler(CheckIsDigit);
            this.Quantity.TextChanged += new TextChangedEventHandler(CheckIsDigit);
            this.Price.TextChanged += new TextChangedEventHandler(CheckIsFloat);
            this.productImage.MouseLeftButtonDown += new MouseButtonEventHandler(ProductImage_Choose);
            this.add_product.Click += Add_productOnClick;
        }

        private void Add_productOnClick(object sender, RoutedEventArgs e)
        {
            Asortyment product = new Asortyment()
            {
                EAN = this.EAN.Text,
                kodProduktu = "",
                nazwaProduktu = this.Name.Text,
                opis = this.Description.Text,
                cena = float.Parse(this.Price.Text.Replace(".",",")),
                stanMagazynowy = Convert.ToInt32(this.Quantity.Text),
                image = string.IsNullOrEmpty(this.productImage.Source.ToString().Replace("file:///", "")) ? "" : this.productImage.Source.ToString().Replace("file:///", ""),
                connected_EAN = new List<ConnectedEAN>()
            };

            dbContext.Asortyments.Add(product);
            var result = dbContext.SaveChanges();

            if (result > 0)
            {
                MessageBox.Show("Produkt został dodany", "Dodawanie produktu");
            }
            else
            {
                MessageBox.Show("Wystąpił błąd podczas dodawania produktu", "Dodawanie produktu");
            }
        }

        private void ProductImage_Choose(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            
            dlg.Title = "Open Image";
            dlg.Filter = "bmp files (*.bmp)|*.bmp|(*.jpg)|*.jpg|(*.png)|*.png";
            if (dlg.ShowDialog() == true)
            {
                Uri imageFile = new Uri(dlg.FileName);
                productImage.Source = new BitmapImage(imageFile);
            }
            
        }

        private void CheckIsDigit(object sender, TextChangedEventArgs e)
        {
            GroupBox txtBox = ((GroupBox)((TextBox)sender).Parent);
            Match regLetters = Regex.Match(((TextBox)sender).Text, @"\D", RegexOptions.IgnoreCase);
            if (regLetters.Success)
            {
                MessageBox.Show($"Dla {txtBox.Header} dozwolone są jedynie wartości liczbowe");
                ((TextBox)sender).Text = ((TextBox)sender).Text.Remove(regLetters.Index,regLetters.Length);
            }
        }

        private void CheckIsFloat(object sender, TextChangedEventArgs e)
        {
            GroupBox txtBox = ((GroupBox)((TextBox)sender).Parent);
            Match regLetters = Regex.Match(((TextBox)sender).Text, @"^[0-9]*(?:\.[0-9]*)?$", RegexOptions.IgnoreCase);
            if (!regLetters.Success)
            {
                MessageBox.Show($"Dla {txtBox.Header} dozwolone są jedynie wartości zmiennoprzecinkowe");
                ((TextBox)sender).Text = ((TextBox)sender).Text.Remove(0);
            }
        }
    }
}
