using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Asortyment_System.Controllers;
using Asortyment_System.Windows;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;

namespace Asortyment_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private AddProduct _addProduct = null;
        private Assortyment _assortyment = null;
        private Settings _settings = null;
        private BarcodeReader Barcode = new BarcodeReader();
        private AsortymentDBContext _context = new AsortymentDBContext();
        public MainWindow()
        {
            InitializeComponent();
            addProduct.Click += new RoutedEventHandler(AddProduct_Click);
            deleteProduct.Click += new RoutedEventHandler(DeleteProduct_Click);
            assortment.Click += new RoutedEventHandler(Assortyment_Click);
            settings.Click += new RoutedEventHandler(Settings_Click);
            this.AddHandler(Window.PreviewKeyDownEvent, new System.Windows.Input.KeyEventHandler(Window_KeyDown), true);
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            this.Barcode.initalizeRead();

            if (e.Key == Key.Enter)
            {
                if (this.Barcode.isBarcodeDevice(300))
                {
                    string barcode = this.Barcode.CodeToText();
                    if (_context.isProductInDatabase(barcode))
                    {
                        ProductFoundMessageBox found = new(barcode);
                        found.ShowDialog();
                    }
                    else
                    {
                        ProductNotFoundMessageBox notFound = new();
                        notFound.add_product.Click += (o, args) =>
                        {
                            this._addProduct = new AddProduct();
                            this._addProduct.EAN.Text = barcode;
                            this._addProduct.Closed += (o, args) => ((AddProduct)o).IsClosed = true;
                            this._addProduct.Show();
                            notFound.Close();
                        };

                        notFound.add_connected.Click += (o, args) =>
                        {
                            AddConnectedMessageBox _addEAN = new AddConnectedMessageBox();
                            _addEAN.EANToAdd = barcode;
                            _addEAN.ShowDialog();
                            notFound.Close();
                        };
                        notFound.ShowDialog();
                    }
                }
            }

            this.Barcode._barcode += e.Key != Key.Return ? e.Key.ToString() : "";
        }


        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (_addProduct == null || _addProduct.IsClosed)
            {
                _addProduct = new AddProduct();
                _addProduct.Closed += (o, args) => ((AddProduct)o).IsClosed = true;
                _addProduct.Show();
            };
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Delete Product");
        }

        private void Assortyment_Click(object sender, RoutedEventArgs e)
        {
            if (_assortyment == null || _assortyment.IsClosed)
            {
                _assortyment = new Assortyment();
                _assortyment.Closed += (o, args) => ((Assortyment)o).IsClosed = true;
                _assortyment.Show();
            };
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            if (_settings == null || _settings.IsClosed)
            {
                _settings = new Settings();
                _settings.Closed += (o, args) => ((Settings)o).IsClosed = true;
                _settings.Show();
            };
        }

        private void importData_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDatabase = new OpenFileDialog();
            openDatabase.Filter = "Database File (*.db)|*.db";
            openDatabase.FilterIndex = 1;
            openDatabase.Multiselect = true;
            openDatabase.CheckFileExists = true;
            openDatabase.CheckPathExists = true;
            var dialog = openDatabase.ShowDialog();

            if (openDatabase.CheckFileExists && openDatabase.FileName.Any() && dialog == System.Windows.Forms.DialogResult.OK)
            {
                ImportDatabase import = new ImportDatabase(new AsortymentDBContext());
                string dbPath = openDatabase.FileName;
                import.importDatabase(dbPath);
            }
        }
    }
}
