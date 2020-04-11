using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using Prism.Commands;
using Prism.Mvvm;

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
        private State _humanSign;
        private int _cellsFilled;
        private string _victoryText;
        private bool _botWin;
        private bool _humanWin;
        private FirstMoveEnum _firstMove;

        public ObservableCollection<CellViewModel> GameField { get; set; }
        public ObservableCollection<State> AvailableSigns { get; set; }
        public ObservableCollection<FirstMoveEnum> AvailableStarters { get; set; }

        private bool _controlsEnabled = true;

        public bool ControlsEnabled
        {
            get => _controlsEnabled;
            set => SetProperty(ref _controlsEnabled, value);
        }

        public string VictoryText
        {
            get => _victoryText;
            set => SetProperty(ref _victoryText, value);
        }

        public int FieldSize
        {
            get => _fieldSize;
            set => SetProperty(ref _fieldSize, value);
        }

        public DelegateCommand StartButtonCommand { get; }

        public State HumanSign
        {
            get => _humanSign;
            set => SetProperty(ref _humanSign, value);
        }

        private State BotSign => _humanSign == State.O ? State.X : State.O;
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
            GameField = new ObservableCollection<CellViewModel>();
            AvailableSigns = new ObservableCollection<State>();
            AvailableStarters = new ObservableCollection<FirstMoveEnum>();
            StartButtonCommand = new DelegateCommand(StartNewGame, CanStartNewGame);

            AvailableSigns.Add(State.X);
            AvailableSigns.Add(State.O);
            HumanSign = AvailableSigns[0];

            AvailableStarters.Add(FirstMoveEnum.Bot);
            AvailableStarters.Add(FirstMoveEnum.Human);
            FirstMove = AvailableStarters[0];
        }

        #endregion

        private void StartNewGame()
        {
            ControlsEnabled = false;
            _botWin = false;
            _humanWin = false;
            _cellsFilled = 0;
            VictoryText = "";
            CreateGameField();

            if (FirstMove == FirstMoveEnum.Bot) BotMove();
        }

        private static bool CanStartNewGame() => true;

        private void HumanMove(object sender)
        {
            _cellsFilled++;
            CheckVictory();

            BotMove();
            CheckVictory();
        }

        private void CheckVictory()
        {
            //Check rows
            for (var r = 0; r < FieldSize; r++)
            {
                var cells = GameField.Where(c => c.Row == r).ToArray();
                if (CheckCellsGroup(cells))
                {
                    GameOver();
                    return;
                }
            }

            //Check cols
            for (var col = 0; col < FieldSize; col++)
            {
                var cells = GameField.Where(c => c.Column == col).ToArray();
                if (CheckCellsGroup(cells))
                {
                    GameOver();
                    return;
                }
            }

            //Check diagonals
            var diagonal = new List<CellViewModel>();
            for (var i = 0; i < FieldSize; i++)
            {
                var cell = GameField.FirstOrDefault(c => c.Column == i && c.Row == i);
                if (cell != null)
                    diagonal.Add(cell);
            }

            if (CheckCellsGroup(diagonal.ToArray()))
            {
                GameOver();
                return;
            }

            diagonal.Clear();
            for (var i = 0; i < FieldSize; i++)
            {
                var cell = GameField.FirstOrDefault(c => c.Column == FieldSize - i - 1 && c.Row == i);
                if (cell != null)
                    diagonal.Add(cell);
            }

            if (CheckCellsGroup(diagonal.ToArray()))
            {
                GameOver();
                return;
            }

            if (_cellsFilled == CellsNumber && !_botWin && !_humanWin)
                NobodyWin();
        }

        private bool CheckCellsGroup(CellViewModel[] cells)
        {
            if (cells.All(c => c.CellState == HumanSign))
            {
                _humanWin = true;
                HighlightCells(cells, Colors.Green);
                return true;
            }

            if (cells.All(c => c.CellState == BotSign))
            {
                _botWin = true;
                HighlightCells(cells, Colors.Red);
                return true;
            }

            return false;
        }

        private void NobodyWin()
        {
            DisableGameField();
            VictoryText = "Nobody win";

            ControlsEnabled = true;
        }

        private void GameOver()
        {
            DisableGameField();

            if (_humanWin)
                HumanVictory();

            if (_botWin)
                BotVictory();

            ControlsEnabled = true;
        }

        private void HumanVictory()
        {
            VictoryText = "Human win";
        }

        private void BotVictory()
        {
            VictoryText = "Bot win";

        }

        private void BotMove()
        {
            if (_cellsFilled == CellsNumber)
            {
                ControlsEnabled = true;
                return;
            }

            var rnd = new Random();

            if (_cellsFilled == CellsNumber - 1)
            {
                var cell = GameField.FirstOrDefault(c => c.CellState == State.Empty);
                if (cell != null)
                {
                    cell.CellState = BotSign;
                    _cellsFilled++;
                    return;
                }
            }

            var emptyCells = GameField.Where(c => c.CellState == State.Empty).ToArray();
            var rndCell = rnd.Next(emptyCells.Count());
            emptyCells[rndCell].CellState = BotSign;

            _cellsFilled++;
        }


        private void CreateGameField()
        {
            GameField.Clear();

            for (var r = 0; r < FieldSize; r++)
            {
                for (var c = 0; c < FieldSize; c++)
                {
                    var cell = new CellViewModel(r, c, HumanSign);
                    cell.OnClick += HumanMove;
                    GameField.Add(cell);
                }
            }
        }

        private void DisableGameField()
        {
            foreach (var cell in GameField) cell.Enabled = false;
        }

        private void HighlightCells(CellViewModel[] cells, Color color)
        {
            foreach (var cell in cells)
                cell.Highlight(color);
        }
    }
}
