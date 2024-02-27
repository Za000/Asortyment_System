using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SQLite;

namespace Asortyment_System.Controllers
{
    public class ImportDatabase
    {
        private readonly AsortymentDBContext _dbContext;

        public ImportDatabase(AsortymentDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Seed(string dbPath)
        {
            int changesSaved = 0;
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Asortyments.Any())
                {
                    var asortyment = getAsortyment(dbPath);
                    _dbContext.Asortyments.AddRange(asortyment);
                    changesSaved = _dbContext.SaveChanges();
                }
                else
                {
                    MessageBox.Show("W bazie są już rekordy");
                    changesSaved = -1;
                }
            }
            else
            {
                MessageBox.Show("Błąd podczas łączenia się z bazą danych");
                changesSaved = -1;
            }

            return changesSaved;
        }

        public void importDatabase(string dbPath)
        {
            int updated = 0;
            if ((updated = this.Seed(dbPath)) > -1)
            {
                MessageBox.Show($"Pomyślnie zimportowano bazę danych produktów, zaktualizowane wartosci: {updated}");
            }
            else
            {
                MessageBox.Show($"Wystąpił błąd");
            }
        }


        private IEnumerable<Asortyment> getAsortyment(string dbPath)
        {
            var attachedEANs = new List<string>();
            var asortyment = new List<Asortyment>();
            using (SQLiteConnection connection = new SQLiteConnection($"URI=file:{dbPath}"))
            {
                connection.Open();
                SQLiteCommand PLU = new SQLiteCommand("SELECT * FROM Table_PLU", connection);
                SQLiteDataReader s_PLU = PLU.ExecuteReader();
                SQLiteCommand A;
                SQLiteDataReader s_A;

                while (s_PLU.Read())
                {
                    Asortyment produkt = new Asortyment()
                    {
                        EAN = s_PLU.GetValue(1).ToString(),
                        kodProduktu = s_PLU.GetValue(1).ToString(),
                        nazwaProduktu = s_PLU.GetValue(3).ToString(),
                        cena = float.Parse(s_PLU.GetValue(5).ToString()),
                        stanMagazynowy = 0,
                        connected_EAN = new(),
                        image = "",
                        opis = ""
                    };
                   

                    A = new SQLiteCommand($"SELECT * FROM `Table_h#01_TSLinkedPLU4000` WHERE `DataFieldString!EAN!` = {s_PLU.GetValue(1)}", connection);
                    s_A = A.ExecuteReader();

                    List<ConnectedEAN> connected = new List<ConnectedEAN>();

                    while (s_A.Read())
                    {
                        if (!attachedEANs.Contains(s_A.GetValue(1).ToString()))
                        {
                            connected.Add(new ConnectedEAN()
                            {
                                BaseEAN = s_A.GetValue(0).ToString(),
                                LinkedEAN = s_A.GetValue(1).ToString(),
                                price = Convert.ToDouble(s_A.GetValue(2)),
                                quantity = Convert.ToInt32(s_A.GetValue(3))
                            });
                            attachedEANs.Add(s_A.GetValue(1).ToString());
                        }
                        
                    }

                    if (connected.Any())
                    {
                        produkt.connected_EAN = connected;
                    }

                    asortyment.Add(produkt);
                }
                
                connection.Close();
            }

            return asortyment;
        }
    }
}
