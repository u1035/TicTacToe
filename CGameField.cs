﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using Prism.Mvvm;

namespace TicTacToe
{

    public class Win
    {
        public bool GameOver { get; set; }
        public Sign WinnerSign { get; set; }

        public IEnumerable<CellViewModel> WinnerLine { get; set; }
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
        public int EmptyCells => _gameField.Count(c => c.CellSign == Sign.Empty);
        public int FilledCells => Rows * Cols - EmptyCells;




        public CGameField(int fieldSize)
        {
            _gameField = new ObservableCollection<CellViewModel>();
            FieldSize = fieldSize;
        }

        public void MakeMove(int row, int col, Sign sign, Color color, int playerId)
        {
            var cell = GetCellAtPosition(row, col);
            cell.Mark(sign, color, playerId);
        }

        public void MakeMove(CellViewModel cell, Sign sign, Color color, int playerId)
        {
            cell.Mark(sign, color, playerId);
        }

        public Win CheckVictory()
        {
            //Check rows
            for (var r = 0; r < FieldSize; r++)
            {
                var cells = GetRow(r);
                var result = CheckCellsGroup(cells);
                if (result.GameOver)
                {
                    return result;
                }
            }
            //Check cols
            for (var col = 0; col < FieldSize; col++)
            {
                var cells = GetColumn(col);
                var result = CheckCellsGroup(cells);
                if (result.GameOver)
                {
                    return result;
                }
            }
            //Check diagonals
            var diagonal1 = GetDiagonal(false);
            var diagonal1Result = CheckCellsGroup(diagonal1);
            if (diagonal1Result.GameOver)
            {
                return diagonal1Result;
            }

            var diagonal2 = GetDiagonal(true);
            var diagonal2Result = CheckCellsGroup(diagonal2);
            if (diagonal2Result.GameOver)
            {
                return diagonal2Result;
            }

            if (FilledCells == CellsNumber)
                return new Win() { GameOver = true, WinnerSign = Sign.Empty };


            return new Win() { GameOver = false, WinnerSign = Sign.Empty };
        }


        private Win CheckCellsGroup(CellViewModel[] cells)
        {
            var result = new Win();
            var cell = cells.FirstOrDefault(c => c.CellSign != Sign.Empty);
            if (cell == null) return result;

            if (cells.All(c => c.CellSign == cell.CellSign))
            {
                var playerId = cell.PlayerId;
                if (cells.All(c => c.PlayerId == playerId))
                {
                    result.WinnerSign = cell.CellSign;
                    result.WinnerLine = cells;
                    result.GameOver = true;
                }
            }
            return result;
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

        public CellViewModel[] GetEmptyCells() => _gameField.Where(c => c.CellSign == Sign.Empty).ToArray();
        public CellViewModel[] GetRow(int row) => _gameField.Where(c => c.Row == row).ToArray();
        public CellViewModel[] GetColumn(int col) => _gameField.Where(c => c.Column == col).ToArray();

        public CellViewModel[] GetDiagonal(bool reversed)
        {
            var result = new List<CellViewModel>();

            for (var i = 0; i < FieldSize; i++)
            {
                var cell = reversed ?
                    GameField.FirstOrDefault(c => c.Column == FieldSize - i - 1 && c.Row == i) :
                    GameField.FirstOrDefault(c => c.Column == i && c.Row == i);

                if (cell != null)
                    result.Add(cell);
            }

            return result.ToArray();
        }

        public CellViewModel GetCellAtPosition(int row, int col)
        {
            return GameField.FirstOrDefault(c => c.Column == col && c.Row == row);
        }

        public CellViewModel GetRandomFromCells(IEnumerable<CellViewModel> cells)
        {
            var rnd = new Random();
            var cellsArray = cells as CellViewModel[] ?? cells.ToArray();
            var randomCell = rnd.Next(cellsArray.Length);
            return cellsArray.ElementAtOrDefault(randomCell);
        }

        public bool IsCellEmpty(int row, int col)
        {
            return GetCellAtPosition(row, col).CellSign == Sign.Empty;
        }

        public CellViewModel GetNearestEmptyCell(int row, int col, int watchRadius = 1)
        {
            var cell = GetCellAtPosition(row, col);
            if (cell != null && cell.CellSign == Sign.Empty)
                return cell;

            int rowMin;
            int rowMax;
            int colMin;
            int colMax;

            if (FieldSize.IsOdd())
            {
                rowMin = Math.Max(0, row - watchRadius);
                rowMax = Math.Min(FieldSize, row + watchRadius);
                colMin = Math.Max(0, col - watchRadius);
                colMax = Math.Min(FieldSize, col + watchRadius);

            }
            else
            {
                rowMin = Math.Max(0, row - watchRadius);
                rowMax = Math.Min(FieldSize, row + watchRadius - 1);
                colMin = Math.Max(0, col - watchRadius);
                colMax = Math.Min(FieldSize, col + watchRadius - 1);
            }

            var cellsAround = new List<CellViewModel>();
            for (var r = rowMin; r <= rowMax; r++)
            {
                for (var c = colMin; c <= colMax; c++)
                {
                    var foundCell = GetCellAtPosition(r, c);
                    if (foundCell != null && foundCell.CellSign == Sign.Empty)
                        cellsAround.Add(GetCellAtPosition(r, c));
                }
            }

            if (cellsAround.Count == 0) return null;

            return GetRandomFromCells(cellsAround);
        }
    }
}
