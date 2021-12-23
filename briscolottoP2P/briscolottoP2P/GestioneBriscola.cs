using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace briscolottoP2P
{
    class GestioneBriscola
    {
        //singleton
        static GestioneBriscola _instance = null;
        static public GestioneBriscola getInstance(MainWindow w)
        {
            if (_instance == null)
                _instance = new GestioneBriscola(w);
            return _instance;
        }
        //VARIABILI

        //indica lo stato della connessione attuale
        //  0 -> nessuna connessione attiva, in attesa
        //  1 -> inviata richiesta connessione, in attesa di risposta
        //  2 -> connessione avviata
        public int statoConnessione;



        public GestioneBriscola(MainWindow w)
        {

        }


    }
}
