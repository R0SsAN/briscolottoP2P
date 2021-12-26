﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace briscolottoP2P
{
    public class Mazzo
    {
        public List<Carta> mazzo;
        public Mazzo()
        {
            mazzo = new List<Carta>();
            caricaMazzo();
        }
        public void caricaMazzo()
        {
            Carta c;
            //carico carte mazzo
            #region caricaBastoni
            c = new Carta("b", 10, 4, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/bastoni/10b.gif");
            mazzo.Add(c);
            c = new Carta("b", 9, 3, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/bastoni/9b.gif");
            mazzo.Add(c);
            c = new Carta("b", 8, 2, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/bastoni/8b.gif");
            mazzo.Add(c);
            c = new Carta("b", 7, 0, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/bastoni/7b.gif");
            mazzo.Add(c);
            c = new Carta("b", 6, 0, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/bastoni/6b.gif");
            mazzo.Add(c);
            c = new Carta("b", 5, 0, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/bastoni/5b.gif");
            mazzo.Add(c);
            c = new Carta("b", 4, 0, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/bastoni/4b.gif");
            mazzo.Add(c);
            c = new Carta("b", 10.5f, 10, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/bastoni/3b.gif");
            mazzo.Add(c);
            c = new Carta("b", 2, 0, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/bastoni/2b.gif");
            mazzo.Add(c);
            c = new Carta("b", 11, 11, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/bastoni/1b.gif");
            mazzo.Add(c);
            #endregion

            #region caricaCoppe
            c = new Carta("c", 10, 4, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/coppe/10c.gif");
            mazzo.Add(c);
            c = new Carta("c", 9, 3, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/coppe/9c.gif");
            mazzo.Add(c);
            c = new Carta("c", 8, 2, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/coppe/8c.gif");
            mazzo.Add(c);
            c = new Carta("c", 7, 0, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/coppe/7c.gif");
            mazzo.Add(c);
            c = new Carta("c", 6, 0, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/coppe/6c.gif");
            mazzo.Add(c);
            c = new Carta("c", 5, 0, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/coppe/5c.gif");
            mazzo.Add(c);
            c = new Carta("c", 4, 0, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/coppe/4c.gif");
            mazzo.Add(c);
            c = new Carta("c", 10.5f, 10, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/coppe/3c.gif");
            mazzo.Add(c);
            c = new Carta("c", 2, 0, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/coppe/2c.gif");
            mazzo.Add(c);
            c = new Carta("c", 11, 11, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/coppe/1c.gif");
            mazzo.Add(c);
            #endregion

            #region caricaDenari
            c = new Carta("d", 10, 4, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/denari/10d.gif");
            mazzo.Add(c);
            c = new Carta("d", 9, 3, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/denari/9d.gif");
            mazzo.Add(c);
            c = new Carta("d", 8, 2, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/denari/8d.gif");
            mazzo.Add(c);
            c = new Carta("d", 7, 0, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/denari/7d.gif");
            mazzo.Add(c);
            c = new Carta("d", 6, 0, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/denari/6d.gif");
            mazzo.Add(c);
            c = new Carta("d", 5, 0, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/denari/5d.gif");
            mazzo.Add(c);
            c = new Carta("d", 4, 0, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/denari/4d.gif");
            mazzo.Add(c);
            c = new Carta("d", 10.5f, 10, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/denari/3d.gif");
            mazzo.Add(c);
            c = new Carta("d", 2, 0, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/denari/2d.gif");
            mazzo.Add(c);
            c = new Carta("d", 11, 11, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/denari/1d.gif");
            mazzo.Add(c);
            #endregion

            #region caricaSpade
            c = new Carta("s", 10, 4, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/spade/10s.gif");
            mazzo.Add(c);
            c = new Carta("s", 9, 3, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/spade/9s.gif");
            mazzo.Add(c);
            c = new Carta("s", 8, 2, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/spade/8s.gif");
            mazzo.Add(c);
            c = new Carta("s", 7, 0, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/spade/7s.gif");
            mazzo.Add(c);
            c = new Carta("s", 6, 0, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/spade/6s.gif");
            mazzo.Add(c);
            c = new Carta("s", 5, 0, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/spade/5s.gif");
            mazzo.Add(c);
            c = new Carta("s", 4, 0, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/spade/4s.gif");
            mazzo.Add(c);
            c = new Carta("s", 10.5f, 10, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/spade/3s.gif");
            mazzo.Add(c);
            c = new Carta("s", 2, 0, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/spade/2s.gif");
            mazzo.Add(c);
            c = new Carta("s", 11, 11, "https://raw.githubusercontent.com/R0SsAN/briscolottoP2P/main/img_carte/spade/1s.gif");
            mazzo.Add(c);
            #endregion
        }
        public void randomizzaMazzo()
        {
            Random rng = new Random();
            List<Carta> temp = mazzo.OrderBy(a => rng.Next()).ToList();
            mazzo = temp;
        }
        public void sincronizzaMazzo(List<Carta> temp)
        {
            //per ogni carta passata dal mazziere cerco nel mazzo locale la carta corrispondente
            //in modo da potergli inserire i valori mancanti
            for (int i = 0; i < temp.Count; i++)
            {
                for (int j = 0; j < mazzo.Count; j++)
                {
                    if(temp[i].seme==mazzo[j].seme && temp[i].valore == mazzo[j].valore)
                    {
                        temp[i].punteggio = mazzo[j].punteggio;
                        temp[i].img = mazzo[j].img;
                    }
                }
            }
            //ora aggiorno il mazzo locale sostituendolo a quello aggiornato
            this.mazzo = temp;
        }
    }
}
