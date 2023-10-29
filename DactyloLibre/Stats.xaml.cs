using System;
using System.Collections.Generic;
using System.Windows;

using DactyloLibre.classes;

namespace DactyloLibre
{
    /// <summary>
    /// Logique d'interaction pour Stats.xaml
    /// </summary>
    public partial class Stats : Window
    {
        private Lang_parser langparser = new Lang_parser();
        public class ItemForList
        {
            public string Noms { get; set; }
            public decimal Vitesse { get; set; }
            public int Scores { get; set; }
            public int Fautes { get; set; }
            public string Temps { get; set; }
        }

        public Stats()
        {
            InitializeComponent();
            NamesHeader.Header = langparser.Finder().Table_headers.Names;
            speedsHeader.Header = langparser.Finder().Table_headers.Speeds;
            scoresHeader.Header = langparser.Finder().Table_headers.Scores;
            faultsHeader.Header = langparser.Finder().Table_headers.Faults;
            timesHeader.Header = langparser.Finder().Table_headers.Times;
        }

        private void Loadedpage(object sender, RoutedEventArgs e)
        {
            ResultatTreatment rslt = new ResultatTreatment();
            List<List<string>> listStats = rslt.ReadStats();


            if (listStats.Count < 1)
            {
                MessageBox.Show(langparser.Finder().Error.FileUnreadable);
            }
            else
            {
                List<ItemForList> itemList = new List<ItemForList> { };

                for (int i = listStats.Count-1; i >= 0;  i--)
                {
                    List<string> list = listStats[i];

                    long timeDelta = long.Parse(list[4]) - long.Parse(list[3]);

                    itemList.Add(new ItemForList
                    {
                        Noms = list[0],
                        Scores = Convert.ToInt32(list[1]),
                        Vitesse = (decimal) Math.Round(Convert.ToDecimal(list[1]) / (timeDelta/1000 == 0 ? 1 : timeDelta/1000), 3),
                        Fautes = Convert.ToInt32(list[2]),
                        Temps = timeDelta / 60000 + "min " + timeDelta / 1000 + "s " + timeDelta % 1000 + "ms"
                    });
                    
                }
                statsDataGrid.ItemsSource = itemList;
            }

        }
    }
}
