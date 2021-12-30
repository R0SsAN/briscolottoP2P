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
        public List<Carta> carteTavolo;
        // vettore carte in mano, utilizzo un vettore invece di una lista per poter gestire correttamente la presa e l'inserimento per ogni giocata
        public Carta[] carteMano;
        //lista carte vinte
        public List<Carta> carteVinte;


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
            carteMano = new Carta[3];
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

        public string calcoloVincitaPerdita()
        {
            string invio = "";
            Carta briscola = mazzo.briscola;
            //questo metodo viene invocato dal secondo giocatore che calcola il vincitore della mano attuale
            //come si calcola chi ha vinto:
            // - se un giocatore butta una briscola e l'altro no, ha vinto il primo
            // - se tutti e due i giocatori buttano una briscola vince quello con il valore più alto
            // - se i giocatori giocano una carta dello stesso seme vince quello con il valore più alto
            // - se i giocatori giocano due semi diversi non briscola vince chi ha giocato la mano per primo

            //in questo caso il primo giocatore ha buttato una carta di briscola mentre il secondo no, vince il primo
            if (briscola.seme == carteTavolo[0].seme && briscola.seme != carteTavolo[1].seme)
                invio = "w;";
            //in questo caso il secondo giocatore ha buttato una carta di briscola mentre il primo no, vince il secondo
            else if (briscola.seme != carteTavolo[0].seme && briscola.seme == carteTavolo[1].seme)
                invio = "l;";
            //in questo caso tutti e due i giocatori hanno buttato una briscola, quindi vince quello con il valore più alto
            else if (briscola.seme == carteTavolo[0].seme && briscola.seme == carteTavolo[1].seme)
            {
                if (carteTavolo[0].valore > carteTavolo[1].valore)
                    invio = "w;";
                else if (carteTavolo[0].valore < carteTavolo[1].valore)
                    invio = "l;";
            }
            //in questo caso tutti e due i giocatori hanno buttato una carta seme non briscola, vince quello con il valore più alto
            else if (carteTavolo[1].seme == carteTavolo[1].seme && briscola.seme != carteTavolo[0].seme)
            {
                if (carteTavolo[0].valore > carteTavolo[1].valore)
                    invio = "w;";
                else if (carteTavolo[0].valore < carteTavolo[1].valore)
                    invio = "l;";
            }
            //in questo caso le due carte sono di semi diversi, vince il primo giocatore
            else if (carteTavolo[1].seme != carteTavolo[1].seme && briscola.seme != carteTavolo[0].seme)
                invio = "w;";

            return invio;
        }
        public void calcoloPunti()
        {
            int temp = 0;
            for (int i = 0; i < carteVinte.Count; i++)
            {
                temp += carteVinte[i].punteggio;
            }
            if (temp > 60)
            {
                //viusalliza in label hai vinto
            }
            else if (temp < 60)
            {
                //viusalliza in label hai perso 
            }
            else
            {
                //viusalliza in label hai pareggio
            }
        }
        public int getNCarteMano()
        {
            int num = 0;
            for (int i = 0; i < carteMano.Length; i++)
            {
                if (carteMano[i] != null)
                    num++;
            }
            return num;
        }
        public void inserisciCartaManoInPos(Carta carta, int pos)
        {
            //uso questo metodo per poter inserire nel vettore delle carte in mano le nuove carta
            // se gli passo pos=-1 vuol dire che voglio inserire alla prima posizione disponibile (usato per il pescaggio iniziale)
            // se gli passo una posizione specifica vuol dire che voglio inserire in quella posizione (usato per l'inserimento delle carte alla fine di ogni mano)
            if (pos == -1)
                carteMano[getNCarteMano()] = carta;
            else
            {
                carteMano[pos] = carta;
            }
        }
    }
}
