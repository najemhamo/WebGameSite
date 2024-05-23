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
        public async Task JoinGameRoom(Guid roomId, string playerName)
        {
            var room = GameRoom.GameRooms.FirstOrDefault(x => x.RoomId == roomId);
            if (room == null || room.RoomCapacity >= 2)
            {
                return;
            }

            if (room.RoomCapacity == 0)
            {
                room.PlayerX = playerName;
            }

            else if (room.RoomCapacity == 1)
            {
                room.PlayerO = playerName;
            }

            if (_sockets[0].State == WebSocketState.Open)
            {
                room.RoomCapacity++;
                await _sockets[0].SendAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(room)), WebSocketMessageType.Text, true, default);
            }

        }

        // A method to leave a game room in real-time
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

        public async Task PlayerMove(PlayerMove move)
        {
            var room = GameRoom.GameRooms.FirstOrDefault(x => x.RoomId == move.RoomId);
            if (room == null)
            {
                return;
            }

            if ((move.Player == "X" && room.PlayerX != move.Player) ||
                    (move.Player == "O" && room.PlayerO != move.Player) ||
                    !TicTacToeGame.IsValidMove(room.Board, Array.IndexOf(move.Board, move.Player == "X" ? 1 : 2)))
                {
                    return;
                }

            room.Board = move.Board;
            var (state, updatedBoard) = TicTacToeGame.CheckGameState(room.Board);
            move.GameState = state;
            move.Board = updatedBoard;

            var moveJson = JsonSerializer.Serialize(move);

            if (_sockets[0].State == WebSocketState.Open)
            {
                await _sockets[0].SendAsync(Encoding.UTF8.GetBytes(moveJson), WebSocketMessageType.Text, true, default);
            }
        
        }
    }
}
