using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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

        //variabile utilizzata per capire qualche carta è stata scelta
        public int scelta;
        public bool puoiPrendere;

        public MainWindow()
        {
            InitializeComponent();
            gestione = GestioneBriscola.getInstance(this);
            invio = GestioneInvio.getInstance();
            ricezione = GestioneRicezione.getInstance();
            invio.caricaGestione();
            ricezione.caricaGestione();
            scelta = -1;
            puoiPrendere = false;
            Invisibile();

            //avvio thread ricezione
            ricezione.startaThread();
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
                mazzo.Visibility = Visibility.Hidden;
                briscola.Visibility = Visibility.Hidden;
                nick.Visibility = Visibility.Visible;
                ip.Visibility = Visibility.Visible;
                lName.Visibility = Visibility.Visible;
                lAvv.Visibility = Visibility.Visible;
                btnName.Visibility = Visibility.Visible;
                btnRischiesta.Visibility = Visibility.Visible;
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
                mazzo.Visibility = Visibility.Visible;
                briscola.Visibility = Visibility.Visible;
                background.Background = (Brush)new BrushConverter().ConvertFrom("#FF035703");
                lName.Visibility = Visibility.Hidden;
                lAvv.Visibility = Visibility.Hidden;
                btnName.Visibility = Visibility.Hidden;
                btnRischiesta.Visibility = Visibility.Hidden;
                nick.Visibility = Visibility.Hidden;
                ip.Visibility = Visibility.Hidden;
            }
        }
        public void invisibileTutto()
        {
            if (!CheckAccess())
                Dispatcher.Invoke(() => { invisibileTutto(); });
            else
            {
                mia1.Visibility = Visibility.Hidden;
                mia2.Visibility = Visibility.Hidden;
                mia3.Visibility = Visibility.Hidden;
                tavolo1.Visibility = Visibility.Hidden;
                tavolo2.Visibility = Visibility.Hidden;
                mazzo.Visibility = Visibility.Hidden;
                briscola.Visibility = Visibility.Hidden;
                lName.Visibility = Visibility.Hidden;
                lAvv.Visibility = Visibility.Hidden;
                btnName.Visibility = Visibility.Hidden;
                btnRischiesta.Visibility = Visibility.Hidden;
                nick.Visibility = Visibility.Hidden;
                ip.Visibility = Visibility.Hidden;
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
            if (gestione.statoConnessione == 0)
            {
                //se l'ip è valido invia una rischiesta di connesione a quel indirizzo ip
                IPAddress temp;
                if (IPAddress.TryParse(lAvv.Text, out temp))
                {
                    gestione.ipDestinatario = lAvv.Text;
                    invio.richiediConnessione(lAvv.Text);
                    gestione.statoConnessione = 1;

                    lName.IsEnabled = false;
                    btnName.IsEnabled = false;
                    lAvv.IsEnabled = false;
                    btnRischiesta.Content = "Annulla richiesta";
                }
            }
            //se invece cè già una richista incorso annullo la richiesta
            else
                annullaRichiesta();

        }
        public void annullaRichiesta()
        {
            if (!CheckAccess())
                Dispatcher.Invoke(() => { annullaRichiesta(); });
            else
            {
                gestione.statoConnessione = 0;
                lName.IsEnabled = true;
                btnName.IsEnabled = true;
                lAvv.IsEnabled = true;
                lAvv.Text = "";
                btnRischiesta.Content = "Invia richiesta";
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
                    imgRisultato.Source = new BitmapImage(new Uri("https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/risultato/youwin.png"));
                    imgRisultato.Visibility = Visibility.Visible;
                }
                else if (a == 0)
                {
                    imgRisultato.Source = new BitmapImage(new Uri("https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/risultato/youdraw.png"));
                    imgRisultato.Visibility = Visibility.Visible;
                }
                else
                {
                    imgRisultato.Source = new BitmapImage(new Uri("https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/risultato/youlose.png"));
                    imgRisultato.Visibility = Visibility.Visible;
                }
                bNuova.Visibility = Visibility.Visible;
            }
        }

        private void mia1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (puoiPrendere)
                scelta = 0;
        }
        private void mia2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (puoiPrendere)
                scelta = 1;
        }
        private void mia3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (puoiPrendere)
                scelta = 2;
        }
        public void aggiornaCarte()
        {
            if (!CheckAccess())
                Dispatcher.Invoke(() => { aggiornaCarte(); });
            else
            {
                Thread.Sleep(300);
                try
                {
                    mia1.Visibility = Visibility.Visible;
                    mia1.Source = new BitmapImage(new Uri(gestione.carteMano[0].img));
                }
                catch (Exception e)
                {
                    mia1.Visibility = Visibility.Hidden;
                }
                try
                {
                    mia2.Visibility = Visibility.Visible;
                    mia2.Source = new BitmapImage(new Uri(gestione.carteMano[1].img));
                }
                catch (Exception e)
                {
                    mia2.Visibility = Visibility.Hidden;
                }
                try
                {
                    mia3.Visibility = Visibility.Visible;
                    mia3.Source = new BitmapImage(new Uri(gestione.carteMano[2].img));
                }
                catch (Exception e)
                {
                    mia3.Visibility = Visibility.Hidden;
                }
                try
                {
                    tavolo1.Visibility = Visibility.Visible;
                    tavolo1.Source = new BitmapImage(new Uri(gestione.carteTavolo[0].img));
                }
                catch (Exception e)
                {
                    tavolo1.Visibility = Visibility.Hidden;
                }
                try
                {
                    tavolo2.Visibility = Visibility.Visible;
                    tavolo2.Source = new BitmapImage(new Uri(gestione.carteTavolo[1].img));
                }
                catch (Exception e)
                {
                    tavolo2.Visibility = Visibility.Hidden;
                }
                try
                {
                    briscola.Visibility = Visibility.Visible;
                    briscola.Source = new BitmapImage(new Uri(gestione.mazzo.sincronizzato[gestione.mazzo.sincronizzato.Count - 1].img));
                }
                catch (Exception e)
                {
                    briscola.Visibility = Visibility.Hidden;
                }
                if (gestione.mazzo.sincronizzato.Count < 1)
                {
                    mazzo.Visibility = Visibility.Hidden;
                }
            }
        }
        public void visualizzaTurno(bool check)
        {
            if (!CheckAccess())
                Dispatcher.Invoke(() => { visualizzaTurno(check); });
            else
            {
                if (check)
                    lTurno.Visibility = Visibility.Visible;
                else
                    lTurno.Visibility = Visibility.Hidden;
            }
        }
        private void bNuova_Click(object sender, RoutedEventArgs e)
        {
            Invisibile();
            //resetto la ricezione in modo da creare un nuovo thread per la nuova partita
            ricezione = new GestioneRicezione();
            ricezione.startaThread();
            //resetto anche la gestione partita
            gestione.carteTavolo = new List<Carta>();
            gestione.carteVinte = new List<Carta>();
            gestione.carteMano = new Carta[3];
            Invisibile();
            imgRisultato.Visibility = Visibility.Hidden;
            annullaRichiesta();
            bNuova.Visibility = Visibility.Hidden;
        }
        public void animazionePesca(int n)
        {
            if (!CheckAccess())
                Dispatcher.Invoke(() => { animazionePesca(n); });
            else
            {
                double newLeft = 0;
                double newTop = 447;
                if (n == 0)
                    newLeft = 441;
                else if (n == 1)
                    newLeft = 619;
                else if (n == 2)
                    newLeft = 799;

                imgAnimazione.Visibility = Visibility.Visible;
                Random rnd = new Random();
                //Create the animations for left and top
                DoubleAnimation animLeft = new DoubleAnimation(Canvas.GetLeft(imgAnimazione), newLeft, new Duration(TimeSpan.FromSeconds(1)));
                DoubleAnimation animTop = new DoubleAnimation(Canvas.GetTop(imgAnimazione), newTop, new Duration(TimeSpan.FromSeconds(1)));

                //Set an easing function so the button will quickly move away, then slow down
                //as it reaches its destination.
                animLeft.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
                animTop.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };

                //Start the animation.
                imgAnimazione.BeginAnimation(Canvas.LeftProperty, animLeft, HandoffBehavior.SnapshotAndReplace);
                imgAnimazione.BeginAnimation(Canvas.TopProperty, animTop, HandoffBehavior.SnapshotAndReplace);
            }
        }
        public void animazioneButta(string path)
        {
            if (!CheckAccess())
                Dispatcher.Invoke(() => { animazioneButta(path); });
            else
            {
                //imposto posizione finale dell'animazione
                double newLeft = 0;
                double newTop = 161;
                if (gestione.carteTavolo.Count == 0)
                    newLeft = 518;
                else if (gestione.carteTavolo.Count == 1)
                    newLeft = 709;
                //imposto posizione iniziale dell'animazione
                Canvas.SetTop(imgAnimazione, 447);
                if (scelta == 0)
                {
                    mia1.Source = null;
                    Canvas.SetLeft(imgAnimazione, 441);
                }
                else if (scelta == 1)
                {
                    mia2.Source = null;
                    Canvas.SetLeft(imgAnimazione, 619);
                }
                else if (scelta == 2)
                {
                    mia3.Source = null;
                    Canvas.SetLeft(imgAnimazione, 799);
                }


                imgAnimazione.Source = new BitmapImage(new Uri(path));
                imgAnimazione.Visibility = Visibility.Visible;
                Random rnd = new Random();
                //Create the animations for left and top
                DoubleAnimation animLeft = new DoubleAnimation(Canvas.GetLeft(imgAnimazione), newLeft, new Duration(TimeSpan.FromSeconds(1)));
                DoubleAnimation animTop = new DoubleAnimation(Canvas.GetTop(imgAnimazione), newTop, new Duration(TimeSpan.FromSeconds(1)));

                //Set an easing function so the button will quickly move away, then slow down
                //as it reaches its destination.
                animLeft.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
                animTop.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };

                //Start the animation.
                imgAnimazione.BeginAnimation(Canvas.LeftProperty, animLeft, HandoffBehavior.SnapshotAndReplace);
                imgAnimazione.BeginAnimation(Canvas.TopProperty, animTop, HandoffBehavior.SnapshotAndReplace);
            }
        }
        public void terminaAnimazione()
        {
            //questo metodo permette di terminare l'animazione in modo da poter poi modificare e spostare l'oggetto precedentemente animato
            if (!CheckAccess())
                Dispatcher.Invoke(() => { terminaAnimazione(); });
            else
            {
                aggiornaCarte();
                imgAnimazione.Visibility = Visibility.Hidden;
                imgAnimazione.BeginAnimation(Canvas.LeftProperty, null);
                imgAnimazione.BeginAnimation(Canvas.TopProperty, null);
                imgAnimazione.Source = new BitmapImage(new Uri("https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/back.gif"));
                Canvas.SetLeft(imgAnimazione, 39);
                Canvas.SetTop(imgAnimazione, 240);
            }

        }
    }
}
