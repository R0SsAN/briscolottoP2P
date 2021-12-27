using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        
        public void Invisibile()
        {
            //rendiamo tutte le carte da gioco invisibili per la fase di connessione
            if (!CheckAccess())
                Dispatcher.Invoke(() => { Invisibile(); });
            else
            {
                mia1.Visibility = Visibility.Hidden;
                mia2.Visibility = Visibility.Hidden;
                mia3.Visibility = Visibility.Hidden;
                tavolo1.Visibility = Visibility.Hidden;
                tavolo2.Visibility = Visibility.Hidden;
                avv1.Visibility = Visibility.Hidden;
                avv2.Visibility = Visibility.Hidden;
                avv3.Visibility = Visibility.Hidden;
                mazzo.Visibility = Visibility.Hidden;
                briscola.Visibility = Visibility.Hidden;
                background.Background = (Brush)new BrushConverter().ConvertFrom("#FF7BE87B");
            }
           
        }
        public void visibile()
        {
            //rendiamo tutte le carte da gioco visibili quando inizia la partita
            if (!CheckAccess())
                Dispatcher.Invoke(() => { visibile(); });
            else
            {
                mia1.Visibility = Visibility.Visible;
                mia2.Visibility = Visibility.Visible;
                mia3.Visibility = Visibility.Visible;
                tavolo1.Visibility = Visibility.Visible;
                tavolo2.Visibility = Visibility.Visible;
                avv1.Visibility = Visibility.Visible;
                avv2.Visibility = Visibility.Visible;
                avv3.Visibility = Visibility.Visible;
                mazzo.Visibility = Visibility.Visible;
                briscola.Visibility = Visibility.Visible;
                background.Background = (Brush)new BrushConverter().ConvertFrom("#FF035703");
                lName.Visibility = Visibility.Hidden;
                lAvv.Visibility = Visibility.Hidden;
                btnName.Visibility = Visibility.Hidden;
                btnRischiesta.Visibility = Visibility.Hidden;
            }
        }

        private void btnName_Click(object sender, RoutedEventArgs e)
        {
            //aggiorno il nome del giocatore
            if (lName.Text != "")
                gestione.nomeLocal = lName.Text;                          
        }

        private void btnRischiesta_Click(object sender, RoutedEventArgs e)
        {
            //se l'ip è valido invia una rischiesta di connesione a quel indirizzo ip
            IPAddress temp;
            if (IPAddress.TryParse(lAvv.Text,out temp ))
            {
                gestione.ipDestinatario = lAvv.Text;
                invio.richiediConnessione(lAvv.Text);
                gestione.statoConnessione = 1;
            }
                
        }

        public void visualizzaRisultato(int a)
        {
            // usiamo la variabile a per determinare il risultato della partita 
            // 1 --> vinto
            // 0 --> pareggio
            // -1 --> perso
            if (!CheckAccess())
                Dispatcher.Invoke(() => { visualizzaRisultato(a); });
            else
            {
                if (a == 1)
                {
                    imgRisultato.Source = new BitmapImage(new Uri(""));
                    imgRisultato.Visibility = Visibility.Visible;
                }
                else if(a==0)
                {
                    imgRisultato.Source = new BitmapImage(new Uri(""));
                    imgRisultato.Visibility = Visibility.Visible;
                }
                else
                {
                    imgRisultato.Source = new BitmapImage(new Uri(""));
                    imgRisultato.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
