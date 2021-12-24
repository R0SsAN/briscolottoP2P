using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace briscolottoP2P
{
    public class Carta
    {
        public string seme { get; set; }
        public int valore { get; set; }
        public int punteggio { get; set; }
        public string img { get; set; }

        public Carta(string seme, int valore, int punteggio, string img)
        {
            this.seme = seme;
            this.valore = valore;
            this.punteggio = punteggio;
            this.img = img;
        }

    }
}
