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



        public GestioneBriscola(MainWindow w)
        {

        }


    }
}
