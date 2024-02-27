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
    public partial class ProductFoundMessageBox : Window
    {
        private string EAN;
        public ProductFoundMessageBox(string EAN)
        {
            InitializeComponent();
            this.EAN = EAN;
            this.Edit.Click += EditOnClick;
        }

        private void EditOnClick(object sender, RoutedEventArgs e)
        {
            Assortyment _asortyment = new Assortyment();
            _asortyment.Search.Text = EAN;
            _asortyment.Show();
            this.Close();
        }
    }
}
