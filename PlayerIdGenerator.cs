using System;
using System.Collections.Generic;

namespace TicTacToe
{
    public static class PlayerIdGenerator
    {
        private static readonly List<int> UsedIds = new List<int>();
        public static int GetNewPlayerId()
        {
            var r = new Random();

            while (true)
            {
                var rnd = r.Next();
                if (UsedIds.Contains(rnd)) continue;

                UsedIds.Add(rnd);
                return rnd;
            }
        }
    }
}
