using System.Net.WebSockets;
using System.Text.Json;
using System.Text;
using Models;

namespace Services
{
    public class WebSocketService
    {
        private readonly List<WebSocket> _sockets = new();

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

                    // Handle received message if needed later in any extensions of the game.
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
            var updateMessage = JsonSerializer.Serialize(new
            {
                RoomId = gameRoom.RoomId,
                PlayerCount = gameRoom.ConnectedPlayers.Count,
                CurrentPlayer = gameRoom.CurrentPlayer
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