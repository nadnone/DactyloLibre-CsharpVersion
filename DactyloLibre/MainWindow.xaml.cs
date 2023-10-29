using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using System.Windows.Threading;
using DactyloLibre.classes;
using System.Windows.Media.Imaging;

namespace DactyloLibre
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int CHARTEXTMAXCOUT = 200;

        private int charCount = 0;
        private int mistakes = 0;
        private Queue<char> historyLetters = new Queue<char> { };
        private string textPath = "";
        private int timerAlternate_nbTime = 8;
        private int timer_Count = 0;
        private DispatcherTimer dTimer;
        private string textAlertMessage = "";
        private int nbCharText = CHARTEXTMAXCOUT;
        private bool playGame = false;
        private DispatcherTimer gameTimer;
        private int alternater_time = 0;
        private long time_start = 0;
        private readonly Lang_parser langparser = new Lang_parser();

        public MainWindow()
        {
            InitializeComponent();

            dTimer = new DispatcherTimer();
            dTimer.Tick += new EventHandler(DTimerTick_AlertAlternate);
            dTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);

            gameTimer = new DispatcherTimer();
            gameTimer.Tick += new EventHandler(GameTime_tick);
            gameTimer.Interval = new TimeSpan(0, 0, 1);

        
            aboutButton.Content = langparser.Finder().Buttons.About;
            importTextBtn_obj.Content = langparser.Finder().Buttons.Import;
            launchButten.Content = langparser.Finder().Buttons.Launch;
            nameTextbox.Text = langparser.Finder().Textbox.Name;
            timeLabel.Content = langparser.Finder().Label.Time;
            charachterCount.Content = langparser.Finder().Label.Characters;

            imageInstruction.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory()+".\\res\\"+langparser.Finder().Image.ImgInstructionName +".png"));

            }
        
        private void GameTime_tick(Object sender, EventArgs e)
        {
            if (!playGame) gameTimer.Stop();
            timer_Count++;
            int m = timer_Count / 60;
            int s = timer_Count % 60;
            timerShow.Content = (m > 0 ? Convert.ToString(m)+"min " : "") + Convert.ToString(s)+"s";
        }

        private void ReloadText()
        {
            List<String> textToWrite = LoadText();
            string t = "";

            if (textToWrite.Count > 0)
            {
                for (int i = 0; i < textToWrite.Count; i++)
                {
                    t += textToWrite[i];
                }
                textPreview.Content = t;

            }
        }

        private void Viewletterpanel(char letter)
        {
            historyLetters.Enqueue(letter);

            if (historyLetters.Count > 3)
            {
                historyLetters.Dequeue();
            }


            var keyPressed_elmnt = historyKeyPressed.Children.Cast<Label>();

            switch (historyLetters.Count)
            {

                case 1:

                    keyPressed_elmnt.ElementAt(2).Content = historyLetters.ElementAt(0);
                    break;

                case 2:
                    keyPressed_elmnt.ElementAt(1).Content = historyLetters.ElementAt(1);
                    keyPressed_elmnt.ElementAt(0).Content = historyLetters.ElementAt(0);
                    break;

                case 3:
                    keyPressed_elmnt.ElementAt(2).Content = historyLetters.ElementAt(0);
                    keyPressed_elmnt.ElementAt(1).Content = historyLetters.ElementAt(1);
                    keyPressed_elmnt.ElementAt(0).Content = historyLetters.ElementAt(2);
                    break;
                default:
                    break;
            }

        }
        private void ViewFutureLetterpanel()
        {
            Queue<char> future_letters = new Queue<char> { };

            // récupération des lettres

            for (int i = 1; i < 4; i++)
            {
                if (i >= textPreview.Content.ToString().Length) break;

                future_letters.Enqueue(((string)textPreview.Content)[i]);
            }


            var keyToPress_elmts = historyKeyToPress.Children.Cast<Label>();

            switch (future_letters.Count)
            {
                case 1:

                    keyToPress_elmts.ElementAt(0).Content = future_letters.ElementAt(0);
                    keyToPress_elmts.ElementAt(1).Content = "";
                    keyToPress_elmts.ElementAt(2).Content = "";
                    break;

                case 2:
                    keyToPress_elmts.ElementAt(0).Content = future_letters.ElementAt(0);
                    keyToPress_elmts.ElementAt(1).Content = future_letters.ElementAt(1);
                    keyToPress_elmts.ElementAt(2).Content = "";

                    break;

                case 3:
                    keyToPress_elmts.ElementAt(2).Content = future_letters.ElementAt(2);
                    keyToPress_elmts.ElementAt(1).Content = future_letters.ElementAt(1);
                    keyToPress_elmts.ElementAt(0).Content = future_letters.ElementAt(0);
                    break;

                default:
                    break;
            }
            /* *********************** */
        }

        private void StopGame()
        {

            launchButten.Content = langparser.Finder().Buttons.Launch;
            playGame = false;
            gameTimer.Stop();

            long time_finished = DateTimeOffset.Now.ToUnixTimeMilliseconds();            

            ResultatTreatment rslt = new ResultatTreatment();
            if (rslt.AppendStats(nameTextbox.Text, charCount, mistakes, time_start, time_finished))
            {
                MessageBox.Show(langparser.Finder().Error.saveError);
            }
            
            var keyPressed_elmnt = historyKeyPressed.Children.Cast<Label>();
            var keyToPress_elmts = historyKeyToPress.Children.Cast<Label>();

            foreach (var item in keyToPress_elmts)
            {
                item.Content = "";
            }
            foreach (var item in keyPressed_elmnt)
            {
                item.Content = "";
            }


            previewError.Content = "";
            historyLetters.Clear();
            textPreview.Content = "";
            charCount = 0;
            mistakes = 0;
            timer_Count = 0;
            nbCharText = CHARTEXTMAXCOUT;
            timerShow.Content = 0;
            showKeyPressed.Text = null;
            charachterCount.Content = langparser.Finder().Label.Characters + " ";
            ReloadText();

            textAlertMessage = langparser.Finder().Alert.GameOver;
            dTimer.Start();
            

            new Stats().Show();

        }

        private void KeyChecker(object sender, TextChangedEventArgs e)
        {
            if (!playGame || showKeyPressed.Text.Length < 1) return;

            char letter = showKeyPressed.Text[0];

            if (nbCharText < 1)
            {
                // Game finished
                StopGame();
                return;
            }

            showKeyPressed.Text = Convert.ToString(letter);


            if (((string)textPreview.Content)[0] == letter)
            {
                charachterCount.Content = langparser.Finder().Label.Characters + " " + ++charCount;
                previewError.Foreground = new SolidColorBrush(Colors.Green);
                previewError.Content = letter;
                ReloadText();
                Viewletterpanel(letter);
            }
            else
            {
                mistakes++;
                previewError.Foreground = Brushes.Red;
                previewError.Content = letter;

            }
            ViewFutureLetterpanel();
            showKeyPressed.Text = "";
        
        }

        private void AboutClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://nadnone.github.io/DactyloLibre-CsharpVersion/");
        }

        private List<String> LoadText()
        {
            if (File.Exists(textPath))
            {
                StreamReader file = new StreamReader(textPath);
                List<String> textToWrite = new List<string> { };

                for (int i = 0; i < charCount; i++)
                {
                    if (file.EndOfStream) break;
                    file.Read();
                }
                for (int i = 0; i < nbCharText; i++)
                {
                    if (file.EndOfStream) break;
                    int s = file.Read();
                    textToWrite.Add(Convert.ToString(Convert.ToChar(s)));
                }

                file.Close();

                if (textToWrite.Count < 1) nbCharText = 0;

                return textToWrite;



            }
            else
            {
                MessageBox.Show(langparser.Finder().Error.FileUnreadable);
                return new List<String> { };
            }
        }

        private void DTimerTick_AlertAlternate(Object sender, EventArgs e)
        {
            if (++alternater_time % 2 > 0)
            {
                alertLabel.Content = textAlertMessage;
            }
            else
            {
                alertLabel.Content = "";
            }

            if (alternater_time >= timerAlternate_nbTime)
            {
                alternater_time = 0;
                dTimer.IsEnabled = false;
            }


        }

        private void ImportTextButton(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Files | *.txt;";
            ofd.CheckFileExists = true;
            ofd.DefaultExt = "txt";


            if ((bool)ofd.ShowDialog())
            {
                textPath = ofd.FileName;

                if (File.Exists(textPath))
                {
                    alertLabel.Foreground = Brushes.Green;
                    textAlertMessage = langparser.Finder().Alert.FileLoaded;
                    dTimer.Start();
                }
                else
                {
                    alertLabel.Foreground = Brushes.Red;
                    textAlertMessage = langparser.Finder().Alert.FileMissing;
                    dTimer.Start();
                }

            }


        }

        private void PlayDactylo(object sender, RoutedEventArgs e)
        {
            if (!playGame && File.Exists(textPath) && nameTextbox.Text != langparser.Finder().Textbox.Name)
            {
                time_start = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                alertLabel.Foreground = Brushes.Green;
                textAlertMessage = langparser.Finder().Alert.TimerStarted;
                dTimer.Start();

                gameTimer.Start();

                launchButten.Content = langparser.Finder().Buttons.Stop;

                playGame = true;
                ReloadText();
            }
            else if((string)launchButten.Content == langparser.Finder().Buttons.Stop && playGame)
            {
                StopGame();
            }
            else if (!File.Exists(textPath))
            {
                alertLabel.Foreground = Brushes.Red;
                textAlertMessage = langparser.Finder().Alert.FileMissing;
                dTimer.Start();
            }
            else if(nameTextbox.Text == langparser.Finder().Textbox.Name)
            {
                alertLabel.Foreground = Brushes.Red;
                textAlertMessage = langparser.Finder().Alert.NameMissing;
                dTimer.Start();
            }
            else if(playGame)
            {
                launchButten.Content = langparser.Finder().Buttons.Stop;

                alertLabel.Foreground = Brushes.Red;
                textAlertMessage = langparser.Finder().Alert.GameAlreadyStarted;
                dTimer.Start();
            }
            else
            {
                MessageBox.Show(langparser.Finder().Error.UnknownError);
            }
            
        }

        private void OnClosed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
