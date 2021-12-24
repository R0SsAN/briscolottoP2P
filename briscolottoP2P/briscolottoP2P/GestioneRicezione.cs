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
        public GestioneRicezione()
        {
            gestioneBriscola = GestioneBriscola.getInstance(null);
            invio = GestioneInvio.getInstance();
            server = new UdpClient(12345);
            endpoint = new IPEndPoint(IPAddress.Any, 0);
        }
        public void startaThread()
        {
            Thread t = new Thread(new ThreadStart(() => threadRicezione()));
            t.Start();
        }
        public void threadRicezione()
        {
            while (true)
            {
                byte[] ricezione = server.Receive(ref endpoint);
                string[] split = Encoding.ASCII.GetString(ricezione).Split(';');
                char scelta = split[0].ElementAt(0);
                switch (scelta)
                { 
                    case 'a':
                        {
                            //in questo caso ricevo da un altro peer la richiesta di connessione
                            //visualizzo una messabox in cui chiedo se accettare o meno la richiesta
                            MessageBoxResult result = MessageBox.Show("Richiesta connessione da: " + split[1] + ". \n Vuoi accettarla?", "Nuova richiesta connessione", MessageBoxButton.YesNo);
                            switch (result)
                            {
                                case MessageBoxResult.Yes:
                                    //l'utente ha accettato la connessione quindi invio risposta al mittente
                                    invio.invioGenerico(endpoint.Address.ToString(), "y;" + gestioneBriscola.nomeLocal+";");
                                    gestioneBriscola.ipDestinatario = endpoint.Address.ToString();
                                    gestioneBriscola.statoConnessione = 3;
                                    break;
                                case MessageBoxResult.No:
                                    //l'utente non ha accettato la connessione quindi invio risposta negativa
                                    invio.invioGenerico(endpoint.Address.ToString(), "n;");
                                    gestioneBriscola.statoConnessione = 0;
                                    break;
                            }
                        }
                        break;

                    case 'y':
                        {
                            //in questo caso ci potrebbero essere due situazioni:
                            //  nel primo caso stò aspettando una risposta dal destinatario
                            if(gestioneBriscola.statoConnessione==1)
                            {
                                //ho ricevuto una risposta positiva dal destinario, quindi mi salvo il suo nome e avvio la connessione
                                gestioneBriscola.nomeRemote = split[1];
                                invio.invioGenerico(gestioneBriscola.ipDestinatario, "y;");
                                gestioneBriscola.statoConnessione = 3;
                                //TODO : implementazione inizio partita
                            }
                            //  nel secondo caso sono invece il destinatario che stà aspettando la seconda conferma dal mittente
                            else if(gestioneBriscola.statoConnessione==2)
                            {
                                gestioneBriscola.statoConnessione = 3;
                            }
                        }
                        break;

                    case 'n':
                        {
                            //se ricevo n; in ogni caso devo ripristinare la connessione
                            gestioneBriscola.statoConnessione = 0;
                        }
                        break;

                    case 'm':
                        {
                            if (gestioneBriscola.statoConnessione == 3)
                            {

                            }
                        }
                        break;

                    case 'p':

                        break;

                    case 'b':

                        break;

                    case 'w':

                        break;

                    case 'l':

                        break;

                    case 'f':

                        break;
                }
            }
        }
    }
}
