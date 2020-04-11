using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace TicTacToe.Bots
{
    public class RandomBot : GameBot
    {
        public RandomBot(State botSign, Color botColor) : base(botSign, botColor)
        {
        }

        public override void BotMove(IEnumerable<CellViewModel> gameField, int fieldSize, ref int cellsFilled)
        {
            var cellsNumber = fieldSize * fieldSize;

            if (cellsFilled == cellsNumber) return;

            var rnd = new Random();

            var emptyCells = gameField.Where(c => c.CellState == State.Empty).ToArray();
            var rndCell = rnd.Next(emptyCells.Count());
            emptyCells[rndCell].CellState = BotSign;
            emptyCells[rndCell].Highlight(BotColor);
            cellsFilled++;
        }
    }
}
