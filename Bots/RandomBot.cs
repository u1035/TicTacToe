using System;
using System.Windows.Media;

namespace TicTacToe.Bots
{
    public class RandomBot : GameBot
    {
        public RandomBot(State botSign, Color botColor, int fieldSize) : base(botSign, botColor, fieldSize)
        {
        }

        public override void BotMove(CGameField gameField)
        {
            var rnd = new Random();

            var emptyCells = gameField.GetEmptyCells();
            var randomCell = rnd.Next(gameField.EmptyCells);
            emptyCells[randomCell].Mark(BotSign, BotColor);
        }
    }
}
