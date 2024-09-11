using System.Text.Json;
using Models;
using GameLogic;
using Azure.Messaging.WebPubSub;
using Microsoft.Extensions.Options;

namespace Services
{
    public class WebSocketService
    {
        private readonly WebPubSubServiceClient _pubSubServiceClient;

        public WebSocketService(IOptions<AzureWebPubSubSettings> settings)
        {
            var config = settings.Value;
            _pubSubServiceClient = new WebPubSubServiceClient(config.ConnectionString, config.HubName);
        }

        public async Task<IResult> HandleWebSocketConnection()
        {
            var uri = _pubSubServiceClient.GetClientAccessUri(TimeSpan.FromMinutes(60));
            Console.WriteLine("Connection established with URI: " + uri.AbsoluteUri);
            return Results.Ok(new { uri = uri });
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
