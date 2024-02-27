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
using Asortyment_System.Controllers;

namespace Asortyment_System.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy CustomMessageBox.xaml
    /// </summary>
    public partial class AddConnectedMessageBox : Window
    {
        private AsortymentDBContext dbContext;
        public string EANToAdd;
        public AddConnectedMessageBox()
        {
            InitializeComponent();
            this.dbContext = new AsortymentDBContext();
        }

        private void Sell_Click(object sender, RoutedEventArgs e)
        {
            if (this.dbContext.isProductInDatabase(this.EAN.Text))
            {
                var newConnectedEAN = new ConnectedEAN()
                {
                    LinkedEAN = this.EANToAdd,
                    BaseEAN = this.EAN.Text,
                    quantity = Convert.ToInt32(this.Quantity.Text),
                    price = Convert.ToDouble(this.Price.Text)
                };
                var assortymentList = this.dbContext.Asortyments.Where(r => r.EAN.Equals(this.EAN.Text)).ToList();

                foreach (var x in assortymentList)
                {
                    if (x.connected_EAN == null)
                    {
                        x.connected_EAN = new List<ConnectedEAN>();
                    }

                    x.connected_EAN.Add(newConnectedEAN);
                }

                var result = this.dbContext.SaveChanges();
                if (result > 0)
                {
                    MessageBox.Show("Produkt został dodany", "Dodawanie Kodu Dowiązanego");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Wystąpił błąd podczas dodawania produktu", "Dodawanie Kodu Dowiązanego");
                }
            }
            else
            {
                MessageBox.Show($"Nie ma w bazie produktu o kodzie {this.EAN.Text}");
            }
        }
    }
}
