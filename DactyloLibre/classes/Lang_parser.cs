using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IniParser;
using IniParser.Model;
using Newtonsoft.Json;
using DactyloLibre.Models;

namespace DactyloLibre.classes
{
    class Lang_parser
    {
        private string langPath = ".\\language_config.ini";

        public dynamic finder()
        {
            if (!File.Exists(langPath)) MessageBox.Show("language files are missing, please reinstall the software correctly");
         
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(langPath, Encoding.UTF8);
            string langDataPath = data["config"]["LanguageFile"];

            if (!File.Exists(langDataPath)) MessageBox.Show("language files are missing, please reinstall the software correctly");

            StreamReader sr = new StreamReader(langDataPath);
            string jsonText = sr.ReadToEnd();

            return JsonConvert.DeserializeObject<LangParserObjects.Rootobject>(jsonText);
        }
    }
}
