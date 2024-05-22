using System.Net.WebSockets;
using System.Text.Json;
using System.Text;
using Models;
using GameLogic;

namespace Services
{
    public class WebSocketService
    {
        private readonly List<WebSocket> _sockets = new();
        private readonly Dictionary<Guid, TicTacToeGame> _games = new();

        public async Task HandleWebSocketConnection(WebSocket socket, Guid roomId)
        {
            var gameRoom = GameRoom.GameRooms.FirstOrDefault(r => r.RoomId == roomId);
            if (gameRoom == null)
            {
                await socket.CloseAsync(WebSocketCloseStatus.InvalidPayloadData, "Room not found", CancellationToken.None);
                return;
            }

            if (gameRoom.ConnectedPlayers.Count >= gameRoom.RoomCapacity)
            {
                await socket.CloseAsync(WebSocketCloseStatus.InvalidPayloadData, "Room is full", CancellationToken.None);
                return;
            }

            gameRoom.ConnectedPlayers.Add(socket);
            _sockets.Add(socket);
            if (!_games.ContainsKey(roomId))
            {
                _games[roomId] = new TicTacToeGame();
            }
            await NotifyRoomUpdate(gameRoom);

            try
            {
                var buffer = new byte[1024 * 2];
                while (socket.State == WebSocketState.Open)
                {
                    var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), default);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, default);
                        break;
                    }

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        var playerMove = JsonSerializer.Deserialize<PlayerMove>(message);

                        var game = _games[roomId];
                        if (game.MakeMove(1, playerMove.X, playerMove.Y))
                        {
                            var (gameOver, winner) = game.CheckGameState();
                            await NotifyRoomUpdate(gameRoom);

                            if (!gameOver)
                            {
                                var (aiX, aiY) = game.MakeAIMove();
                                var (aiGameOver, aiWinner) = game.CheckGameState();
                                await NotifyRoomUpdate(gameRoom);
                                if (aiGameOver)
                                {
                                    // Handle game over scenario, e.g., notify players and reset the game
                                }
                            }
                            else
                            {
                                // Handle game over scenario, e.g., notify players and reset the game
                            }
                        }
                    }
                }
            }
            finally
            {
                gameRoom.ConnectedPlayers.Remove(socket);
                _sockets.Remove(socket);
                await NotifyRoomUpdate(gameRoom);
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "WebSocket connection closed", default);
            }
        }

        private async Task NotifyRoomUpdate(GameRoom gameRoom)
        {
            var game = _games[gameRoom.RoomId];
            var updateMessage = JsonSerializer.Serialize(new
            {
                RoomId = gameRoom.RoomId,
                PlayerCount = gameRoom.ConnectedPlayers.Count,
                CurrentPlayer = gameRoom.CurrentPlayer,
                Board = game.Board
            });

            var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(updateMessage));

            foreach (var socket in gameRoom.ConnectedPlayers)
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}