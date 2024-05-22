namespace GameLogic
{
    public class TicTacToeGame
    {
        public int[,] Board { get; private set; } = new int[3, 3];
        public int CurrentPlayer { get; private set; } = 1; // 1 for player, -1 for PC

        public bool MakeMove(int player, int x, int y)
        {
            if (Board[x, y] == 0)
            {
                Board[x, y] = player;
                CurrentPlayer = -CurrentPlayer; // Switch player
                return true;
            }
            return false;
        }

        public (bool gameOver, int winner) CheckGameState()
        {
            // Check rows, columns, and diagonals
            for (int i = 0; i < 3; i++)
            {
                if (Math.Abs(Board[i, 0] + Board[i, 1] + Board[i, 2]) == 3) return (true, Board[i, 0]);
                if (Math.Abs(Board[0, i] + Board[1, i] + Board[2, i]) == 3) return (true, Board[0, i]);
            }
            if (Math.Abs(Board[0, 0] + Board[1, 1] + Board[2, 2]) == 3) return (true, Board[0, 0]);
            if (Math.Abs(Board[2, 0] + Board[1, 1] + Board[0, 2]) == 3) return (true, Board[2, 0]);

            // Check for draw
            bool draw = true;
            foreach (var cell in Board)
            {
                if (cell == 0) draw = false;
            }
            return (draw, 0);
        }

        public (int x, int y) MakeAIMove()
        {
            // Simple AI: Make the first available move
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Board[i, j] == 0)
                    {
                        MakeMove(-1, i, j);
                        return (i, j);
                    }
                }
            }
            return (-1, -1); // Should never reach here if called correctly
        }
    }
}