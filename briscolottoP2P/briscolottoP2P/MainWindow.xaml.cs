using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            gestione = GestioneBriscola.getInstance(this);
            invio = GestioneInvio.getInstance();
            ricezione = GestioneRicezione.getInstance();
            invio.caricaGestione();
            ricezione.caricaGestione();

            //avvio thread ricezione
            ricezione.startaThread();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            gestione.ipDestinatario = text.Text;
            invio.richiediConnessione(gestione.ipDestinatario);
            gestione.statoConnessione = 1;
        }
    }
}
