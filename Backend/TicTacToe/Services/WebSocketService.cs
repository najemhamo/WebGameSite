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

        public async Task HandleWebSocketConnection(WebSocket socket)
        {
            _sockets.Add(socket);
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

                    // Handle received message if needed

                    // Optionally, broadcast message to other clients
                    foreach (var s in _sockets.Where(s => s != socket && s.State == WebSocketState.Open))
                    {
                        await s.SendAsync(buffer[..result.Count], WebSocketMessageType.Text, true, default);
                    }
                }
            }
            finally
            {
                _sockets.Remove(socket);
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "WebSocket connection closed", default);
            }
        }

        // A method to join a game room
        public async Task JoinGameRoom(Guid roomId)
        {
            var room = GameRoom.GameRooms.FirstOrDefault(x => x.RoomId == roomId);
            if (room == null)
            {
                return;
            }

            if (room.RoomCapacity == 2)
            {
                return;
            }
            foreach (var socket in _sockets)
            {
                if (socket.State == WebSocketState.Open)
                {
                    room.RoomCapacity++;
                    await socket.SendAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(room)), WebSocketMessageType.Text, true, default);
                }
            }
        }

        // A method to leave a game room
        public async Task LeaveGameRoom(Guid roomId)
        {
            var room = GameRoom.GameRooms.FirstOrDefault(x => x.RoomId == roomId);
            if (room == null)
            {
                return;
            }

            if (room.RoomCapacity == 0)
            {
                return;
            }
            foreach (var socket in _sockets)
            {
                if (socket.State == WebSocketState.Open)
                {
                    room.RoomCapacity--;
                    await socket.SendAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(room)), WebSocketMessageType.Text, true, default);
                }
            }
        }
    }
}