using Microsoft.AspNetCore.Mvc;
using Repository;
using Services;
using Models;

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
            game.MapPost("rooms/{roomId}/move", PlayerMove);
        }


        private static async Task GetWebSocketConnection(HttpContext context, WebSocketService webSocketService)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                await webSocketService.HandleWebSocketConnection(webSocket);
            }
            else
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Expected a WebSocket request");
            }
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
        private static async Task<IResult> JoinRoom(Guid roomId, [FromQuery] string playerName, WebSocketService webSocketService)
        {
            await webSocketService.JoinGameRoom(roomId, playerName);
            return Results.Ok();
        }


        // An endpoint to leave a game room in real-time
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        private static async Task<IResult> PlayerMove(PlayerMove move, WebSocketService webSocketService)
        {
            var board = await webSocketService.PlayerMove(move);

            if (board == null)
                return Results.BadRequest();

            return Results.Ok();
        }
    }
}