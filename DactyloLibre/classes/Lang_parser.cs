using System.IO;
using System.Text;
using System.Windows;
using IniParser;
using IniParser.Model;
using Newtonsoft.Json;
using DactyloLibre.Models;
using System;

namespace DactyloLibre.classes
{
    class Lang_parser
    {
        private const string LANGPATH = ".\\language_config.ini";

        private void Error_quit(string text)
        {
            MessageBox.Show(text);
            Environment.Exit(1);
        }
        public dynamic Finder()
        {
            if (!File.Exists(LANGPATH)) Error_quit("language files are missing, please reinstall the software correctly");
         
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(LANGPATH, Encoding.UTF8);
            string langDataPath = data["config"]["LanguageFile"];

            if (!File.Exists(langDataPath)) Error_quit("language files are missing, please reinstall the software correctly");

            StreamReader sr = new StreamReader(langDataPath);
            string jsonText = sr.ReadToEnd();

            return JsonConvert.DeserializeObject<LangParserObjects.Rootobject>(jsonText);
        }
    }
}
