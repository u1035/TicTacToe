using System;
using System.Windows;
using System.Windows.Media;
using Prism.Commands;
using Prism.Mvvm;

namespace TicTacToe
{
    public enum Sign
    {
        Empty,
        X,
        O
    }

    public class CellViewModel : BindableBase
    {
        #region Properties

        private Sign _state = Sign.Empty;
        private Brush _foregroundBrush = new SolidColorBrush(SystemColors.ControlTextColor);
        private Brush _borderBrush = new SolidColorBrush(SystemColors.ActiveBorderColor);
        private int _borderThickness = 1;
        private bool _enabled = true;
        private string _text;
        private int _row;
        private int _col;
        private int _playerId;

        public Sign CellSign
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

        public int PlayerId
        {
            get => _playerId;
            private set => _playerId = value;
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
            switch (CellSign)
            {
                case Sign.Empty:
                    Text = "";
                    break;
                case Sign.X:
                    Text = "X";
                    break;
                case Sign.O:
                    Text = "O";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Mark(Sign sign, Color color, int playerId)
        {
            if (CellSign != Sign.Empty) return;
            CellSign = sign;
            ForegroundBrush = new SolidColorBrush(color);
            PlayerId = playerId;
        }

        public void GameOverHighlight(Color color)
        {
            BorderThickness = 3;
            BorderBrush = new SolidColorBrush(color);
        }

        private void ButtonClicked()
        {
            if (!Enabled || CellSign != Sign.Empty) return;
            OnClick?.Invoke(this);
        }
        private static bool CanClick() => true;
    }
}
