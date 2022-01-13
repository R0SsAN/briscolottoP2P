using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace briscolottoP2P
{
    public class GestioneRicezione
    {
        GestioneBriscola gestioneBriscola;
        GestioneInvio invio;
        UdpClient server;
        IPEndPoint endpoint;

        static GestioneRicezione _instance = null;
        static public GestioneRicezione getInstance()
        {
            if (_instance == null)
                _instance = new GestioneRicezione();
            return _instance;
        }
        public GestioneRicezione()
        {
            invio = GestioneInvio.getInstance();
            server = new UdpClient(12345);
            endpoint = new IPEndPoint(IPAddress.Any, 0);
        }
        public void caricaGestione()
        {
            gestioneBriscola = GestioneBriscola.getInstance(null);
        }
        public void startaThread()
        {
            Thread t = new Thread(new ThreadStart(() => threadRicezione()));
            t.IsBackground = true;
            t.Start();
        }
        public void threadRicezione()
        {
            while (true)
            {
                if (server == null)
                    break;
                byte[] ricezione = server.Receive(ref endpoint);
                string[] split = Encoding.ASCII.GetString(ricezione).Split(';');
                char scelta = split[0].ElementAt(0);
                switch (scelta)
                {
                    case 'a':
                        {
                            //in questo caso ricevo da un altro peer la richiesta di connessione
                            if (gestioneBriscola.statoConnessione == 0)
                            {
                                //visualizzo una messabox in cui chiedo se accettare o meno la richiesta
                                MessageBoxResult result = MessageBox.Show("Richiesta connessione da: " + split[1] + ". \n Vuoi accettarla?", "Nuova richiesta connessione", MessageBoxButton.YesNo);
                                switch (result)
                                {
                                    case MessageBoxResult.Yes:
                                        //l'utente ha accettato la connessione quindi invio risposta al mittente
                                        invio.invioGenerico(endpoint.Address.ToString(), "y;" + gestioneBriscola.nomeLocal + ";");
                                        gestioneBriscola.ipDestinatario = endpoint.Address.ToString();
                                        gestioneBriscola.statoConnessione = 2;
                                        break;
                                    case MessageBoxResult.No:
                                        //l'utente non ha accettato la connessione quindi invio risposta negativa
                                        invio.invioGenerico(endpoint.Address.ToString(), "n;");
                                        gestioneBriscola.interfaccia.annullaRichiesta();
                                        break;
                                }
                            }
                            else
                            {
                                invio.invioGenerico(endpoint.Address.ToString(), "n;");
                            }
                        }
                        break;

                    case 'y':
                        {
                            //in questo caso ci potrebbero essere due situazioni:
                            //  nel primo caso stò aspettando una risposta dal destinatario
                            if (gestioneBriscola.statoConnessione == 1 && gestioneBriscola.ipDestinatario == endpoint.Address.ToString())
                            {
                                //ho ricevuto una risposta positiva dal destinario, quindi mi salvo il suo nome e avvio la connessione
                                gestioneBriscola.nomeRemote = split[1];
                                invio.invioGenerico(gestioneBriscola.ipDestinatario, "y;");
                                gestioneBriscola.statoConnessione = 3;
                                //TODO : implementazione inizio partita
                                gestioneBriscola.avvioPartitaMazziere();
                                Console.WriteLine("gioca");
                            }
                            //  nel secondo caso sono invece il destinatario che stà aspettando la seconda conferma dal mittente
                            else if (gestioneBriscola.statoConnessione == 2 && gestioneBriscola.ipDestinatario == endpoint.Address.ToString())
                            {
                                gestioneBriscola.statoConnessione = 3;
                                gestioneBriscola.avvioPartitaGiocatore();
                                Console.WriteLine("gioca");
                            }
                            else
                            {
                                invio.invioGenerico(endpoint.Address.ToString(), "n;");
                            }
                        }
                        break;

                    case 'n':
                        {
                            //se ricevo n; in ogni caso devo ripristinare la connessione
                            gestioneBriscola.interfaccia.annullaRichiesta();
                        }
                        break;
                }
            }
        }
        public void waitConfermaMazzo()
        {
            //aspetto la conferma di ricezione mazzo da parte del destinatario
            string stringa;
            do
            {
                byte[] ricezione = server.Receive(ref endpoint);
                stringa = Encoding.ASCII.GetString(ricezione);

                if (gestioneBriscola.ipDestinatario != endpoint.Address.ToString())
                    invio.invioGenerico(endpoint.Address.ToString(), "n;");
            }
            while (stringa != "m;y;" && gestioneBriscola.ipDestinatario != endpoint.Address.ToString());
        }
        public List<Carta> riceviMazzo()
        {
            //aspetto la ricezione del mazzo da parte del mazziere
            string[] split;
            do
            {
                byte[] ricezione = server.Receive(ref endpoint);
                split = Encoding.ASCII.GetString(ricezione).Split(';');

                if (gestioneBriscola.ipDestinatario != endpoint.Address.ToString())
                    invio.invioGenerico(endpoint.Address.ToString(), "n;");
            }
            while (split.Length != 42 || gestioneBriscola.ipDestinatario != endpoint.Address.ToString());
            //ora che ho ricevuto il mazzo lo formatto in una lista di carte
            List<Carta> carte = new List<Carta>();
            for (int i = 1; i < split.Length - 1; i++)
            {
                string[] temp = split[i].Split(',');
                carte.Add(new Carta(temp[1], 0, "", Convert.ToInt32(temp[0])));
            }
            return carte;
        }
        public void waitPresaCarta()
        {
            //aspetto la conferma che l'altro giocatore ha preso una carta dal mazzo, quando la ricevo la rimuovo anche io in modo da aggiornare il mazzo
            string stringa;
            do
            {
                byte[] ricezione = server.Receive(ref endpoint);
                stringa = Encoding.ASCII.GetString(ricezione);

                if (gestioneBriscola.ipDestinatario != endpoint.Address.ToString())
                    invio.invioGenerico(endpoint.Address.ToString(), "n;");
            }
            while (stringa != "p;" && gestioneBriscola.ipDestinatario != endpoint.Address.ToString());
            //metto un try catch in modo che alla fine della partita quando nno ci sono più carte nel mazzo non mi si interrompe il programma
            try
            {
                gestioneBriscola.mazzo.sincronizzato.RemoveAt(0);
            }
            catch (Exception e)
            { }

        }
        public Carta waitCartaGiocata()
        {
            //aspetto che l'altro giocatore mi mandi la carta
            Carta temp;
            string[] split;
            do
            {
                byte[] ricezione = server.Receive(ref endpoint);
                split = Encoding.ASCII.GetString(ricezione).Split(';');

                if (gestioneBriscola.ipDestinatario != endpoint.Address.ToString())
                    invio.invioGenerico(endpoint.Address.ToString(), "n;");
            }
            while (split[0] != "b;" && gestioneBriscola.ipDestinatario != endpoint.Address.ToString());
            string[] split2 = split[1].Split(',');
            temp = new Carta(split2[1], 0, "", Convert.ToInt32(split2[0]));
            return temp;
        }
        public bool waitEsitoGiocata()
        {
            string stringa;
            do
            {
                byte[] ricezione = server.Receive(ref endpoint);
                stringa = Encoding.ASCII.GetString(ricezione);

                if (gestioneBriscola.ipDestinatario != endpoint.Address.ToString())
                    invio.invioGenerico(endpoint.Address.ToString(), "n;");
            }
            while (stringa != "w;" && stringa != "l;" && gestioneBriscola.ipDestinatario != endpoint.Address.ToString());
            //se ha vinto allora ritorno true
            if (stringa == "w;")
                return true;
            //se ha perso ritorno false
            return false;
        }
        public void chiudiConnessione()
        {
            server.Close();
            server = null;
        }
    }
}
