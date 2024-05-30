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
        public static int[] MakeComputerMove(int[] board, string difficulty)
        {
            switch (difficulty)
            {
                case "Easy":
                    return MakeRandomMove(board);
                case "Hard":
                    return MakeStrategicMove(board);
                default:
                    return MakeRandomMove(board);
            }
        }

        private static int[] MakeRandomMove(int[] board)
        {
            var emptyPositions = board.Select((value, index) => new { value, index })
                                    .Where(x => x.value == 0)
                                    .Select(x => x.index)
                                    .ToList();
            if (emptyPositions.Count == 0)
            {
                return board;
            }

            var random = new Random();
            int move = emptyPositions[random.Next(emptyPositions.Count)];
            board[move] = 2; // Assuming the computer is always 'X'

            return board;
        }

        private static int[] MakeStrategicMove(int[] board)
        {
            // This is a simple strategy that could be improved in future extensions of the game
            var emptyPositions = board.Select((value, index) => new { value, index })
                                    .Where(x => x.value == 0)
                                    .Select(x => x.index)
                                    .ToList();
            if (emptyPositions.Count == 0)
            {
                return board;
            }

            // Try to win
            foreach (var pos in emptyPositions)
            {
                var testBoard = (int[])board.Clone();
                testBoard[pos] = 2;
                if (CheckGameState(testBoard).Item1 == GameState.Win)
                {
                    board[pos] = 2;
                    return board;
                }
            }

            // Block opponent from winning
            foreach (var pos in emptyPositions)
            {
                var testBoard = (int[])board.Clone();
                testBoard[pos] = 1;
                if (CheckGameState(testBoard).Item1 == GameState.Win)
                {
                    board[pos] = 2;
                    return board;
                }
            }

            // Otherwise, make a random move
            return MakeRandomMove(board);
        }
    }

}
