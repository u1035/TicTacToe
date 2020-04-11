using System;
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
        private State _state = State.Empty;
        private readonly State _humanSymbol;
        private bool _enabled = true;
        private string _text;
        private int _row;
        private int _col;

        public State CellState
        {
            get => _state;
            set
            {
                if (SetProperty(ref _state, value))
                {
                    ChangeButtonText();
                }
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


        public delegate void ClickEvent(object sender);
        public event ClickEvent OnClick;

        public CellViewModel(int row, int col, State humanSymbol)
        {
            ClickCommand = new DelegateCommand(ButtonClicked, CanClick);
            Row = row;
            Column = col;
            _humanSymbol = humanSymbol;
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

        private void ButtonClicked()
        {
            if (CellState != State.Empty) return;

            CellState = _humanSymbol;
            OnClick?.Invoke(this);
        }
        private bool CanClick()
        {
            return CellState == State.Empty;
        }
    }
}
