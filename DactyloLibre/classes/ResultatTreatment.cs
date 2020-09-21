using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;


namespace DactyloLibre
{
    class ResultatTreatment
    {
        private string path = ".\\stats.txt";

        public List<List<string>> readStats()
        {
            if (!File.Exists(path)) return new List<List<string>> { };

            StreamReader sr = new StreamReader(path);

            List <List<string>> listStats = new List<List<string>> { };
            List<string> listValues = new List<string> { };
            
            string[] values = new string[5];
            string read = "";

            while ((read = sr.ReadLine()) != null)
            {
               
                values = read.Split(',');
                if (values.Length < 5) return new List<List<string>> { };

                listValues.Add(values[0]);
                listValues.Add(values[1]);
                listValues.Add(values[2]);
                listValues.Add(values[3]);
                listValues.Add(values[4]);

                listStats.Add(listValues);
                listValues = new List<string> { };

            }
            return listStats;
        }

        public bool appendStats(string name, int c, int fault, long timeS, long timeF)
        {

            name = name.Replace(",", ".");

            string text = name + "," + c + "," + fault + "," + timeS + "," + timeF;

            try
            {
                File.AppendAllText(path, text+"\n");
            }
            catch (IOException)
            {
                return true;
            }

            return false;
        }

    }
}
