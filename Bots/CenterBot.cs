using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media;

namespace TicTacToe.Bots
{
    class CenterBot : GameBot
    {
        private int _myMoves;
        public CenterBot(CGameField gameField, Sign botSign, Color botColor, int fieldSize) : base(gameField, botSign, botColor, fieldSize)
        {
        }

        public override void BotMove()
        {
            if (_myMoves == 0)
            {
                //Field center, +/-
                var row = (int)Math.Floor(FieldSize / 2.0);
                var col = (int)Math.Floor(FieldSize / 2.0);


                var centralCell = GameField.GetNearestEmptyCell(row, col);
                if (centralCell != null)
                {
                    GameField.MakeMove(centralCell, BotSign, BotColor, MyPlayerId);
                    _myMoves++;
                    return;
                }
            }

            if (TryToCompleteLine())
            {
                _myMoves++;
                return;
            }

            var emptyCells = GameField.GetEmptyCells();
            var randomCell = GameField.GetRandomFromCells(emptyCells);
            GameField.MakeMove(randomCell, BotSign, BotColor, MyPlayerId);

            _myMoves++;
        }

        private bool TryToCompleteLine()
        {
            List<CellViewModel> bestLine = new List<CellViewModel>();

            //rows
            for (var r = 0; r < FieldSize; r++)
            {
                var row = GameField.GetRow(r);
                if (row.All(c => c.PlayerId == MyPlayerId || c.CellSign == Sign.Empty))
                {
                    var emptyCells = row.Where(c => c.CellSign == Sign.Empty).ToList();
                    if (bestLine.Count == 0 || bestLine.Count > emptyCells.Count())
                    {
                        bestLine = emptyCells;
                    }
                }
            }

            //cols
            for (var col = 0; col < FieldSize; col++)
            {
                var column = GameField.GetColumn(col);
                if (column.All(c => c.PlayerId == MyPlayerId || c.CellSign == Sign.Empty))
                {
                    var emptyCells = column.Where(c => c.CellSign == Sign.Empty).ToList();
                    if (bestLine.Count == 0 || bestLine.Count > emptyCells.Count())
                    {
                        bestLine = emptyCells;
                    }
                }
            }

            //diag 1 and 2
            var diag1 = GameField.GetDiagonal(false);
            if (diag1.All(c => c.PlayerId == MyPlayerId || c.CellSign == Sign.Empty))
            {
                var emptyCells = diag1.Where(c => c.CellSign == Sign.Empty).ToList();
                if (bestLine.Count == 0 || bestLine.Count > emptyCells.Count())
                {
                    bestLine = emptyCells;
                }
            }

            var diag2 = GameField.GetDiagonal(true);
            if (diag2.All(c => c.CellSign == BotSign || c.CellSign == Sign.Empty))
            {
                var emptyCells = diag2.Where(c => c.CellSign == Sign.Empty).ToList();
                if (bestLine.Count == 0 || bestLine.Count > emptyCells.Count())
                {
                    bestLine = emptyCells;
                }
            }

            if (bestLine.Count > 0)
            {
                var randomCell = GameField.GetRandomFromCells(bestLine);
                GameField.MakeMove(randomCell, BotSign, BotColor, MyPlayerId);
                return true;
            }


            return false;
        }
    }
}
