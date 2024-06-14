using Azure.Messaging.WebPubSub;
using System.Text.Json;
using Models;
using GameLogic;

namespace Services
{
    public class WebSocketService
    {
        private readonly WebPubSubServiceClient _pubSubServiceClient;


        public WebSocketService(WebPubSubServiceClient pubSubServiceClient)
        {
            _pubSubServiceClient = pubSubServiceClient;
        }

        public async Task HandleWebSocketConnection(HttpContext context)
        {
            var clientUri = _pubSubServiceClient.GetClientAccessUri();

            context.Response.Redirect(clientUri.ToString());
            Console.WriteLine("Connection established with URI: " + clientUri.AbsoluteUri);
        }

        public async Task JoinGameRoom(Guid roomId, string playerName)
        {
            var room = GameRoom.GameRooms.FirstOrDefault(x => x.RoomId == roomId);
            if (room == null || room.RoomCapacity >= 2)
            {
                return;
            }

            // Assign players to the room
            if (room.PlayerX == playerName)
            {
                room.PlayerX = playerName;
            }
            else if (room.PlayerO == playerName)
            {
                room.PlayerO = playerName;
            }
            else if (room.RoomCapacity == 0)
            {
                room.PlayerO = playerName;
            }
            else if (room.RoomCapacity == 1)
            {
                room.PlayerX = playerName;
            }

            room.RoomCapacity++;

            // Send updated room info to all clients
            await _pubSubServiceClient.SendToAllAsync(JsonSerializer.Serialize(room));
        }

        public async Task LeaveGameRoom(Guid roomId)
        {
            var room = GameRoom.GameRooms.FirstOrDefault(x => x.RoomId == roomId);
            if (room == null)
            {
                return;
            }

            room.RoomCapacity--;

            // Send updated room info to all clients
            await _pubSubServiceClient.SendToAllAsync(JsonSerializer.Serialize(room));
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

            if (state == GameState.Win)
            {
                move.GameState = GameState.Win;
                room.Winner = move.Player;
            }
            else if (state == GameState.Draw)
            {
                move.GameState = GameState.Draw;
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

            await _pubSubServiceClient.SendToAllAsync(moveJson);
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
                move.GameState = GameState.Win;
                room.Winner = move.Player;
            }
            else if (state == GameState.Draw)
            {
                move.GameState = GameState.Draw;
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

            await _pubSubServiceClient.SendToAllAsync(moveJson);

            // AI Move
            if (state == GameState.StillPlaying && move.Player == "O")
            {
                room.Board = TicTacToeGame.MakeComputerMove(room.Board, room.Difficulty);
                (state, updatedBoard) = TicTacToeGame.CheckGameState(room.Board);
                room.Board = updatedBoard;

                if (state == GameState.Win)
                {
                    move.GameState = GameState.Win;
                    room.Winner = "X";
                }
                else if (state == GameState.Draw)
                {
                    move.GameState = GameState.Draw;
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

                await _pubSubServiceClient.SendToAllAsync(moveJson);
            }
        }

        public async Task ResetGame(Guid roomId)
        {
            var room = GameRoom.GameRooms.FirstOrDefault(x => x.RoomId == roomId);
            if (room == null)
            {
                return;
            }

            room.Board = new int[9]; // Reset the board
            room.Winner = null; // Reset the winner
            var moveJson = JsonSerializer.Serialize(new
            {
                Board = room.Board,
                GameState = GameState.StillPlaying,
                Player = "",
                Winner = room.Winner
            });

            await _pubSubServiceClient.SendToAllAsync(moveJson);
        }
    }
}


// using System.Net.WebSockets;
// using System.Text.Json;
// using System.Text;
// using Models;
// using GameLogic;

// namespace Services
// {
//     public class WebSocketService
//     {
//         private readonly List<WebSocket> _sockets = new();

//         public async Task HandleWebSocketConnection(WebSocket socket)
//         {
//             _sockets.Add(socket);
//             Console.WriteLine("Connection established");
//             try
//             {
//                 var buffer = new byte[1024 * 2];
//                 while (socket.State == WebSocketState.Open)
//                 {
//                     var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), default);
//                     if (result.MessageType == WebSocketMessageType.Close)
//                     {
//                         await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, default);
//                         Console.WriteLine("Connection closed by the client");
//                         break;
//                     }

//                     //Debugging
//                     var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

//                     foreach (var s in _sockets.Where(s => s != socket && s.State == WebSocketState.Open))
//                     {
//                         if (s.State == WebSocketState.Open)
//                         {
//                             //    await s.SendAsync(buffer[..result.Count], WebSocketMessageType.Text, true, default);
//                             await s.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)), WebSocketMessageType.Text, true, default);
//                         }
//                     }
//                 }
//             }
//             finally
//             {
//                 _sockets.Remove(socket);
//                 await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "WebSocket connection closed", default);
//                 Console.WriteLine("WebSocket connection closed due to an error.");
//             }
//         }

//         // A method to join a game room
//         public async Task JoinGameRoom(Guid roomId, string playerName)
//         {
//             var room = GameRoom.GameRooms.FirstOrDefault(x => x.RoomId == roomId);
//             if (room == null || room.RoomCapacity >= 2)
//             {
//                 return;
//             }

//             // Check if the player was previously in the room and assign them their previous position
//             if (room.PlayerX == playerName)
//             {
//                 room.PlayerX = playerName;
//             }
//             else if (room.PlayerO == playerName)
//             {
//                 room.PlayerO = playerName;
//             }

//             // Assign the first player to join the room to O and the second player to X
//             else if (room.RoomCapacity == 0)
//             {
//                 room.PlayerO = playerName;
//             }
//             else if (room.RoomCapacity == 1)
//             {
//                 room.PlayerX = playerName;
//             }

//             if (_sockets[0].State == WebSocketState.Open)
//             {
//                 room.RoomCapacity++;
//                 await _sockets[0].SendAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(room)), WebSocketMessageType.Text, true, default);
//             }


//         }

//         // A method to leave a game room in real-time
//         public async Task LeaveGameRoom(Guid roomId)
//         {
//             var room = GameRoom.GameRooms.FirstOrDefault(x => x.RoomId == roomId);
//             if (room == null)
//             {
//                 return;
//             }

//             if (room.RoomCapacity == 0)
//             {
//                 return;
//             }
//             if (_sockets[0].State == WebSocketState.Open)
//             {
//                 room.RoomCapacity--;
//                 await _sockets[0].SendAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(room)), WebSocketMessageType.Text, true, default);
//             }

//         }

//         public async Task MultiPlayerMove(PlayerMove move)
//         {
//             var room = GameRoom.GameRooms.FirstOrDefault(x => x.RoomId == move.RoomId);
//             if (room == null)
//             {
//                 return;
//             }

//             room.Board = move.Board;
//             var (state, updatedBoard) = TicTacToeGame.CheckGameState(room.Board);
//             room.Board = updatedBoard;


//             // Update the game state to 1 (win) or 2 (draw)
//             if (state == GameState.Win)
//             {
//                 move.GameState = (GameState)1;
//                 room.Winner = move.Player;
//             }
//             else if (state == GameState.Draw)
//             {
//                 move.GameState = (GameState)2;
//             }
//             else
//             {
//                 move.GameState = GameState.StillPlaying;
//             }

//             var moveJson = JsonSerializer.Serialize(new
//             {
//                 Board = move.Board,
//                 GameState = move.GameState,
//                 Player = move.Player,
//                 Winner = room.Winner
//             });

//             foreach (var socket in _sockets)
//             {
//                 if (socket.State == WebSocketState.Open)
//                 {
//                     await socket.SendAsync(Encoding.UTF8.GetBytes(moveJson), WebSocketMessageType.Text, true, default);
//                 }
//             }
//         }
//         public async Task SinglePlayerMove(PlayerMove move)
//         {

//             var room = GameRoom.GameRooms.FirstOrDefault(x => x.RoomId == move.RoomId);
//             if (room == null)
//             {
//                 return;
//             }

//             room.Board = move.Board;
//             var (state, updatedBoard) = TicTacToeGame.CheckGameState(room.Board);
//             room.Board = updatedBoard;

//             if (state == GameState.Win)
//             {
//                 move.GameState = (GameState)1;
//                 room.Winner = move.Player;
//             }
//             else if (state == GameState.Draw)
//             {
//                 move.GameState = (GameState)2;
//             }
//             else
//             {
//                 move.GameState = GameState.StillPlaying;
//             }

//             var moveJson = JsonSerializer.Serialize(new
//             {
//                 Board = move.Board,
//                 GameState = move.GameState,
//                 Player = move.Player,
//                 Winner = room.Winner
//             });

//             foreach (var socket in _sockets)
//             {
//                 if (socket.State == WebSocketState.Open)
//                 {
//                     await socket.SendAsync(Encoding.UTF8.GetBytes(moveJson), WebSocketMessageType.Text, true, default);
//                 }
//             }

//             // AI Move
//             if (state == GameState.StillPlaying && move.Player == "O")
//             {
//                 room.Board = TicTacToeGame.MakeComputerMove(room.Board, room.Difficulty);
//                 (state, updatedBoard) = TicTacToeGame.CheckGameState(room.Board);
//                 room.Board = updatedBoard;

//                 if (state == GameState.Win)
//                 {
//                     move.GameState = (GameState)1;
//                     room.Winner = "X";
//                 }
//                 else if (state == GameState.Draw)
//                 {
//                     move.GameState = (GameState)2;
//                 }
//                 else
//                 {
//                     move.GameState = GameState.StillPlaying;
//                 }

//                 var computerMove = new PlayerMove
//                 {
//                     RoomId = move.RoomId,
//                     Board = room.Board,
//                     Player = "X",
//                     GameState = state
//                 };

//                 moveJson = JsonSerializer.Serialize(new
//                 {
//                     Board = computerMove.Board,
//                     GameState = (int)computerMove.GameState,
//                     Player = computerMove.Player
//                 });

//                 foreach (var socket in _sockets)
//                 {
//                     if (socket.State == WebSocketState.Open)
//                     {
//                         await socket.SendAsync(Encoding.UTF8.GetBytes(moveJson), WebSocketMessageType.Text, true, default);
//                     }
//                 }
//             }

//             return;
//         }

//         public async Task ResetGame(Guid roomId)
//         {
//             var room = GameRoom.GameRooms.FirstOrDefault(x => x.RoomId == roomId);
//             if (room == null)
//             {
//                 return;
//             }

//             room.Board = new int[9]; // Reset the board
//             room.Winner = null; // Reset the winner
//             var moveJson = JsonSerializer.Serialize(new
//             {
//                 Board = room.Board,
//                 GameState = (GameState)0, // Reset the game state to the initial state
//                 Player = "",
//                 Winner = room.Winner
//             });

//             foreach (var socket in _sockets)
//             {
//                 if (socket.State == WebSocketState.Open)
//                 {
//                     await socket.SendAsync(Encoding.UTF8.GetBytes(moveJson), WebSocketMessageType.Text, true, default);
//                 }
//             }
//         }
//     }
// }
