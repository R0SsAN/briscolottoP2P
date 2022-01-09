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
        public float valore { get; set; }
        public int punteggio { get; set; }
        public string img { get; set; }

        public Carta(string seme, float valore, int punteggio, string img, int numero)
        {
            this.seme = seme;
            this.valore = valore;
            this.punteggio = punteggio;
            this.img = img;
            this.numero = numero;
        }

    }
}
