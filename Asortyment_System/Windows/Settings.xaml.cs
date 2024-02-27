using Asortyment_System.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
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

namespace Asortyment_System.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public bool IsClosed { get; set; } = true;

        private Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        private AsortymentDBContext db = new AsortymentDBContext();

        public Settings()
        {
            InitializeComponent();
            IsClosed = false;
            connectionString.Text = cfg.AppSettings.Settings["ConnectionString"].Value;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            cfg.AppSettings.Settings["ConnectionString"].Value = connectionString.Text;
            cfg.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");


            this.db.Database.SetConnectionString($"{cfg.AppSettings.Settings["ConnectionString"].Value};TrustServerCertificate=True;");



            if (this.db.Database.CanConnect())
            {
                try
                {
                    if (!this.db.Asortyments.Any())
                    {
                        this.db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    this.db.Database.Migrate();
                }
                MessageBox.Show("Zapisano zmiany");
            }
            else
            {
                MessageBox.Show("Błąd podczas łączenia się z bazą danych");
            }
        }
    }
}
