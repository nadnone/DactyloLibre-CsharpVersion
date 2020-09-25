using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IniParser;
using IniParser.Model;
using DactyloLibre.classes;

namespace DactyloLibre
{
    /// <summary>
    /// Logique d'interaction pour Stats.xaml
    /// </summary>
    public partial class Stats : Window
    {
        private Lang_parser langparser = new Lang_parser();
        public class itemForList
        {
            public string noms { get; set; }
            public decimal vitesse { get; set; }
            public int scores { get; set; }
            public int fautes { get; set; }
            public string temps { get; set; }
        }

        public Stats()
        {
            InitializeComponent();
            NamesHeader.Header = langparser.finder().Table_headers.names;
            speedsHeader.Header = langparser.finder().Table_headers.speeds;
            scoresHeader.Header = langparser.finder().Table_headers.scores;
            faultsHeader.Header = langparser.finder().Table_headers.faults;
            timesHeader.Header = langparser.finder().Table_headers.times;
        }

        private void loadedpage(object sender, RoutedEventArgs e)
        {
            ResultatTreatment rslt = new ResultatTreatment();
            List<List<string>> listStats = rslt.readStats();


            if (listStats.Count < 1)
            {
                MessageBox.Show(langparser.finder().Error.FileUnreadable);
            }
            else
            {
                List<itemForList> itemList = new List<itemForList> { };

                for (int i = listStats.Count-1; i >= 0;  i--)
                {
                    List<string> list = listStats[i];

                    long timeDelta = long.Parse(list[4]) - long.Parse(list[3]);

                    itemList.Add(new itemForList
                    {
                        noms = list[0],
                        scores = Convert.ToInt32(list[1]),
                        vitesse = (decimal)(Convert.ToDecimal(list[1]) / (timeDelta/1000 == 0 ? 1 : timeDelta/1000)),
                        fautes = Convert.ToInt32(list[2]),
                        temps = timeDelta / 60000 + "min " + timeDelta / 1000 + "s " + timeDelta % 1000 + "ms"
                    });
                    
                }
                statsDataGrid.ItemsSource = itemList;
            }

        }
    }
}
