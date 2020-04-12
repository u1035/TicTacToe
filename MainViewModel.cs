using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using Prism.Commands;
using Prism.Mvvm;
using TicTacToe.Bots;

namespace TicTacToe
{
    public enum FirstMoveEnum
    {
        Human,
        Bot
    }

    public class MainViewModel : BindableBase
    {
        #region Properties

        private int _fieldSize = 3;
        private Sign _humanSign;
        private int _numberOfBots = 1;
        private string _victoryText;
        private Win _currentGameState;
        private FirstMoveEnum _firstMove;
        private PropertyInfo _selectedPlayerColor;
        private PropertyInfo _selectedBotColor;
        private Color _playerColor;
        private Color _botColor;

        public const int HumanPlayerId = -1;

        public CGameField GameField { get; set; }
        public ObservableCollection<Sign> AvailableSigns { get; set; }
        public ObservableCollection<FirstMoveEnum> AvailableStarters { get; set; }

        private readonly List<GameBot> _bots;
        public PropertyInfo[] AvailableColors { get; set; }


        public PropertyInfo SelectedBotColor
        {
            get => _selectedBotColor;
            set
            {
                if (SetProperty(ref _selectedBotColor, value))
                {
                    _botColor = (Color)ColorConverter.ConvertFromString(_selectedBotColor.Name);
                }
            }
        }
        public PropertyInfo SelectedPlayerColor
        {
            get => _selectedPlayerColor;
            set
            {
                if (SetProperty(ref _selectedPlayerColor, value))
                {
                    _playerColor = (Color)ColorConverter.ConvertFromString(_selectedPlayerColor.Name);
                }
            }
        }

        public string VictoryText
        {
            get => _victoryText;
            set => SetProperty(ref _victoryText, value);
        }
        public int NumberOfBots
        {
            get => _numberOfBots;
            set => SetProperty(ref _numberOfBots, value);
        }
        public int FieldSize
        {
            get => _fieldSize;
            set => SetProperty(ref _fieldSize, value);
        }

        public DelegateCommand StartButtonCommand { get; }
        public DelegateCommand StopButtonCommand { get; }


        public Sign HumanSign
        {
            get => _humanSign;
            set => SetProperty(ref _humanSign, value);
        }

        private Sign BotSign => _humanSign == Sign.O ? Sign.X : Sign.O;
        private int CellsNumber => (int)Math.Pow(_fieldSize, 2);

        public FirstMoveEnum FirstMove
        {
            get => _firstMove;
            set => SetProperty(ref _firstMove, value);
        }

        #endregion

        #region Constructors

        public MainViewModel()
        {
            GameField = new CGameField(_fieldSize);
            AvailableSigns = new ObservableCollection<Sign>();
            AvailableStarters = new ObservableCollection<FirstMoveEnum>();
            AvailableColors = typeof(Colors).GetProperties();
            SelectedPlayerColor = AvailableColors.FirstOrDefault(c => c.Name == "Green");
            SelectedBotColor = AvailableColors.FirstOrDefault(c => c.Name == "Red");

            StartButtonCommand = new DelegateCommand(StartNewGame, CanStartNewGame);
            StopButtonCommand = new DelegateCommand(StopGame, CanStopGame);

            AvailableSigns.Add(Sign.X);
            AvailableSigns.Add(Sign.O);
            HumanSign = AvailableSigns[0];

            AvailableStarters.Add(FirstMoveEnum.Bot);
            AvailableStarters.Add(FirstMoveEnum.Human);
            FirstMove = AvailableStarters[0];

            _bots = new List<GameBot>();
        }

        #endregion

        private void StartNewGame()
        {
            VictoryText = "";
            GameField.CreateGameField(FieldSize, HumanMove);

            _bots.Clear();
            //_bots.Add(new RandomBot(GameField, BotSign, _botColor, FieldSize));
            _bots.Add(new CenterBot(GameField, BotSign, _botColor, FieldSize));

            if (NumberOfBots > 1)
            {
                var r = new Random();
                for (var i = 0; i < NumberOfBots - 1; i++)
                    _bots.Add(new CenterBot(GameField, BotSign, Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(1, 233)), FieldSize));
            }

            if (FirstMove == FirstMoveEnum.Bot) BotMove();
        }

        private void StopGame()
        {
            if (!GameField.GameInProgress) return;

            VictoryText = "";
            _bots.Clear();
            GameField.GameField.Clear();

            GameField.GameInProgress = false;
        }

        private static bool CanStartNewGame() => true;
        private static bool CanStopGame() => true;


        private void HumanMove(object sender)
        {
            GameField.MakeMove(((CellViewModel)sender).Row, ((CellViewModel)sender).Column, HumanSign, _playerColor, HumanPlayerId);
            CheckVictory();

            if (_currentGameState.GameOver) return;

            BotMove();
        }

        private void BotMove()
        {
            foreach (var bot in _bots)
            {
                if (GameField.FilledCells == CellsNumber) return;
                bot.BotMove();

                CheckVictory();
            }
        }

        private void CheckVictory()
        {
            _currentGameState = GameField.CheckVictory();
            if (_currentGameState.GameOver)
            {
                GameField.DisableGameField();
                GameField.GameInProgress = false;
                GameOver(_currentGameState);
            }

        }

        private void GameOver(Win winner)
        {
            if (winner.WinnerSign == HumanSign)
            {
                VictoryText = "Player win";
                GameField.GameOverHighlightCells(winner.WinnerLine, _playerColor);
            }
            else if (winner.WinnerSign == BotSign)
            {
                VictoryText = "Bot win";
                GameField.GameOverHighlightCells(winner.WinnerLine, _botColor);
            }
            else
            {
                VictoryText = "Nobody win";
            }
        }


    }
}
