using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DactyloLibre.Models
{
    class LangParserObjects
    {

        public class Rootobject
        {
            public Buttons Buttons { get; set; }
            public Textbox Textbox { get; set; }
            public Label Label { get; set; }
            public Alert Alert { get; set; }
            public Error Error { get; set; }
            public Table_Headers Table_headers { get; set; }
            public Image Image { get; set; }
        }

        public class Buttons
        {
            public string about { get; set; }
            public string import { get; set; }
            public string launch { get; set; }
            public string stop { get; set; }
        }

        public class Textbox
        {
            public string name { get; set; }
        }

        public class Label
        {
            public string characters { get; set; }
            public string time { get; set; }
        }

        public class Alert
        {
            public string fileMissing { get; set; }
            public string nameMissing { get; set; }
            public string fileLoaded { get; set; }
            public string TimerStarted { get; set; }
            public string gameOver { get; set; }
            public string gameAlreadyStarted { get; set; }
        }

        public class Error
        {
            public string FileUnreadable { get; set; }
            public string idontKnowButError { get; set; }
            public string saveError { get; set; }
        }

        public class Table_Headers
        {
            public string names { get; set; }
            public string speeds { get; set; }
            public string scores { get; set; }
            public string faults { get; set; }
            public string times { get; set; }
        }

        public class Image
        {
            public string ImgInstructionName { get; set; }
            public string _comment { get; set; }
        }

    }
}
