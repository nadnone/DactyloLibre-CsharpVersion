using System.Collections.Generic;
using System.IO;


namespace DactyloLibre
{
    class ResultatTreatment
    {
        private const string PATH = ".\\stats.txt";

        public List<List<string>> ReadStats()
        {
            if (!File.Exists(PATH)) return new List<List<string>> { };

            StreamReader sr = new StreamReader(PATH);

            List <List<string>> listStats = new List<List<string>> { };
            List<string> listValues = new List<string> { };
            string read;

            while ((read = sr.ReadLine()) != null)
            {

                string[] values = read.Split(',');
                if (values.Length < 5) return new List<List<string>> { };

                for (int i = 0; i < 5; i++)
                {
                    listValues.Add(values[i]);
                }


                listStats.Add(listValues);
                listValues = new List<string> { };

            }
            return listStats;
        }

        public bool AppendStats(string name, int c, int fault, long timeS, long timeF)
        {

            name = name.Replace(",", ".");

            string text = name + "," + c + "," + fault + "," + timeS + "," + timeF;

            try
            {
                File.AppendAllText(PATH, text+"\n");
            }
            catch (IOException)
            {
                return true;
            }

            return false;
        }

    }
}
