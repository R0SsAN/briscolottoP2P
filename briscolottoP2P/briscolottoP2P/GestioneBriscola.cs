using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

            //pescaggio iniziale delle 3 carte
            pescaggioIniziale(true);
            giocataSecondo();
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

            //pescaggio iniziale delle 3 carte
            pescaggioIniziale(false);
            giocataPrimo();
        }
        public void pescaggioIniziale(bool type)
        {
            interfaccia.aggiornaCarte();
            //type identifica se sono il mazziere o l'altro giocatore
            //  type == true   se sono il mazziere sono il primo a prendere la carta
            //  type == false  se sono l'altro giocatore devo aspettare che il mazziere prenda per primo la carta
            if (type)
            {
                for (int i = 0; i < 3; i++)
                {
                    pescaCarta(-1);
                    ricezione.waitPresaCarta();
                }
            }
            else
            {
                ricezione.waitPresaCarta();
                for (int i = 0; i < 2; i++)
                {
                    pescaCarta(-1);
                    ricezione.waitPresaCarta();
                }
                pescaCarta(-1);
            }
        }
        public void pescaCarta(int pos)
        {
            //pesco una carta e aggiorno anche l'altro giocatore
            if (getNCarteMano() < 3 && mazzo.sincronizzato.Count > 0)
            {
                Carta temp = mazzo.sincronizzato[0];
                mazzo.sincronizzato.RemoveAt(0);
                if (pos == -1)
                {
                    inserisciCartaManoInPos(temp, -1);
                    interfaccia.animazionePesca(getNCarteMano() - 1);
                }
                else
                {
                    inserisciCartaManoInPos(temp, pos);
                    interfaccia.animazionePesca(pos);
                }
                //avvio animazione carta
                Thread.Sleep(650);
                interfaccia.terminaAnimazione();
                invio.invioGenerico(ipDestinatario, "p;");
            }
            else
            {
                invio.invioGenerico(ipDestinatario, "p;");
            }

        }
        public void giocataPrimo()
        {
            interfaccia.aggiornaCarte();
            interfaccia.scelta = -1;
            if (getNCarteMano() == 0)
            {
                concludiPartita();
                return;
            }
            //faccio scegliere la carta all'utente
            interfaccia.puoiPrendere = true;
            interfaccia.visualizzaTurno(true);
            while (interfaccia.scelta == -1) ;
            interfaccia.puoiPrendere = false;
            interfaccia.animazioneButta(carteMano[interfaccia.scelta].img);
            Thread.Sleep(650);
            interfaccia.terminaAnimazione();
            interfaccia.visualizzaTurno(false);
            Carta temp = carteMano[interfaccia.scelta];
            //rimuovo carta scelta dal vettore di carte mie
            carteMano[interfaccia.scelta] = null;
            //la carico nel tavolo
            carteTavolo.Add(temp);
            //la invio all'altro giocatore in modo da sincronizzarsi
            invio.inviaCartaGiocata(temp);
            interfaccia.aggiornaCarte();

            //ora aspetto che l'altro giocatore mi mandi la sua carta giocata
            //ricevo la carta giocata dal secondo giocatore
            Carta giocata = ricezione.waitCartaGiocata();
            giocata = mazzo.completaCarta(giocata);
            //aggiungo anche io la carta del destinatario nel tavolo in modo da averlo sincronizzato
            carteTavolo.Add(giocata);

            //ora aspetto dal secondo giocatore l'esito della mano
            bool esito = ricezione.waitEsitoGiocata();
            //se ho perso la mano
            if (!esito)
            {
                mostraEsito(false);
                //rimuovo le carte dal tavolo (che ha vinto l'altro giocatore)
                carteTavolo.Clear();

                //ora aspetto che l'altro giocatore peschi la carta per la prossima mano
                ricezione.waitPresaCarta();
                //ora pesco anche io la carta
                pescaCarta(interfaccia.scelta);
                //ora visto che ho perso la mano inizio come secondo giocatore
                giocataSecondo();
            }
            //se invece ho vinto la mano
            else if (esito)
            {
                mostraEsito(true);
                //inserisco le carte del tavolo nelle mie carte vinte
                carteVinte.Add(carteTavolo[0]);
                carteVinte.Add(carteTavolo[1]);
                //svuoto il tavolo
                carteTavolo.Clear();

                //ora pesco la carta per la prossima mano
                pescaCarta(interfaccia.scelta);
                //ora aspetto che l'altro giocatore peschi anche lui la carta
                ricezione.waitPresaCarta();
                //ora visto che ho vinto ricomincio la prossima mano come primo giocatore
                giocataPrimo();
            }

        }
        public void giocataSecondo()
        {
            interfaccia.aggiornaCarte();
            interfaccia.scelta = -1;
            if (getNCarteMano() == 0)
            {
                concludiPartita();
                return;
            }
            //ricevo la carta giocata dal primo giocatore
            Carta giocata = ricezione.waitCartaGiocata();
            giocata = mazzo.completaCarta(giocata);
            //aggiungo anche io la carta del destinatario nel tavolo in modo da averlo sincronizzato
            carteTavolo.Add(giocata);
            interfaccia.aggiornaCarte();
            //faccio scegliere la carta all'utente
            interfaccia.puoiPrendere = true;
            interfaccia.visualizzaTurno(true);
            while (interfaccia.scelta == -1) ;
            interfaccia.puoiPrendere = false;
            interfaccia.animazioneButta(carteMano[interfaccia.scelta].img);
            Thread.Sleep(650);
            interfaccia.terminaAnimazione();
            interfaccia.visualizzaTurno(false);
            Carta temp = carteMano[interfaccia.scelta];
            //rimuovo carta scelta dal vettore di carte mie
            carteMano[interfaccia.scelta] = null;
            //la carico nel tavolo
            carteTavolo.Add(temp);
            //la invio all'altro giocatore in modo da sincronizzarsi
            invio.inviaCartaGiocata(temp);
            interfaccia.aggiornaCarte();

            //ora devo vedere chi ha vinto o perso la giocata
            string esito = calcoloVincitaPerdita();

            //se ho vinto
            if (esito == "l;")
            {
                mostraEsito(true);
                //inserisco le carte del tavolo nelle mie carte vinte
                carteVinte.Add(carteTavolo[0]);
                carteVinte.Add(carteTavolo[1]);
                //svuoto il tavolo
                carteTavolo.Clear();
                //ora comunico all'altro giocatore che ha perso
                invio.invioGenerico(ipDestinatario, esito);

                //ora pesco la carta per la prossima mano
                pescaCarta(interfaccia.scelta);
                //ora aspetto che l'altro giocatore peschi anche lui la carta
                ricezione.waitPresaCarta();
                //ora visto che ho vinto ricomincio la prossima mano come primo giocatore
                giocataPrimo();
            }
            //se invece ho perso
            else if (esito == "w;")
            {
                mostraEsito(false);
                //svuoto il tavolo
                carteTavolo.Clear();
                //ora comunico all'altro giocatore che ha vinto
                invio.invioGenerico(ipDestinatario, esito);
                //ora aspetto che l'altro giocatore peschi la carta
                ricezione.waitPresaCarta();
                //ora pesco io la carta
                pescaCarta(interfaccia.scelta);
                //dopo aver pescato le carte inizio la prossima mano come secondo giocatore
                giocataSecondo();
            }
        }

        public void inviaMazzo()
        {
            mazzo = new Mazzo();
            mazzo.randomizzaMazzo();
            invio.inviaMazzo(mazzo.sincronizzato);
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
            else if (carteTavolo[0].seme == carteTavolo[1].seme && briscola.seme != carteTavolo[0].seme)
            {
                if (carteTavolo[0].valore > carteTavolo[1].valore)
                    invio = "w;";
                else if (carteTavolo[0].valore < carteTavolo[1].valore)
                    invio = "l;";
            }
            //in questo caso le due carte sono di semi diversi, vince il primo giocatore
            else if (carteTavolo[0].seme != carteTavolo[1].seme)
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
                interfaccia.invisibileTutto();
                interfaccia.visualizzaRisultato(1);
            }
            else if (temp < 60)
            {
                interfaccia.invisibileTutto();
                interfaccia.visualizzaRisultato(-1);
            }
            else
            {
                interfaccia.invisibileTutto();
                interfaccia.visualizzaRisultato(0);
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
        public void concludiPartita()
        {

            calcoloPunti();
            ricezione.chiudiConnessione();

        }
        public void mostraEsito(bool check)
        {
            //se ho vinto  check=true
            //se ho perso  check=false
            Thread t = new Thread(new ThreadStart(() =>
            {
                if (check)
                    interfaccia.mostraEsito(1);
                else
                    interfaccia.mostraEsito(0);
                Thread.Sleep(2000);
                    interfaccia.mostraEsito(-1);
            }));
            t.Start();
        }
    }

}
