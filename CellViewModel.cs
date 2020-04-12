using System;
using System.Windows;
using System.Windows.Media;
using Prism.Commands;
using Prism.Mvvm;

namespace TicTacToe
{
    public enum State
    {
        Empty,
        X,
        O
    }

    public class CellViewModel : BindableBase
    {
        #region Properties

        private State _state = State.Empty;
        private Brush _foregroundBrush = new SolidColorBrush(SystemColors.ControlTextColor);
        private Brush _borderBrush = new SolidColorBrush(SystemColors.ActiveBorderColor);
        private int _borderThickness = 1;
        private bool _enabled = true;
        private string _text;
        private int _row;
        private int _col;

        public State CellState
        {
            get => _state;
            private set
            {
                if (SetProperty(ref _state, value)) ChangeButtonText();
            }
        }

        public string Text
        {
            get => _text;
            private set => SetProperty(ref _text, value);
        }

        public int Column
        {
            get => _col;
            private set => SetProperty(ref _col, value);
        }
        public int Row
        {
            get => _row;
            private set => SetProperty(ref _row, value);
        }
        public DelegateCommand ClickCommand { get; }

        public bool Enabled
        {
            get => _enabled;
            set => SetProperty(ref _enabled, value);
        }

        public Brush ForegroundBrush
        {
            get => _foregroundBrush;
            set => SetProperty(ref _foregroundBrush, value);
        }

        public Brush BorderBrush
        {
            get => _borderBrush;
            set => SetProperty(ref _borderBrush, value);
        }
        public int BorderThickness
        {
            get => _borderThickness;
            set => SetProperty(ref _borderThickness, value);
        }

        public delegate void ClickEvent(object sender);
        public event ClickEvent OnClick;

        #endregion


        public CellViewModel(int row, int col)
        {
            ClickCommand = new DelegateCommand(ButtonClicked, CanClick);
            Row = row;
            Column = col;
        }

        private void ChangeButtonText()
        {
            switch (CellState)
            {
                case State.Empty:
                    Text = "";
                    break;
                case State.X:
                    Text = "X";
                    break;
                case State.O:
                    Text = "O";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Mark(State sign, Color color)
        {
            if (CellState != State.Empty) return;
            CellState = sign;
            ForegroundBrush = new SolidColorBrush(color);
        }

        public void GameOverHighlight(Color color)
        {
            BorderThickness = 3;
            BorderBrush = new SolidColorBrush(color);
        }

        private void ButtonClicked()
        {
            if (!Enabled || CellState != State.Empty) return;
            OnClick?.Invoke(this);
        }
        private static bool CanClick() => true;
    }
}
