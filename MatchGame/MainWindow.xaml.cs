using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MatchGame
{
    using System.Windows.Threading;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int timeForGame = 1000;
        readonly List<string> animalEmojiDB = new List<string>()
            {
                "🦝", "🦢", "🦩", "🦉", "🐦", "🐧", "🐌", "🐿",
                "🐘", "🦣", "🐊", "🦕", "🐢", "🦦", "🐂",
                "🦌", "🦬", "🦏", "🦛", "🐑", "🐫", "🦒",
                "🐹", "🐈", "🐕", "🐅", "🦄", "🐒", "🐎",
            };
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsRemained;
        int matchesFound;
        double myRecord = double.MaxValue;

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.01);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            tenthsOfSecondsRemained--;
            timeTextBlock.Text = (tenthsOfSecondsRemained / 100F).ToString("000.00s");
            if (tenthsOfSecondsRemained <= 0)
            {
                StopGame();
            }
            if (matchesFound == 8)
            {
                StopGame();
                double score = (timeForGame - tenthsOfSecondsRemained) / 100F;
                if (score <= myRecord)
                {
                    myRecord = score;
                    recordTextBlock.Text = myRecord.ToString("00.00s");
                    LabelForRecords.Visibility = Visibility.Hidden;
                    lblStoryboard.Visibility = Visibility.Visible;
                }
                
            }
        }

        private void SetUpGame()
        {
            List<string> animalEmoji = new List<string>(16);
            Random random = new Random();
            LabelForRecords.Visibility = Visibility.Hidden;
            lblStoryboard.Visibility = Visibility.Hidden;
            for (int i = 0; i < 16;)
            {
                int index = random.Next(animalEmojiDB.Count);
                string nextEmoji = animalEmojiDB[index];
                if (animalEmoji.Contains(nextEmoji))
                    continue;
                animalEmoji.Add(nextEmoji);
                animalEmoji.Add(nextEmoji);
                i += 2;
            }
            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>().Where(x => x.Name == ""))
            {
                int index = random.Next(animalEmoji.Count);
                string nextEmoji = animalEmoji[index];
                textBlock.Text = nextEmoji;
                textBlock.Visibility = Visibility.Visible;
                animalEmoji.RemoveAt(index);
            }
            tenthsOfSecondsRemained = timeForGame;
            matchesFound = 0;
            timer.Start();
        }

        private void StopGame()
        {
            timer.Stop();
            timeTextBlock.Text = timeTextBlock.Text + " - Play again?";
            recordTextBlock.Text = myRecord.ToString("000.00s");
            LabelForRecords.Visibility = Visibility.Visible;
            recordTextBlock.Visibility = Visibility.Visible;
        }
        TextBlock lastTextBlockClicked;
        bool findingMatch;
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if (findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }

        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8 || tenthsOfSecondsRemained == 0)
            {
                SetUpGame();
            }
        }
    }
}
