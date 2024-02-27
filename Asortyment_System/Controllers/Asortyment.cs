using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asortyment_System.Controllers
{
    public class Asortyment
    {
        [Key]public string EAN { get; set; }
        public string kodProduktu { get; set; }
        public string nazwaProduktu { get; set; }
        public string opis { get; set; }
        public float cena { get; set; }
        public int stanMagazynowy { get; set; }
        public string image { get; set; }
        public List<ConnectedEAN> connected_EAN { get; set; }
    }
}
