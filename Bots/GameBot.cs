using System.Collections.Generic;
using System.Windows.Media;

namespace TicTacToe.Bots
{
    public abstract class GameBot
    {
        protected readonly State BotSign;
        protected readonly Color BotColor;

        protected GameBot(State botSign, Color botColor)
        {
            BotColor = botColor;
            BotSign = botSign;
        }

        public abstract void BotMove(IEnumerable<CellViewModel> gameField, int fieldSize, ref int cellsFilled);
    }
}