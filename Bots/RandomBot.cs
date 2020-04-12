using System.Windows.Media;

namespace TicTacToe.Bots
{
    public class RandomBot : GameBot
    {
        public RandomBot(CGameField gameField, Sign botSign, Color botColor, int fieldSize) : base(gameField, botSign, botColor, fieldSize)
        {
        }

        public override void BotMove()
        {
            var emptyCells = GameField.GetEmptyCells();
            var randomCell = GameField.GetRandomFromCells(emptyCells);
            GameField.MakeMove(randomCell, BotSign, BotColor, MyPlayerId);
        }
    }
}
