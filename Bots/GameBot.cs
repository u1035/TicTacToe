using System.Windows.Media;

namespace TicTacToe.Bots
{
    public abstract class GameBot
    {
        protected readonly Sign BotSign;
        protected readonly Color BotColor;
        protected readonly int FieldSize;
        protected readonly CGameField GameField;
        protected readonly int MyPlayerId;

        protected GameBot(CGameField gameField, Sign botSign, Color botColor, int fieldSize)
        {
            BotColor = botColor;
            FieldSize = fieldSize;
            BotSign = botSign;
            GameField = gameField;

            MyPlayerId = PlayerIdGenerator.GetNewPlayerId();
        }

        public abstract void BotMove();
    }
}