using System;
using System.Collections.Generic;
using System.Data;
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

namespace DactyloLibre
{
    /// <summary>
    /// Logique d'interaction pour Stats.xaml
    /// </summary>
    public partial class Stats : Window
    {
        public class itemForList
        {
            public string noms { get; set; }
            public double vitesse { get; set; }
            public int scores { get; set; }
            public int fautes { get; set; }
            public int temps { get; set; }
        }

        public Stats()
        {
            InitializeComponent();
        }

        private void loadedpage(object sender, RoutedEventArgs e)
        {
            ResultatTreatment rslt = new ResultatTreatment();
            List<List<string>> listStats = rslt.readStats();


            if (listStats.Count < 1)
            {
                MessageBox.Show("Fichier vide ou illisible");
            }
            else
            {
                List<itemForList> itemList = new List<itemForList> { };

                for (int i = 0; i < listStats.Count; i++)
                {
                    List<string> list = listStats[i];


                    itemList.Add(new itemForList{
                        noms = list[0],
                        scores = Convert.ToInt32(list[1]),
                        vitesse = (double)(Convert.ToInt32(list[1]) / (Convert.ToInt32(list[3]) <= 0 ? 1: Convert.ToInt32(list[3]))),
                        fautes = Convert.ToInt32(list[2]),
                        temps = Convert.ToInt32(list[3])
                    });

                }

                statsDataGrid.ItemsSource = itemList;
            }

        }
    }
}
