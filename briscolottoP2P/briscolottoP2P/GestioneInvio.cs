using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace briscolottoP2P
{
    public class GestioneInvio
    {
        GestioneBriscola gestioneBriscola;
        UdpClient client;
        int portaInvio;

        static GestioneInvio _instance = null;
        static public GestioneInvio getInstance()
        {
            if (_instance == null)
                _instance = new GestioneInvio();
            return _instance;
        }
        private GestioneInvio()
        {
            client = new UdpClient();
            portaInvio = 12345;
        }
        public void caricaGestione()
        {
            gestioneBriscola = GestioneBriscola.getInstance(null);
        }
        public void invioGenerico(string ip, string invio)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(invio);
            client.Send(buffer, buffer.Length, ip, portaInvio);
        }
        public void richiediConnessione(string ip)
        {
            string invio = "a;" + gestioneBriscola.nomeLocal + ";";
            byte[] buffer = Encoding.ASCII.GetBytes(invio);
            client.Send(buffer, buffer.Length, ip, portaInvio);

            //creo un timer con il quale il destinatario deve rispondere entro 20 secondi alla richiesta, se non lo fa la richiesta viene automaticamente annullata
            Thread t = new Thread(new ThreadStart(() =>
            {
                int i = 0;
                while (i < 21)
                {
                    Thread.Sleep(200);
                    if (gestioneBriscola.statoConnessione > 1)
                        break;
                    else if (gestioneBriscola.statoConnessione == 0)
                        break;
                    else if (i == 20 && gestioneBriscola.statoConnessione == 1)
                    {
                        gestioneBriscola.interfaccia.annullaRichiesta();
                        break;
                    }
                    i++;
                }
            }));
            t.IsBackground = true;
            t.Start();
        }
        public void inviaMazzo(List<Carta> mazzo)
        {
            string invio = "m;";
            for (int i = 0; i < mazzo.Count; i++)
            {
                invio += mazzo[i].valore + "," + mazzo[i].seme + ";";
            }
            byte[] buffer = Encoding.ASCII.GetBytes(invio);
            client.Send(buffer, buffer.Length, gestioneBriscola.ipDestinatario, portaInvio);
        }
        public void inviaCartaGiocata(Carta carta)
        {
            string invio = "b;" + carta.valore + ";" + carta.seme + ";";
            byte[] buffer = Encoding.ASCII.GetBytes(invio);
            client.Send(buffer, buffer.Length, gestioneBriscola.ipDestinatario, portaInvio);
        }
    }
}
