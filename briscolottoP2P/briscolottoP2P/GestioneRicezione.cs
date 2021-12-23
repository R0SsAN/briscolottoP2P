using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace briscolottoP2P
{
    class GestioneRicezione
    {
        GestioneBriscola gestioneBriscola;
        UdpClient server;
        IPEndPoint endpoint;
        public GestioneRicezione()
        {
            gestioneBriscola = GestioneBriscola.getInstance(null);
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

                        break;

                    case 'y':

                        break;

                    case 'n':

                        break;

                    case 'm':

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
