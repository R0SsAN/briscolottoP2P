using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace briscolottoP2P
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GestioneBriscola gestione;
        GestioneRicezione ricezione;
        GestioneInvio invio;
        public MainWindow()
        {
            InitializeComponent();
            gestione = new GestioneBriscola(this);
            ricezione = new GestioneRicezione();
            invio = new GestioneInvio();

            //avvio thread ricezione
            ricezione.startaThread();
        }
    }
}
