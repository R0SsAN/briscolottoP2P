using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace briscolottoP2P
{
    public class Mazzo
    {
        public List<Carta> mazzo;
        public Mazzo()
        {
            mazzo = new List<Carta>();
            caricaMazzo();
        }
        public void caricaMazzo()
        {
            //caricare il mazzo di carte con relativi valori e immagini
        }
        public void randomizzaMazzo()
        {
            Random rng = new Random();
            List<Carta> temp = mazzo.OrderBy(a => rng.Next()).ToList();
            mazzo = temp;
        }
    }
}
