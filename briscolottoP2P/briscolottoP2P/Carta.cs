using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace briscolottoP2P
{
    public class Carta
    {
        public int numero { get; set; }
        public string seme { get; set; }
        public int valore { get; set; }
        public string img { get; set; }

        public Carta(string seme, int valore, string img, int numero)
        {
            this.seme = seme;
            this.valore = valore;
            this.img = img;
            this.numero = numero;
        }

    }
}
