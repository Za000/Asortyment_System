using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asortyment_System.Controllers
{
    public class ConnectedEAN
    {
        [Key]public string LinkedEAN { get; set; }
        public string BaseEAN { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
    }
}
