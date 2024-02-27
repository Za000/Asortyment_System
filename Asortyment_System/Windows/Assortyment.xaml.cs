using Asortyment_System.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;
using Asortyment_System.Controllers;
using Microsoft.Identity.Client.NativeInterop;

namespace Asortyment_System.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy Assortyment.xaml
    /// </summary>
    public partial class Assortyment : Window
    {
        //Public
        public bool IsClosed { get; set; } = true;

        //Private
        private List<Asortyment> _Asortyment = new List<Asortyment>();
        private BarcodeReader Barcode = new BarcodeReader();
        private AsortymentDBContext _context;
        private string selectedEAN;
        public Assortyment()
        {
            InitializeComponent();
            IsClosed = false;
            this._context = new AsortymentDBContext();
            this._context.Database.EnsureCreated();

            update_product.Click += Update_productOnClick;

            this.AddHandler(Window.PreviewKeyDownEvent, new System.Windows.Input.KeyEventHandler(Window_KeyDown), true);

            var result = this._context.Asortyments.ToList();

            foreach (var asortyment in result)
            {
                this._context.Entry(asortyment)
                    .Collection(a => a.connected_EAN)
                    .Load();
            }

            var products = new List<Asortyment>();
            products.AddRange(result);
            this._Asortyment = products;

            assortyment_items.SelectedCellsChanged += new SelectedCellsChangedEventHandler(this.Asortyment_CellContentClick);

            ICollectionView itemsView = CollectionViewSource.GetDefaultView(products);
            assortyment_items.ItemsSource = itemsView;
            itemsView.Filter = new Predicate<object>(FilterAsortyment);

            this.Search.TextChanged += SearchOnKeyDown;
            

        }

        private void Update_productOnClick(object sender, RoutedEventArgs e)
        {
            var selectedProduct = this._context.Asortyments.Where(r => r.EAN.Equals(selectedEAN)).ToList();

            if (selectedProduct.Any())
            {
                foreach (var sel in selectedProduct)
                {

                    sel.EAN = this.EAN.Text;
                    sel.kodProduktu = this.EAN.Text;
                    sel.nazwaProduktu = string.IsNullOrEmpty(this.Name.Text)
                            ? sel.nazwaProduktu
                            : this.Name.Text;
                    sel.opis = string.IsNullOrEmpty(this.Description.Text) ? sel.opis : this.Description.Text;
                        sel.cena = string.IsNullOrEmpty(this.Price.Text) ? sel.cena : float.Parse(this.Price.Text);
                    sel.stanMagazynowy = string.IsNullOrEmpty(this.Quantity.Text)
                            ? sel.stanMagazynowy
                            : Convert.ToInt32(this.Quantity.Text);
                    sel.image = sel.image;
                    sel.connected_EAN = sel.connected_EAN;

                }

                var result = this._context.SaveChanges();

                if (result > 0)
                {
                    MessageBox.Show("Produkt został zapisany", "Edycja produktu");
                    ((ICollectionView)this.assortyment_items.ItemsSource).Refresh();
                }
                else
                {
                    MessageBox.Show("Błąd podczas zapisu produktu", "Edycja produktu");
                }
            }
            else
            {
                MessageBox.Show($"Brak w bazie produktu o kodzie {selectedEAN}", "Edycja produktu");
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            this.Barcode.initalizeRead();

            if (e.Key == Key.Enter)
            {
                if (this.Barcode.isBarcodeDevice(300))
                {
                    string barcode = this.Barcode.CodeToText();
                    this.Search.Text = barcode;
                }
            }

            this.Barcode._barcode += e.Key != Key.Return ? e.Key.ToString() : "";
        }

        private void SearchOnKeyDown(object sender, TextChangedEventArgs e)
        {
            ICollectionView itemsView = CollectionViewSource.GetDefaultView(assortyment_items.ItemsSource);
            ((ICollectionView)this.assortyment_items.ItemsSource).Refresh();
        }

        private void Asortyment_CellContentClick(object sender, SelectedCellsChangedEventArgs e)
        {
            Asortyment selected = assortyment_items.CurrentItem as Asortyment;
            ConnectedEAN_items.Items.Clear();
            selected = selected == null ? new Asortyment() : selected;
            selected.connected_EAN = selected.connected_EAN == null ? new List<ConnectedEAN>() : selected.connected_EAN;

            EAN.Text = selected.EAN;
            Name.Text = selected.nazwaProduktu;
            Description.Text = selected.opis;
            Price.Text = selected.cena.ToString();
            Quantity.Text = selected.stanMagazynowy.ToString();
            selectedEAN = selected.EAN;

            if (selected.connected_EAN.Any())
            {
                List<ConnectedEAN> c_EAN = selected.connected_EAN;
                foreach (var c in c_EAN)
                {
                    ConnectedEAN_items.Items.Add(c);
                }
            }
        }

        private bool FilterAsortyment(object o)
        {
            if (string.IsNullOrEmpty(this.Search.Text)) return true;
            var search = (Asortyment)o;
            long SearchBoxOut;

            if (long.TryParse(this.Search.Text, out SearchBoxOut))
            {
                if (SearchBoxOut == long.Parse(search.EAN)) return true;
            }
            else
            {
                if (search.nazwaProduktu.ToLower().Contains(this.Search.Text.ToLower())) return true;
            }

            if (search.connected_EAN.Where(r => r.LinkedEAN.Equals(this.Search.Text)).Any()) return true;

            return false;
        }

    }
}
