using Microsoft.AspNetCore.Mvc;
using Repository;
using Services;
using Models;
using Microsoft.AspNetCore.Cors;

namespace Endpoints
{
    public static class GameEndpoints
    {
        public static void ConfigureTicTacToeEndpoints(this WebApplication app)
        {
            var game = app.MapGroup("/tictactoe");
            game.MapGet("/", GetWebSocketConnection);
            game.MapGet("rooms", GetAllRooms);
            game.MapGet("rooms/{roomId}", GetRoom);
            game.MapPost("rooms/{roomId}/join", JoinRoom);
            game.MapPost("rooms/{roomId}/leave", LeaveRoom);
            game.MapPost("rooms/{roomId}/MultiPlayerMove", MultiPlayerMove);
            game.MapPost("rooms/{roomId}/SinglePlayerMove", SinglePlayerMove);
            game.MapPost("rooms/create", CreateRoom);
            game.MapDelete("rooms/{roomId}/delete", DeleteRoom);
            game.MapPost("rooms/{roomId}/reset", ResetGame);
        }

        [EnableCors]
        private static async Task<IResult> GetWebSocketConnection(WebSocketService webSocketService)
        {
            var connection = await webSocketService.HandleWebSocketConnection();
            return Results.Ok(connection);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> GetAllRooms(IGameRepository gameRepository)
        {
            var rooms = await gameRepository.GetGameRooms();
            return Results.Ok(rooms);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> GetRoom(IGameRepository gameRepository, Guid roomId)
        {
            var room = await gameRepository.GetGameRoom(roomId);
            if (room == null)
            {
                return Results.BadRequest();
            }
            return Results.Ok(room);
        }

        // An endpoint to join a game room in real-time
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [EnableCors]
        private static async Task<IResult> JoinRoom(Guid roomId, [FromQuery] string playerName, WebSocketService webSocketService)
        {
            await webSocketService.JoinGameRoom(roomId, playerName);
            return Results.Ok();
        }


        // An endpoint to leave a game room in real-time
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [EnableCors]
        private static async Task<IResult> LeaveRoom(Guid roomId, WebSocketService webSocketService, IGameRepository gameRepository)
        {
            var room = await gameRepository.GetGameRoom(roomId);
            if (room.RoomCapacity == 0)
            {
                return Results.BadRequest();
            }
            await webSocketService.LeaveGameRoom(roomId);
            return Results.Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [EnableCors]
        private static async Task<IResult> MultiPlayerMove(PlayerMove move, WebSocketService webSocketService)
        {
            await webSocketService.MultiPlayerMove(move);
            return Results.Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [EnableCors]
        public static async Task<IResult> SinglePlayerMove(PlayerMove move, WebSocketService webSocketService)
        {
            await webSocketService.SinglePlayerMove(move);
            return Results.Ok();
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [EnableCors]
        private static async Task<IResult> CreateRoom(IGameRepository gameRepository, [FromQuery] string playerName, [FromQuery] string difficulty)
        {
            if (string.IsNullOrEmpty(playerName) || string.IsNullOrEmpty(difficulty))
            {
                return Results.BadRequest();
            }
            if (difficulty != "Easy" && difficulty != "Hard")
            {
                return Results.BadRequest();
            }
            await gameRepository.CreateSinglePlayerRoom(playerName, difficulty);

            // Return the new created room
            var rooms = await gameRepository.GetGameRooms();
            return Results.Ok(rooms.LastOrDefault());
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [EnableCors]
        private static async Task<IResult> DeleteRoom(Guid roomId, IGameRepository gameRepository)
        {
            if (await gameRepository.GetGameRoom(roomId) == null)
            {
                return Results.BadRequest();
            }
            await gameRepository.DeleteGameRoom(roomId);
            return Results.Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [EnableCors]
        private static async Task<IResult> ResetGame(IGameRepository gameRepository, Guid roomId, WebSocketService webSocketService)
        {
            await webSocketService.ResetGame(roomId);
            var currentRoom = await gameRepository.GetGameRoom(roomId);
            return Results.Ok(currentRoom);
        }
    }
}