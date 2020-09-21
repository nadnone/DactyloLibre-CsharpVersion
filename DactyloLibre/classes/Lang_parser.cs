using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IniParser;
using IniParser.Model;

namespace DactyloLibre.classes
{
    class Lang_parser
    {
        private string langPath = ".\\language_config.ini";

        class LangData
        {
            
        }
        public IniData finder()
        {
            if (!File.Exists(langPath)) MessageBox.Show("language files are missing, please reinstall the software correctly");

            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(langPath, Encoding.UTF8);
            string langDataPath = data["config"]["LanguageFile"];

           data = parser.ReadFile(".\\"+langDataPath);

            return data;
        }
    }
}
