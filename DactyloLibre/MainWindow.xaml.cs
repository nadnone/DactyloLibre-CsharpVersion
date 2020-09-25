using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using IniParser;
using IniParser.Model;
using DactyloLibre.classes;
using System.Runtime;
using System.Windows.Media.Imaging;
using System.Security.Policy;
using DactyloLibre.Models;

namespace DactyloLibre
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int CHARTEXTMAXCOUT = 200;

        private Int16 charCount = 0;
        private Int16 fault = 0;
        private char letter = ' ';
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
        private Lang_parser langparser = new Lang_parser();

        public MainWindow()
        {
            InitializeComponent();

            dTimer = new DispatcherTimer();
            dTimer.Tick += new EventHandler(dTimerTick_AlertAlternate);
            dTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);

            gameTimer = new DispatcherTimer();
            gameTimer.Tick += new EventHandler(gameTime_tick);
            gameTimer.Interval = new TimeSpan(0, 0, 1);

        
            aboutButton.Content = langparser.finder().Buttons.about;
            importTextBtn_obj.Content = langparser.finder().Buttons.import;
            launchButten.Content = langparser.finder().Buttons.launch;
            nameTextbox.Text = langparser.finder().Textbox.name;
            timeLabel.Content = langparser.finder().Label.time;
            charachterCount.Content = langparser.finder().Label.characters;

            imageInstruction.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory()+".\\res\\"+langparser.finder().Image.ImgInstructionName +".png"));

            }
        
        private void gameTime_tick(Object sender, EventArgs e)
        {
            if (!playGame) gameTimer.Stop();
            timer_Count++;
            int m = timer_Count / 60;
            int s = timer_Count % 60;
            timerShow.Content = (m > 0 ? Convert.ToString(m)+"min " : "") + Convert.ToString(s)+"s";
        }

        private void reloadText()
        {
            List<String> text2w = new List<String> { };
            text2w = LoadText();
            string t = "";
            if (text2w.Count > 0)
            {
                for (int i = 0; i < text2w.Count; i++)
                {
                    t += text2w[i];
                }
                textPreview.Content = t;

            }
        }

        private void viewletterpanel(char lastletter)
        {
            historyLetters.Enqueue(letter);

            if (historyLetters.Count > 3)
            {
                historyLetters.Dequeue();
            }

            switch (historyLetters.Count)
            {

                case 1:
                    historyKeyPressed3.Content = historyLetters.ElementAt(0);

                    break;
                case 2:
                    historyKeyPressed2.Content = historyLetters.ElementAt(1);
                    historyKeyPressed3.Content = historyLetters.ElementAt(0);
                    break;
                case 3:
                    historyKeyPressed3.Content = historyLetters.ElementAt(0);
                    historyKeyPressed2.Content = historyLetters.ElementAt(1);
                    historyKeyPressed1.Content = historyLetters.ElementAt(2);
                    break;
                default:
                    break;
            }

            /* 
             * A revoir
             */

            Queue<char> vQueue = new Queue<char> { };

            vQueue.Enqueue(lastletter);
            if (((string)textPreview.Content).Length < 3) return;

            for(int i = 0; i < 2; i++)
            {
                vQueue.Enqueue(((string)textPreview.Content)[i]);
            }

            if (vQueue.Count > 3) vQueue.Dequeue();

            switch (vQueue.Count)
            {
                case 1:
                    historyKeyToPressed1.Content = vQueue.ElementAt(0);
                    break;
                case 2:
                    historyKeyToPressed2.Content = vQueue.ElementAt(1);
                    historyKeyToPressed1.Content = vQueue.ElementAt(0);
                    break;
                case 3:
                    historyKeyToPressed3.Content = vQueue.ElementAt(0);
                    historyKeyToPressed2.Content = vQueue.ElementAt(2);
                    historyKeyToPressed1.Content = vQueue.ElementAt(1);
                    break;
                default:
                    break;
            }
            /* *********************** */
        }

        private void stopGame()
        {
            playGame = false;
            gameTimer.Stop();

            long time_finished = DateTimeOffset.Now.ToUnixTimeMilliseconds();            

            ResultatTreatment rslt = new ResultatTreatment();
            if (rslt.appendStats(nameTextbox.Text, charCount, fault, time_start, time_finished))
            {
                MessageBox.Show(langparser.finder().Error.saveError);
            }

            historyKeyPressed1.Content = "";
            historyKeyPressed2.Content = "";
            historyKeyPressed3.Content = "";
            historyKeyToPressed1.Content = "";
            historyKeyToPressed2.Content = "";
            historyKeyToPressed3.Content = "";
            previewError.Content = "";
            historyLetters.Clear();
            textPreview.Content = "";
            charCount = 0;
            fault = 0;
            timer_Count = 0;
            nbCharText = CHARTEXTMAXCOUT;
            timerShow.Content = 0;
            showKeyPressed.Text = null;
            charachterCount.Content = langparser.finder().Label.characters + " ";
            reloadText();

            textAlertMessage = langparser.finder().Alert.gameOver;
            dTimer.Start();


            

            new Stats().Show();

        }

        private void keyDown(object sender, TextChangedEventArgs e)
        {
            if (!playGame || showKeyPressed.Text.Length < 1) return;

            letter = showKeyPressed.Text[0];

            if (nbCharText < 1)
            {
                // Game finished
                stopGame();
                return;
            }

            showKeyPressed.Text = Convert.ToString(letter);


            if (((string)textPreview.Content)[0] == letter)
            {
                charachterCount.Content = langparser.finder().Label.characters + " " + ++charCount;
                previewError.Foreground = new SolidColorBrush(Colors.Green);
                previewError.Content = letter;
                reloadText();
                viewletterpanel(letter);
            }
            else
            {
                fault++;
                previewError.Foreground = Brushes.Red;
                previewError.Content = letter;
            }
            showKeyPressed.Text = "";
        
        }

        private void aboutClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://spoutnik911.github.io/DactyloLibre-CsharpVersion/");
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
                    Int32 s = file.Read();
                    textToWrite.Add(Convert.ToString(Convert.ToChar(s)));
                }

                file.Close();

                if (textToWrite.Count < 1) nbCharText = 0;

                return textToWrite;



            }
            else
            {
                MessageBox.Show(langparser.finder().Error.FileUnreadable);
                return new List<String> { };
            }
        }

        private void dTimerTick_AlertAlternate(Object sender, EventArgs e)
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

        private void importTextButton(object sender, RoutedEventArgs e)
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
                    textAlertMessage = langparser.finder().Alert.fileLoaded;
                    dTimer.Start();
                }
                else
                {
                    alertLabel.Foreground = Brushes.Red;
                    textAlertMessage = langparser.finder().Alert.fileMissing;
                    dTimer.Start();
                }

            }


        }

        private void playDactylo(object sender, RoutedEventArgs e)
        {
            if (!playGame && File.Exists(textPath) && nameTextbox.Text != langparser.finder().Textbox.name)
            {
                time_start = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                alertLabel.Foreground = Brushes.Green;
                textAlertMessage = langparser.finder().Alert.TimerStarted;
                dTimer.Start();

                gameTimer.Start();

                launchButten.Content = langparser.finder().Buttons.stop;

                playGame = true;
                reloadText();
            }
            else if((string)launchButten.Content == langparser.finder().Buttons.stop && playGame)
            {
                stopGame();
            }
            else if (!File.Exists(textPath))
            {
                alertLabel.Foreground = Brushes.Red;
                textAlertMessage = langparser.finder().Alert.fileMissing;
                dTimer.Start();
            }
            else if(nameTextbox.Text == langparser.finder().textbox.name)
            {
                alertLabel.Foreground = Brushes.Red;
                textAlertMessage = langparser.finder().Alert.nameMissing;
                dTimer.Start();
            }
            else if(playGame)
            {
                launchButten.Content = langparser.finder().Buttons.stop;

                alertLabel.Foreground = Brushes.Red;
                textAlertMessage = langparser.finder().Alert.gameAlreadyStarted;
                dTimer.Start();
            }
            else
            {
                MessageBox.Show(langparser.finder().Error.idontKnowButError);
            }
            
        }

        private void onClosed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
