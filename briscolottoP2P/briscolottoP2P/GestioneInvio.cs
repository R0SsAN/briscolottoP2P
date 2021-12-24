using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace briscolottoP2P
{
    class GestioneInvio
    {
        GestioneBriscola gestioneBriscola;
        UdpClient client;

        static GestioneInvio _instance = null;
        static public GestioneInvio getInstance()
        {
            if (_instance == null)
                _instance = new GestioneInvio();
            return _instance;
        }
        public GestioneInvio()
        {
            gestioneBriscola = GestioneBriscola.getInstance(null);
            client = new UdpClient();
        }
        public void invioGenerico(string ip,string invio)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(invio);
            client.Send(buffer, buffer.Length, ip, 12345);
        }
        public void richiediConnessione(string ip)
        {
            string invio = "a;" + gestioneBriscola.nomeLocal + ";";
            byte[] buffer = Encoding.ASCII.GetBytes(invio);
            client.Send(buffer, buffer.Length, ip, 12345);
        }
    }
}
