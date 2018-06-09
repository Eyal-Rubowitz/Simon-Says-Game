using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;
using System;
using Windows.System.Threading;
using System.Threading.Tasks;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SimonGame
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        BitmapImage[] simonPanelImages;
        SimonList simonGame;
        SimonLink guessLink;
        DispatcherTimer timer;
        private bool isShowDefault;
        private bool isReveal;
        private int guessCounter;
        private int gameSteps;
        private int gameRecord;

        public MainPage()
        {
            isShowDefault = false;
            isReveal = true;
            this.InitializeComponent();
            // set all game images + hide the "GAME OVER" sign
            InitImgs();
            // initiate game instance
            simonGame = new SimonList();
            // add the first link/step of the game
            RunTimer();
            //simonGame.RevealSequence();
        }

        private void InitImgs()
        {
            gameSteps = 0;
            gameRecord = 0;
            //txtBlPlayerRecord.Text = "Game Record: " + gameRecord;
            //txtBlPlayerScore.Text = "Game Score: " + gameSteps;
            txtBlGameOver.Visibility = Visibility.Collapsed;
            simonPanelImages = new BitmapImage[5];
            for (int i = 0; i < simonPanelImages.Length; i++)
            {
                simonPanelImages[i] = new BitmapImage(new Uri(base.BaseUri, string.Format(@"/Assets/Images/SimonPanel_{0}.png", i)));
            }
            gameImage.Source = simonPanelImages[0];
            redImg.Source = simonPanelImages[1];
            greenImg.Source = simonPanelImages[2];
            blueImg.Source = simonPanelImages[3];
            yellowImg.Source = simonPanelImages[4];
        }

        public void RunTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(600);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, object e)
        {
            RevealLink();
        }

        public void RevealLink()
        {
            isReveal = true;
            if (isShowDefault)
            {
                gameImage.Source = simonPanelImages[0];
                isShowDefault = !isShowDefault;
                return;
            }

            if (simonGame.currLink != null)
            {
                gameImage.Source = simonPanelImages[simonGame.currLink._data];
                PlayEFX(simonGame.currLink._data);
                simonGame.currLink = simonGame.currLink._next;
                if (simonGame.currLink == null)
                {
                    isReveal = false;
                    guessCounter = 0;
                    toggleButtons();
                }
            }
            else
            {
                gameImage.Source = simonPanelImages[0];
            }
            isShowDefault = !isShowDefault;
        }

        private void toggleButtons()
        {
            if (isReveal == true)
            {
                btnBlue.Visibility = Visibility.Collapsed;
                btnGreen.Visibility = Visibility.Collapsed;
                btnRed.Visibility = Visibility.Collapsed;
                btnYellow.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnBlue.Visibility = Visibility.Visible;
                btnGreen.Visibility = Visibility.Visible;
                btnRed.Visibility = Visibility.Visible;
                btnYellow.Visibility = Visibility.Visible;
            }
        }

        private void btnNewGame_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GameOver();
            gameImage.Source = simonPanelImages[0];
            toggleButtons();
            txtBlGameOver.Visibility = Visibility.Collapsed;
            simonGame.AddGameStep();
            timer.Start();
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            int buttonPressed;
            switch (((Button)sender).Name)
            {
                case "btnRed":
                    buttonPressed = 1;
                    mediaElement1.Play();
                    break;
                case "btnBlue":
                    buttonPressed = 2;
                    mediaElement2.Play();
                    break;
                case "btnYellow":
                    buttonPressed = 3;
                    mediaElement3.Play();
                    break;
                case "btnGreen":
                    buttonPressed = 4;
                    mediaElement4.Play();
                    break;
                default:
                    buttonPressed = 0;
                    break;
            }
            GuessSimonSequence(buttonPressed);
            gameImage.Source = simonPanelImages[buttonPressed];
        }

        private void GuessSimonSequence(int guess)
        {
            SimonLink tempLink = null;
            if (guessLink == null)
            {
                tempLink = simonGame._head;
                guessLink = tempLink;
            }
            else
            {
                tempLink = guessLink;
            }
            if (tempLink == null)
                return;
            if (simonGame._linksCounter >= guessCounter)
            {
                if (tempLink._data == guess)
                {
                    guessLink = guessLink._next;
                    guessCounter++;
                }
                else
                {
                    GameOver();
                    return;
                }

                if (simonGame._linksCounter == guessCounter)
                {
                    simonGame.AddGameStep();
                    guessCounter = 0;
                    guessLink = null;
                    simonGame.currLink = simonGame._head;
                    isReveal = true;
                }
            }
        }

        private void GameOver()
        {
            timer.Stop();
            guessLink = null;
            txtBlGameOver.Visibility = Visibility.Visible;
            simonGame.EndOfGame();
        }

        private void PlayEFX(int efx)
        {
            switch (efx)
            {
                case 1:
                    mediaElement1.Play();
                    break;
                case 2:
                    mediaElement2.Play();
                    break;
                case 3:
                    mediaElement3.Play();
                    break;
                case 4:
                    mediaElement4.Play();
                    break;
            }
        }
    }
}
