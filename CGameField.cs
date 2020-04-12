using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using Prism.Mvvm;

namespace TicTacToe
{

    public enum Winner
    {
        Nobody,
        Human,
        Bot
    }

    public class CGameField : BindableBase
    {
        private ObservableCollection<CellViewModel> _gameField;
        private int _fieldSize;
        private bool _gameInProgress;


        public bool GameInProgress
        {
            get => _gameInProgress;
            set => SetProperty(ref _gameInProgress, value);
            //{
            //    if (SetProperty(ref _gameInProgress, value))
            //        ControlsEnabled = !value;
            //}
        }

        public ObservableCollection<CellViewModel> GameField
        {
            get => _gameField;
            set => SetProperty(ref _gameField, value);
        }

        public int FieldSize
        {
            get => _fieldSize;
            set => SetProperty(ref _fieldSize, value);
        }

        public int Rows => FieldSize;
        public int Cols => FieldSize;
        public int CellsNumber => (int)Math.Pow(FieldSize, 2);
        public int EmptyCells => _gameField.Count(c => c.CellState == State.Empty);
        public int FilledCells => Rows * Cols - EmptyCells;




        public CGameField(int fieldSize)
        {
            _gameField = new ObservableCollection<CellViewModel>();
            FieldSize = fieldSize;
        }


        

        
        public void MakeMove(int row, int col, State sign, Color color)
        {
            var cell = GetCellAtPosition(row, col);
            cell.Mark(sign, color);
        }
        

        public void CreateGameField(int fieldSize, CellViewModel.ClickEvent humanMove)
        {
            GameInProgress = true;

            FieldSize = fieldSize;
            GameField.Clear();

            for (var r = 0; r < FieldSize; r++)
            {
                for (var c = 0; c < FieldSize; c++)
                {
                    var cell = new CellViewModel(r, c);
                    cell.OnClick += humanMove;
                    GameField.Add(cell);
                }
            }
        }

        public void DisableGameField()
        {
            foreach (var cell in GameField) cell.Enabled = false;
        }

        public void GameOverHighlightCells(IEnumerable<CellViewModel> cells, Color color)
        {
            foreach (var cell in cells)
                cell.GameOverHighlight(color);
        }

        public CellViewModel[] GetEmptyCells() => _gameField.Where(c => c.CellState == State.Empty).ToArray();

        public CellViewModel GetCellAtPosition(int row, int col)
        {
            return GameField.FirstOrDefault(c => c.Column == col && c.Row == row);
        }

    }
}
