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
                room.PlayerO = playerName;
            }

            else if (room.RoomCapacity == 1)
            {
                room.PlayerX = playerName;
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
            if (_sockets[0].State == WebSocketState.Open)
            {
                room.RoomCapacity--;
                await _sockets[0].SendAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(room)), WebSocketMessageType.Text, true, default);
            }

        }

        public async Task MultiPlayerMove(PlayerMove move)
        {
            var room = GameRoom.GameRooms.FirstOrDefault(x => x.RoomId == move.RoomId);
            if (room == null)
            {
                return;
            }

            room.Board = move.Board;
            var (state, updatedBoard) = TicTacToeGame.CheckGameState(room.Board);
            room.Board = updatedBoard;


            // Update the game state to 1 (win) or 2 (draw)
            if (state == GameState.Win)
            {
                move.GameState = (GameState)1;
                room.Winner = move.Player;
            }
            else if (state == GameState.Draw)
            {
                move.GameState = (GameState)2;
            }
            else
            {
                move.GameState = GameState.StillPlaying;
            }

            var moveJson = JsonSerializer.Serialize(new
            {
                Board = move.Board,
                GameState = move.GameState,
                Player = move.Player,
                Winner = room.Winner
            });

            foreach (var socket in _sockets)
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(Encoding.UTF8.GetBytes(moveJson), WebSocketMessageType.Text, true, default);
                }
            }
        }
        public async Task SinglePlayerMove(PlayerMove move)
        {

            var room = GameRoom.GameRooms.FirstOrDefault(x => x.RoomId == move.RoomId);
            if (room == null)
            {
                return;
            }

            room.Board = move.Board;
            var (state, updatedBoard) = TicTacToeGame.CheckGameState(room.Board);
            room.Board = updatedBoard;

            if (state == GameState.Win)
            {
                move.GameState = (GameState)1;
                room.Winner = move.Player;
            }
            else if (state == GameState.Draw)
            {
                move.GameState = (GameState)2;
            }
            else
            {
                move.GameState = GameState.StillPlaying;
            }

            var moveJson = JsonSerializer.Serialize(new
            {
                Board = move.Board,
                GameState = move.GameState,
                Player = move.Player,
                Winner = room.Winner
            });

            foreach (var socket in _sockets)
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(Encoding.UTF8.GetBytes(moveJson), WebSocketMessageType.Text, true, default);
                }
            }

            // AI Move
            if (state == GameState.StillPlaying && move.Player == "O")
            {
                room.Board = TicTacToeGame.MakeComputerMove(room.Board, room.Difficulty);
                (state, updatedBoard) = TicTacToeGame.CheckGameState(room.Board);
                room.Board = updatedBoard;

                if (state == GameState.Win)
                {
                    move.GameState = (GameState)1;
                    room.Winner = "X";
                }
                else if (state == GameState.Draw)
                {
                    move.GameState = (GameState)2;
                }
                else
                {
                    move.GameState = GameState.StillPlaying;
                }

                    var computerMove = new PlayerMove
                {
                    RoomId = move.RoomId,
                    Board = room.Board,
                    Player = "X",
                    GameState = state
                };

                moveJson = JsonSerializer.Serialize(new
                {
                    Board = computerMove.Board,
                    GameState = (int)computerMove.GameState,
                    Player = computerMove.Player
                });

                foreach (var socket in _sockets)
                {
                    if (socket.State == WebSocketState.Open)
                    {
                        await socket.SendAsync(Encoding.UTF8.GetBytes(moveJson), WebSocketMessageType.Text, true, default);
                    }
                }
            }

            return;
        }
    }
}
