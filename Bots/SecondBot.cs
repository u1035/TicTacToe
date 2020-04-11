using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace TicTacToe.Bots
{
    class SecondBot : GameBot
    {
        public SecondBot(State botSign, Color botColor, int fieldSize) : base(botSign, botColor, fieldSize)
        {
        }

        public override void BotMove(IEnumerable<CellViewModel> gameField)
        {
            
        }
    }
}
