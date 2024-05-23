using Models;

namespace GameLogic
{
    public class TicTacToeGame
    {
        public static (GameState, int[]) CheckGameState(int[] board)
        {
            int[][] winPositions = {
                new[] { 0, 1, 2 },
                new[] { 3, 4, 5 },
                new[] { 6, 7, 8 },
                new[] { 0, 3, 6 },
                new[] { 1, 4, 7 },
                new[] { 2, 5, 8 },
                new[] { 0, 4, 8 },
                new[] { 2, 4, 6 }
            };

            foreach (var pos in winPositions)
            {
                if (board[pos[0]] != 0 && board[pos[0]] == board[pos[1]] && board[pos[1]] == board[pos[2]])
                {
                    var winningBoard = (int[])board.Clone();
                    winningBoard[pos[0]] = winningBoard[pos[1]] = winningBoard[pos[2]] = 3;
                    return (GameState.Win, winningBoard);
                }
            }

            if (board.All(x => x != 0))
            {
                return (GameState.Draw, board);
            }

            return (GameState.StillPlaying, board);
        }

        public static bool IsValidMove(int[] board, int move)
        {
            return move >= 0 && move < 9 && board[move] == 0;
        }
    }
}
