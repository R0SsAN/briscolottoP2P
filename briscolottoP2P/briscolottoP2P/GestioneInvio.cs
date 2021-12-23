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
        public GestioneInvio()
        {
            gestioneBriscola = GestioneBriscola.getInstance(null);
            client = new UdpClient();
        }
    }
}
