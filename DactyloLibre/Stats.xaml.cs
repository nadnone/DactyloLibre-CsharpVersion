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
            public string name { get; set; }
            public double score { get; set; }
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

            GridView gridview = new GridView();
            listview.View = gridview;

            gridview.Columns.Add(new GridViewColumn
            {
                Header = "Nom",
                DisplayMemberBinding = new Binding("nom")
            });
            gridview.Columns.Add(new GridViewColumn
            {
                Header = "Score [c/s]",
                DisplayMemberBinding = new Binding("score")
            });
            gridview.Columns.Add(new GridViewColumn
            {
                Header = "Fautes",
                DisplayMemberBinding = new Binding("fautes")
            });
            gridview.Columns.Add(new GridViewColumn
            {
                Header = "Temps",
                DisplayMemberBinding = new Binding("temps")
            });



            if (listStats.Count < 1)
            {
                MessageBox.Show("Fichier vide ou illisible");
            }
            else
            {
                for (int i = 0; i < listStats.Count; i++)
                {
                    List<string> list = listStats[i];

                    listview.Items.Add(new itemForList
                    {
                        name = list[0],
                        score = (double)(Convert.ToInt32(list[1]) / Convert.ToInt32(list[3])),
                        fautes = Convert.ToInt32(list[2]),
                        temps = Convert.ToInt32(list[3])
                    });


                }
            }

        }
    }
}
