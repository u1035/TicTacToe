using System.Windows.Media;

namespace TicTacToe.Bots
{
    public abstract class GameBot
    {
        protected readonly State BotSign;
        protected readonly Color BotColor;
        protected readonly int FieldSize;

        protected GameBot(State botSign, Color botColor, int fieldSize)
        {
            BotColor = botColor;
            FieldSize = fieldSize;
            BotSign = botSign;
        }

        public abstract void BotMove(CGameField gameField);
    }
}