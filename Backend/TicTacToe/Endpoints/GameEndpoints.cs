using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Services;

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

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> JoinRoom(Guid roomId, WebSocketService webSocketService)
        {
            await webSocketService.JoinGameRoom(roomId);
            return Results.Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> LeaveRoom(Guid roomId, WebSocketService webSocketService)
        {
            await webSocketService.LeaveGameRoom(roomId);
            return Results.Ok();
        }
    }
}





/*
Plan B: In case WebSocketService is not available
We are supposed to implement the JoinRoom and LeaveRoom methods in diffrent way
*/

    //     [ProducesResponseType(StatusCodes.Status200OK)]
    //     [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //     private static async Task<IResult> JoinRoom(IGameRepository gameRepository, Guid roomId)
    //     {
    //         var room = await gameRepository.JoinGameRoom(roomId);
    //         if (room == null)
    //         {
    //             return Results.BadRequest();
    //         }
    //         return Results.Ok(room);
    //     }

    //     [ProducesResponseType(StatusCodes.Status200OK)]
    //     [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //     private static async Task<IResult> LeaveRoom(IGameRepository gameRepository, Guid roomId)
    //     {
    //         var room = await gameRepository.LeaveGameRoom(roomId);
    //         if (room == null)
    //         {
    //             return Results.BadRequest();
    //         }
    //         return Results.Ok(room);
    // }