using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace briscolottoP2P
{
    public class GestioneBriscola
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

        public GestioneInvio invio;
        public GestioneRicezione ricezione;

        //mazzo contenente tutte le carte
        public Mazzo mazzo;

        //interfaccia per poter modificare la grafica
        public MainWindow interfaccia;

        //indica lo stato della connessione attuale
        //  0 -> nessuna connessione attiva, in attesa
        //  1 -> inviata richiesta connessione, in attesa di risposta dal destinatario
        //  2 -> ricevuta richiesta connessione, inviata conferma e in attesa di risposta dal mittente
        //  3 -> connessione avviata
        public int statoConnessione;

        //variabili che contengono il nome del giocatore local e di quello remoto con cui stà giocando
        public string nomeLocal;
        public string nomeRemote;

        public string ipDestinatario;


        // lista carte in tavolo
        List<Carta> carteTavolo;
        // lista carte in mano
        List<Carta> carteMano;
        //lista carte vinte
        List<Carta> carteVinte;


        private GestioneBriscola(MainWindow w)
        {
            invio = GestioneInvio.getInstance();
            ricezione = GestioneRicezione.getInstance();
            mazzo = new Mazzo();
            statoConnessione = 0;
            nomeLocal = "peer";
            nomeRemote = "peer";
            ipDestinatario = "";
            interfaccia = w;
            carteTavolo = new List<Carta>();
            carteMano = new List<Carta>();
            carteVinte = new List<Carta>();
        }
        public void avvioPartitaMazziere()
        {

            interfaccia.visibile();
            //in questo caso sono il mazziere quindi invio il mazzo e aspetto la sua conferma
            inviaMazzo();
            ricezione.waitConfermaMazzo();
            //dopo aver ricevuto la conferma di ricezione del mazzo l'altro giocatore inizia la giocata

        }
        public void avvioPartitaGiocatore()
        {
            interfaccia.visibile();
            //in questo caso sono il primo giocatore quindi ricevo il mazzo dal mazziere
            List<Carta> temp = ricezione.riceviMazzo();
            mazzo.sincronizzaMazzo(temp);
            //dopo averlo ricevuto invio conferma al mazziere
            invio.invioGenerico(ipDestinatario, "m;y;");
            //inizio giocata
        }

        public void inviaMazzo()
        {
            mazzo = new Mazzo();
            mazzo.randomizzaMazzo();
            invio.inviaMazzo(mazzo.mazzo);
        }

        public void calcoloPunti()
        {
            int temp = 0;
            for (int i = 0; i < carteVinte.Count; i++)
            {
                temp += carteVinte[i].punteggio;
            }
            if(temp>60)
            {
                //viusalliza in label hai vinto
            }else if(temp<60)
            {
                //viusalliza in label hai perso 
            }else
            {
                //viusalliza in label hai pareggio
            }
        }

    }
}
