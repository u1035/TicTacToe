using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace TicTacToe
{
    public class GameBot
    {
        private readonly State _botSign;
        private readonly Color _botColor;


        public GameBot(State botSign, Color botColor)
        {
            _botColor = botColor;
            _botSign = botSign;
        }

        public void BotMove(IEnumerable<CellViewModel> gameField, int fieldSize, ref int cellsFilled)
        {
            var cellsNumber = fieldSize * fieldSize;

            if (cellsFilled == cellsNumber) return;

            var rnd = new Random();

            var emptyCells = gameField.Where(c => c.CellState == State.Empty).ToArray();
            var rndCell = rnd.Next(emptyCells.Count());
            emptyCells[rndCell].CellState = _botSign;
            emptyCells[rndCell].Highlight(_botColor);
            cellsFilled++;
        }
    }
}
