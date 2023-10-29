
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
        }

        public class Buttons
        {
            public string About { get; set; }
            public string Import { get; set; }
            public string Launch { get; set; }
            public string Stop { get; set; }
        }

        public class Textbox
        {
            public string Name { get; set; }
        }

        public class Label
        {
            public string Characters { get; set; }
            public string Time { get; set; }
            public string InfoPrev { get; set; }
            public string InfoTypeArea { get; set; }
        }

        public class Alert
        {
            public string FileMissing { get; set; }
            public string NameMissing { get; set; }
            public string FileLoaded { get; set; }
            public string TimerStarted { get; set; }
            public string GameOver { get; set; }
            public string GameAlreadyStarted { get; set; }
        }

        public class Error
        {
            public string FileUnreadable { get; set; }
            public string UnknownError { get; set; }
            public string SaveError { get; set; }
        }

        public class Table_Headers
        {
            public string Names { get; set; }
            public string Speeds { get; set; }
            public string Scores { get; set; }
            public string Faults { get; set; }
            public string Times { get; set; }
        }
 

    }
}
